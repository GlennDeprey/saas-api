using Saas.Common.Domain;
using MediatR;
using Microsoft.Extensions.Logging;
using Serilog.Context;

namespace Saas.Common.Application.Behaviors;

internal sealed partial class RequestLoggingPipelineBehavior<TRequest, TResponse>: IPipelineBehavior<TRequest, TResponse>
    where TRequest : class
    where TResponse : Result
{
    private readonly ILogger<RequestLoggingPipelineBehavior<TRequest, TResponse>> _logger;

    public RequestLoggingPipelineBehavior(
        ILogger<RequestLoggingPipelineBehavior<TRequest, TResponse>> logger)
    {
        _logger = logger;
    }

    public async Task<TResponse> Handle(
        TRequest request,
        RequestHandlerDelegate<TResponse> next,
        CancellationToken cancellationToken)
    {
        var moduleName = GetModuleName(typeof(TRequest).FullName!);
        var requestName = typeof(TRequest).Name;

        using (LogContext.PushProperty("Module", moduleName))
        {
            LogProcessingRequest(requestName);

            var result = await next(cancellationToken);

            if (result.IsSuccess)
            {
                LogCompletedRequest(requestName);
            }
            else
            {
                using (LogContext.PushProperty("Error", result.Error, true))
                {
                    LogCompletedRequestWithError(requestName);
                }
            }

            return result;
        }
    }

    private static string GetModuleName(string requestName) => requestName.Split('.')[2];


    #region Logging
    [LoggerMessage(Level = LogLevel.Information, Message = "Processing request {RequestName}")]
    private partial void LogProcessingRequest(string requestName);

    [LoggerMessage(Level = LogLevel.Information, Message = "Completed request {RequestName}")]
    private partial void LogCompletedRequest(string requestName);

    [LoggerMessage(Level = LogLevel.Error, Message = "Completed request {RequestName} with error")]
    private partial void LogCompletedRequestWithError(string requestName);
    #endregion
}