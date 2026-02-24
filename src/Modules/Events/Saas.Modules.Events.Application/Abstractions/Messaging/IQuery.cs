using Saas.Modules.Events.Domain.Abstractions;
using MediatR;

namespace Saas.Modules.Events.Application.Abstractions.Messaging;

public interface IQuery<TResponse> : IRequest<Result<TResponse>>;
