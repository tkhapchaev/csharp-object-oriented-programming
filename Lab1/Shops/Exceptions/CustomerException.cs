namespace Shops.Exceptions;

public class CustomerException : Exception
{
    private CustomerException(string customer)
    {
        Customer = customer;
    }

    public string Customer { get; }

    public static CustomerException CustomerHasNoEnoughMoney(string customer) => new CustomerException(customer);
}