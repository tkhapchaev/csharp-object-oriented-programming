using Shops.Exceptions;

namespace Shops.Entities;

public class Order
{
    public Order(Customer customer, Shop shop, ShoppingCart shoppingCart, decimal totalPrice)
    {
        Customer = customer ?? throw new ArgumentNullException();

        Shop = shop ?? throw new ArgumentNullException();

        ShoppingCart = shoppingCart ?? throw new ArgumentNullException();

        if (totalPrice <= 0)
        {
            throw MoneyException.InvalidAmountOfMoney(totalPrice);
        }

        TotalPrice = totalPrice;
    }

    public Shop Shop { get; }

    public Customer Customer { get; }

    public ShoppingCart ShoppingCart { get; }

    public decimal TotalPrice { get; }
}