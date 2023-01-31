using Banks.Exceptions;

namespace Banks.Models;

public class Address
{
    public Address(string country, string city, string street, int houseNumber)
    {
        Country = country ?? throw new ArgumentNullException(nameof(country));

        City = city ?? throw new ArgumentNullException(nameof(city));

        Street = street ?? throw new ArgumentNullException(nameof(street));

        if (houseNumber <= 0)
        {
            throw AddressException.InvalidHouseNumber(houseNumber);
        }

        HouseNumber = houseNumber;
    }

    public string Country { get; }

    public string City { get; }

    public string Street { get; }

    public int HouseNumber { get; }

    public override string ToString()
    {
        return $"Country: {Country}; City: {City}; Street: {Street}; House number: {HouseNumber}.";
    }
}