using Saas.Common.Application.Exceptions;
using MediatR;
using Microsoft.Extensions.Logging;

namespace Saas.Common.Application.Behaviors;

internal partial class ExceptionHandlingPipelineBehavior<TRequest, TResponse>: IPipelineBehavior<TRequest, TResponse>
    where TRequest : class
{
    private readonly ILogger<ExceptionHandlingPipelineBehavior<TRequest, TResponse>> _logger;
    public ExceptionHandlingPipelineBehavior(
        ILogger<ExceptionHandlingPipelineBehavior<TRequest, TResponse>> logger)
    {
        _logger = logger;
    }

    public async Task<TResponse> Handle(
        TRequest request,
        RequestHandlerDelegate<TResponse> next,
        CancellationToken cancellationToken)
    {
        try
        {
            return await next(cancellationToken);
        }
        catch (Exception exception)
        {
            LogUnhandledExceptionRequest(exception, typeof(TRequest).Name);

            throw new SaasException(typeof(TRequest).Name, innerException: exception);
        }
    }

    #region Logging
    [LoggerMessage(Level = LogLevel.Error, Message = "Unhandled exception for {RequestName}")]
    private partial void LogUnhandledExceptionRequest(Exception expection, string requestName);

    #endregion
}

