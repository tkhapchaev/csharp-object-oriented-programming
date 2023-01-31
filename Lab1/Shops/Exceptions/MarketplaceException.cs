namespace Shops.Exceptions;

public class MarketplaceException : Exception
{
    private MarketplaceException(string message)
    {
    }

    public static MarketplaceException MarketplaceHasNoSuchCustomer(string customer) =>
        new MarketplaceException($"Marketplace has no customer \"{customer}\".");

    public static MarketplaceException MarketplaceHasNoSuchProduct(string product) =>
        new MarketplaceException($"Marketplace has no product \"{product}\".");

    public static MarketplaceException MarketplaceHasNoSuchShop(string shop) =>
        new MarketplaceException($"Marketplace has no shop \"{shop}\".");

    public static MarketplaceException MarketplaceCannotFindAppropriateShop(string customer) =>
        new MarketplaceException($"Marketplace cannot find appropriate shop for {customer}'s shopping cart.");
}