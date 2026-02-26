using Saas.Common.Application.Messaging;

namespace Saas.Modules.Ticketing.Application.Customers.CreateCustomer;

public sealed record CreateCustomerCommand(Guid CustomerId, string Email, string FirstName, string LastName)
    : ICommand;
