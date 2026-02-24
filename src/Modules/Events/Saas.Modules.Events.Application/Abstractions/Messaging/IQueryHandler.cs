using Saas.Modules.Events.Domain.Abstractions;
using MediatR;

namespace Saas.Modules.Events.Application.Abstractions.Messaging;

public interface IQueryHandler<in TQuery, TResponse> : IRequestHandler<TQuery, Result<TResponse>>
    where TQuery : IQuery<TResponse>;
