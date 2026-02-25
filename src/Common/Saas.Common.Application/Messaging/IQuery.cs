using Saas.Common.Domain;
using MediatR;

namespace Saas.Common.Application.Messaging;

public interface IQuery<TResponse> : IRequest<Result<TResponse>>;
