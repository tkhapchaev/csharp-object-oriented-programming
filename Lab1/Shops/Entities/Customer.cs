using Shops.Exceptions;
using Shops.Models;

namespace Shops.Entities;

public class Customer
{
    public Customer(string name, int id, Address address, decimal moneyBalance)
    {
        Name = name ?? throw new ArgumentNullException();

        Address = address ?? throw new ArgumentNullException();

        if (moneyBalance < 0)
        {
            throw MoneyException.InvalidAmountOfMoney(moneyBalance);
        }

        MoneyBalance = moneyBalance;

        if (id < 0)
        {
            throw IdException.InvalidIdValue(id);
        }

        Id = id;
    }

    public string Name { get; }

    public Address Address { get; }

    public int Id { get; }

    public decimal MoneyBalance { get; private set; }

    public void TopUpMoneyBalance(decimal value)
    {
        if (value <= 0)
        {
            throw MoneyException.InvalidAmountOfMoney(value);
        }

        MoneyBalance += value;
    }

    public void ReduceMoneyBalance(decimal value)
    {
        if (value <= 0)
        {
            throw MoneyException.InvalidAmountOfMoney(value);
        }

        MoneyBalance -= value;
    }
}