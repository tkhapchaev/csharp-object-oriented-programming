using Shops.Entities;
using Shops.Exceptions;

namespace Shops.Models;

public class ShoppingCartItem
{
    public ShoppingCartItem(Product product, int amount)
    {
        Product = product ?? throw new ArgumentNullException();

        if (amount <= 0)
        {
            throw ProductException.InvalidAmountOfProduct(product.Name);
        }

        Amount = amount;
    }

    public Product Product { get; }

    public int Amount { get; private set; }

    public void IncreaseProductAmount(int value)
    {
        if (value <= 0)
        {
            throw ProductException.InvalidAmountOfProduct(Product.Name);
        }

        Amount += value;
    }

    public void ReduceProductAmount(int value)
    {
        if (value <= 0)
        {
            throw ProductException.InvalidAmountOfProduct(Product.Name);
        }

        Amount -= value;
    }
}