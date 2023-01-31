namespace Shops.Exceptions;

public class ShopException : Exception
{
    private ShopException(string shop, string product)
    {
        Shop = shop;
        Product = product;
    }

    public string Shop { get; }

    public string Product { get; }

    public static ShopException ShopHasNoEnoughProduct(string shop, string product) => new ShopException(shop, product);

    public static ShopException ShopDoesNotContainSuchConsignment(string shop, string product) =>
        new ShopException(shop, product);

    public static ShopException ShopDoesNotContainSuchProduct(string shop, string product) =>
        new ShopException(shop, product);
}