namespace Shops.Exceptions;

public class ProductException : Exception
{
    private ProductException(string product)
    {
        Product = product;
    }

    public string Product { get; }

    public static ProductException InvalidAmountOfProduct(string product) => new ProductException(product);
}