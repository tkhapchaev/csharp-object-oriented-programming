using Shops.Exceptions;

namespace Shops.Entities;

public class Consignment
{
    public Consignment(Product product, Shop shop, decimal price, int amount)
    {
        Product = product ?? throw new ArgumentNullException();

        Shop = shop ?? throw new ArgumentNullException();

        if (price <= 0)
        {
            throw MoneyException.InvalidAmountOfMoney(price);
        }

        Price = price;

        if (amount <= 0)
        {
            throw ProductException.InvalidAmountOfProduct(Product.Name);
        }

        Amount = amount;
    }

    public Product Product { get; }

    public Shop Shop { get; }

    public decimal Price { get; private set; }

    public int Amount { get; private set; }

    public void ChangePrice(decimal newPrice)
    {
        if (newPrice <= 0)
        {
            throw MoneyException.InvalidAmountOfMoney(newPrice);
        }

        Price = newPrice;
    }

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