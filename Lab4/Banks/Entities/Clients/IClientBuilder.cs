using Banks.Models;

namespace Banks.Entities.Clients;

public interface IClientBuilder
{
    public IClientBuilder WithName(string name);

    public IClientBuilder WithSurname(string surname);

    public IClientBuilder WithPassport(Passport? passport);

    public IClientBuilder WithAddress(Address? address);

    public Client Build();
}