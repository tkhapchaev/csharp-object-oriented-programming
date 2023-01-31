using Shops.Entities;
using Shops.Exceptions;
using Shops.Models;
using Shops.Services;
using Xunit;

namespace Shops.Test;

public class MarketplaceServiceTests
{
    private readonly MarketplaceService _marketplaceService;

    public MarketplaceServiceTests()
    {
        _marketplaceService = new MarketplaceService();
    }

    [Fact]
    public void AddProductToShop_ShopContainsProduct()
    {
        var shopAddress = new Address("Russia", "Saint Petersburg", "Ivanovskaya", 17);

        Shop shop = _marketplaceService.AddShop("Shop", shopAddress);

        Product product1 = _marketplaceService.AddProduct("Product 1");
        Product product2 = _marketplaceService.AddProduct("Product 2");

        Consignment consignment1 = _marketplaceService.AddConsignmentToShop(shop, product1, 1200, 60);
        Consignment consignment2 = _marketplaceService.AddConsignmentToShop(shop, product2, 800, 15);

        Assert.Contains(consignment1, shop.Consignments);
        Assert.Same(product1, consignment1.Product);

        Assert.Contains(consignment2, shop.Consignments);
        Assert.Same(product2, consignment2.Product);
    }

    [Fact]
    public void SetNewProductPrice_ProductPriceHasChanged()
    {
        var shopAddress = new Address("Russia", "Saint Petersburg", "Ivanovskaya", 17);

        Shop shop = _marketplaceService.AddShop("Shop", shopAddress);
        Product product = _marketplaceService.AddProduct("Product");
        Consignment consignment = _marketplaceService.AddConsignmentToShop(shop, product, 100, 30);

        const decimal newPrice = 200;

        _marketplaceService.ChangeProductPrice(shop, product, newPrice);

        Assert.Equal(consignment.Price, newPrice);
    }

    [Fact]
    public void CustomerBuysProduct_TheMostProfitableOrderWasCreated()
    {
        var shopAddress1 = new Address("Russia", "Saint Petersburg", "Ivanovskaya", 17);
        var shopAddress2 = new Address("Russia", "Moscow", "Staraya Basmannaya", 20);
        var customerAddress = new Address("Russia", "Saint Petersburg", "4-ya sovetskaya", 40);

        const decimal expectedPrice = 15200, expectedCustomerBalance = 4800;
        const int amountOfProductInShop = 90, amountOfProductToCustomer = 2;

        Customer customer = _marketplaceService.AddCustomer("Customer", customerAddress, 20000);
        Product product = _marketplaceService.AddProduct("Product");

        Shop shop1 = _marketplaceService.AddShop("Shop 1", shopAddress1);
        _marketplaceService.AddConsignmentToShop(shop1, product, 8000, 20);

        Shop shop2 = _marketplaceService.AddShop("Shop 2", shopAddress2);
        Consignment consignment = _marketplaceService.AddConsignmentToShop(shop2, product, 7600, amountOfProductInShop);

        Shop targetShop = _marketplaceService.GetShopWithTheLowestPrice(product, amountOfProductToCustomer);
        Order order = _marketplaceService.MakeOrder(customer, targetShop, product, amountOfProductToCustomer);

        Assert.Equal(order.TotalPrice, expectedPrice);
        Assert.Equal(shop2.Revenue, expectedPrice);
        Assert.Equal(customer.MoneyBalance, expectedCustomerBalance);
        Assert.Equal(consignment.Amount, amountOfProductInShop - amountOfProductToCustomer);
    }

    [Fact]
    public void NoEnoughProductInTheShop_ThrowException()
    {
        var shopAddress = new Address("Russia", "Saint Petersburg", "Ivanovskaya", 17);
        var customerAddress = new Address("Russia", "Saint Petersburg", "4-ya sovetskaya", 40);

        Customer customer = _marketplaceService.AddCustomer("Customer", customerAddress, 5000);
        Product product = _marketplaceService.AddProduct("Product");
        Shop shop = _marketplaceService.AddShop("Shop", shopAddress);

        _marketplaceService.AddConsignmentToShop(shop, product, 800, 5);

        Assert.Throws<ShopException>(() => _marketplaceService.MakeOrder(customer, shop, product, 10));
    }

    [Fact]
    public void MarketplaceHasNoSuchProduct_ThrowException()
    {
        var shopAddress = new Address("Russia", "Saint Petersburg", "Ivanovskaya", 17);

        Product product1 = _marketplaceService.AddProduct("Product 1");
        Product product2 = _marketplaceService.AddProduct("Product 2");

        Shop shop = _marketplaceService.AddShop("Shop", shopAddress);

        _marketplaceService.AddConsignmentToShop(shop, product1, 800, 5);

        Assert.Throws<MarketplaceException>(
            () => _marketplaceService.GetShopWithTheLowestPrice(product2, 1));
    }

    [Fact]
    public void CustomerHasNoEnoughMoney_ThrowException()
    {
        var shopAddress = new Address("Russia", "Saint Petersburg", "Ivanovskaya", 17);
        var customerAddress = new Address("Russia", "Saint Petersburg", "4-ya sovetskaya", 40);

        Customer customer = _marketplaceService.AddCustomer("Customer", customerAddress, 2000);
        Product product = _marketplaceService.AddProduct("Product");
        Shop shop = _marketplaceService.AddShop("Shop", shopAddress);

        _marketplaceService.AddConsignmentToShop(shop, product, 14000, 40);

        Assert.Throws<CustomerException>(() => _marketplaceService.MakeOrder(customer, shop, product, 1));
    }
}