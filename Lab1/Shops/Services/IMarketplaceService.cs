using Shops.Entities;
using Shops.Models;

namespace Shops.Services;

public interface IMarketplaceService
{
    Shop AddShop(string name, Address address);

    void RemoveShop(Shop shop);

    Customer AddCustomer(string name, Address address, decimal moneyBalance);

    void RemoveCustomer(Customer customer);

    Product AddProduct(string name);

    void RemoveProduct(Product product);

    Consignment AddConsignmentToShop(Shop shop, Product product, decimal price, int amount);

    void RemoveConsignmentFromShop(Consignment consignment);

    void ChangeProductPrice(Shop shop, Product product, decimal newPrice);

    decimal GetConsignmentTotalPrice(Shop shop, Product product, int amount);

    Order MakeOrder(Customer customer, Shop shop, Product product, int amount);

    Shop GetShopWithTheLowestPrice(Product product, int amount);

    Shop GetShopWithTheLowestPrice(Customer customer, ShoppingCart shoppingCart);
}