using Shops.Entities;
using Shops.Exceptions;
using Shops.Models;

namespace Shops.Services;

public class MarketplaceService : IMarketplaceService
{
    private readonly List<Shop> _shops;

    private readonly List<Customer> _customers;

    private readonly List<Product> _products;

    private int _lastRegisteredShopId;

    private int _lastRegisteredCustomerId;

    private int _lastRegisteredProductId;

    public MarketplaceService()
    {
        _shops = new List<Shop>();
        _customers = new List<Customer>();
        _products = new List<Product>();

        _lastRegisteredShopId = 0;
        _lastRegisteredCustomerId = 0;
        _lastRegisteredProductId = 0;
    }

    public IReadOnlyList<Shop> Shops => _shops.AsReadOnly();

    public IReadOnlyList<Customer> Customers => _customers.AsReadOnly();

    public IReadOnlyList<Product> Products => _products.AsReadOnly();

    public Shop AddShop(string name, Address address)
    {
        var shop = new Shop(name, _lastRegisteredShopId, address);
        _lastRegisteredShopId++;
        _shops.Add(shop);

        return shop;
    }

    public void RemoveShop(Shop shop)
    {
        if (!_shops.Contains(shop))
        {
            throw MarketplaceException.MarketplaceHasNoSuchShop(shop.Name);
        }

        _shops.Remove(shop);
    }

    public Customer AddCustomer(string name, Address address, decimal moneyBalance)
    {
        var customer = new Customer(name, _lastRegisteredCustomerId, address, moneyBalance);
        _lastRegisteredCustomerId++;
        _customers.Add(customer);

        return customer;
    }

    public void RemoveCustomer(Customer customer)
    {
        if (!_customers.Contains(customer))
        {
            throw MarketplaceException.MarketplaceHasNoSuchCustomer(customer.Name);
        }

        _customers.Remove(customer);
    }

    public Product AddProduct(string name)
    {
        var product = new Product(name, _lastRegisteredProductId);
        _lastRegisteredProductId++;
        _products.Add(product);

        return product;
    }

    public void RemoveProduct(Product product)
    {
        if (!_products.Contains(product))
        {
            throw MarketplaceException.MarketplaceHasNoSuchProduct(product.Name);
        }

        _products.Remove(product);
    }

    public Consignment AddConsignmentToShop(Shop shop, Product product, decimal price, int amount)
    {
        var consignment = new Consignment(product, shop, price, amount);
        shop.AddConsignment(consignment);

        return consignment;
    }

    public void RemoveConsignmentFromShop(Consignment consignment)
    {
        consignment.Shop.RemoveConsignment(consignment);
    }

    public void ChangeProductPrice(Shop shop, Product product, decimal newPrice)
    {
        foreach (Consignment targetConsignment in shop.Consignments
                     .Where(consignment => consignment.Product == product))
        {
            targetConsignment.ChangePrice(newPrice);
        }
    }

    public decimal GetConsignmentTotalPrice(Shop shop, Product product, int amount)
    {
        foreach (Consignment consignment in shop.Consignments)
        {
            if (consignment.Product != product) continue;

            if (consignment.Amount < amount)
            {
                throw ShopException.ShopHasNoEnoughProduct(shop.Name, product.Name);
            }

            decimal totalPrice = consignment.Price * amount;

            return totalPrice;
        }

        throw ShopException.ShopDoesNotContainSuchProduct(shop.Name, product.Name);
    }

    public Order MakeOrder(Customer customer, Shop shop, Product product, int amount)
    {
        foreach (Consignment consignment in shop.Consignments)
        {
            if (consignment.Product != product) continue;

            if (consignment.Amount < amount)
            {
                throw ShopException.ShopHasNoEnoughProduct(shop.Name, product.Name);
            }

            decimal totalPrice = consignment.Price * amount;

            if (customer.MoneyBalance < totalPrice)
            {
                throw CustomerException.CustomerHasNoEnoughMoney(customer.Name);
            }

            var shoppingCart = new ShoppingCart();
            shoppingCart.AddItem(new ShoppingCartItem(product, amount));

            var order = new Order(customer, shop, shoppingCart, totalPrice);

            consignment.ReduceProductAmount(amount);
            customer.ReduceMoneyBalance(totalPrice);
            shop.IncreaseRevenue(totalPrice);

            return order;
        }

        throw ShopException.ShopDoesNotContainSuchProduct(shop.Name, product.Name);
    }

    public Shop GetShopWithTheLowestPrice(Product product, int amount)
    {
        Consignment? targetConsignment = null;
        decimal minimalPrice = int.MaxValue;

        foreach (Shop shop in _shops)
        {
            foreach (Consignment consignment in shop.Consignments)
            {
                if (consignment.Product != product) continue;

                decimal totalPrice = consignment.Price * amount;

                if (totalPrice < minimalPrice)
                {
                    minimalPrice = totalPrice;
                    targetConsignment = consignment;
                }
            }
        }

        if (targetConsignment is null)
        {
            throw MarketplaceException.MarketplaceHasNoSuchProduct(product.Name);
        }

        return targetConsignment.Shop;
    }

    public Shop GetShopWithTheLowestPrice(Customer customer, ShoppingCart shoppingCart)
    {
        Shop? targetShop = null;
        decimal minimalPrice = int.MaxValue;

        foreach (Shop shop in _shops)
        {
            bool shopHasAllNecessaryProducts = true;
            decimal totalPrice = 0;

            foreach (ShoppingCartItem shoppingCartItem in shoppingCart.ShoppingCartItems)
            {
                try
                {
                    totalPrice += GetConsignmentTotalPrice(shop, shoppingCartItem.Product, shoppingCartItem.Amount);
                }
                catch (ShopException)
                {
                    shopHasAllNecessaryProducts = false;
                    break;
                }
            }

            if (shopHasAllNecessaryProducts && totalPrice < minimalPrice)
            {
                minimalPrice = totalPrice;
                targetShop = shop;
            }
        }

        if (targetShop is null)
        {
            throw MarketplaceException.MarketplaceCannotFindAppropriateShop(customer.Name);
        }

        return targetShop;
    }
}