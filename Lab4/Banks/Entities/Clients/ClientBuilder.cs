using Banks.Models;

namespace Banks.Entities.Clients;

public class ClientBuilder : IClientBuilder
{
    private string _name = string.Empty;

    private string _surname = string.Empty;

    private Passport? _passport;

    private Address? _address;

    public IClientBuilder WithName(string name)
    {
        _name = name ?? throw new ArgumentNullException(nameof(name));

        return this;
    }

    public IClientBuilder WithSurname(string surname)
    {
        _surname = surname ?? throw new ArgumentNullException(nameof(surname));

        return this;
    }

    public IClientBuilder WithPassport(Passport? passport)
    {
        _passport = passport;

        return this;
    }

    public IClientBuilder WithAddress(Address? address)
    {
        _address = address;

        return this;
    }

    public Client Build()
    {
        var result = new Client(_name, _surname)
        {
            Passport = _passport,
            Address = _address,
        };

        return result;
    }
}