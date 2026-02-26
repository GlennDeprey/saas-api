using Dapper;
using Saas.Common.Application.Data;
using Saas.Common.Application.Messaging;
using Saas.Common.Domain;
using Saas.Modules.Ticketing.Domain.Customers;

namespace Saas.Modules.Ticketing.Application.Customers.GetCustomer;

internal sealed class GetCustomerByIdQueryHandler: IQueryHandler<GetCustomerQuery, CustomerResponse>
{
    private readonly IDbConnectionFactory _dbConnectionFactory;
    public GetCustomerByIdQueryHandler(IDbConnectionFactory dbConnectionFactory)
    {
        _dbConnectionFactory = dbConnectionFactory;
    }

    public async Task<Result<CustomerResponse>> Handle(GetCustomerQuery request, CancellationToken cancellationToken)
    {
        await using var connection = await _dbConnectionFactory.OpenConnectionAsync();

        const string sql =
            $"""
             SELECT
                 id AS {nameof(CustomerResponse.Id)},
                 email AS {nameof(CustomerResponse.Email)},
                 first_name AS {nameof(CustomerResponse.FirstName)},
                 last_name AS {nameof(CustomerResponse.LastName)}
             FROM ticketing.customers
             WHERE id = @CustomerId
             """;

        var customer = await connection.QuerySingleOrDefaultAsync<CustomerResponse>(sql, request);

        return customer is null ? Result.Failure<CustomerResponse>(CustomerErrors.NotFound(request.CustomerId)) :
            (Result<CustomerResponse>)customer;
    }
}
