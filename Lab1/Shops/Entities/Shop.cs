using Shops.Exceptions;
using Shops.Models;

namespace Shops.Entities;

public class Shop
{
    private readonly List<Consignment> _consignments;

    public Shop(string name, int id, Address address)
    {
        Name = name ?? throw new ArgumentNullException();

        Address = address ?? throw new ArgumentNullException();

        _consignments = new List<Consignment>();

        Revenue = 0;

        if (id < 0)
        {
            throw IdException.InvalidIdValue(id);
        }

        Id = id;
    }

    public string Name { get; }

    public Address Address { get; }

    public int Id { get; }

    public decimal Revenue { get; private set; }

    public IReadOnlyList<Consignment> Consignments => _consignments.AsReadOnly();

    public void AddConsignment(Consignment consignment)
    {
        if (_consignments.Contains(consignment))
        {
            consignment.IncreaseProductAmount(consignment.Amount);
        }
        else
        {
            _consignments.Add(consignment);
        }
    }

    public void RemoveConsignment(Consignment consignment)
    {
        if (_consignments != null && !_consignments.Contains(consignment))
        {
            throw ShopException.ShopDoesNotContainSuchConsignment(Name, consignment.Product.Name);
        }

        _consignments?.Remove(consignment);
    }

    public void IncreaseRevenue(decimal value)
    {
        if (value <= 0)
        {
            throw MoneyException.InvalidAmountOfMoney(value);
        }

        Revenue += value;
    }

    public void ReduceRevenue(decimal value)
    {
        if (value <= 0)
        {
            throw MoneyException.InvalidAmountOfMoney(value);
        }

        Revenue -= value;
    }
}