using Saas.Common.Domain;

namespace Saas.Modules.Ticketing.Domain.Customers;
#pragma warning disable CS8618
public sealed class Customer: Entity
{
    private Customer()
    {
    }

    public Guid Id { get; private set; }

    public string Email { get; private set; }

    public string FirstName { get; private set; }

    public string LastName { get; private set; }

    public static Customer Create(Guid id, string email, string firstName, string lastName)
    {
        return new Customer
        {
            Id = id,
            Email = email,
            FirstName = firstName,
            LastName = lastName
        };
    }

    public void Update(string firstName, string lastName)
    {
        FirstName = firstName;
        LastName = lastName;
    }
}
