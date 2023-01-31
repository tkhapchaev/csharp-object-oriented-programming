namespace Shops.Exceptions;

public class ShoppingCartException : Exception
{
    private ShoppingCartException(string product, int amount)
    {
        Product = product;
        Amount = amount;
    }

    public string Product { get; }

    public int Amount { get; }

    public static ShoppingCartException ShoppingCartHasNoSuchItem(string product, int amount) =>
        new ShoppingCartException(product, amount);
}