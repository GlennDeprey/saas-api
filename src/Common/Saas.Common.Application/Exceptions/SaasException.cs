using Saas.Common.Domain;

namespace Saas.Common.Application.Exceptions;

#pragma warning disable RCS1194
public sealed class SaasException : Exception
{
    public SaasException(string requestName, Error? error = default, Exception? innerException = default)
        : base("Application exception", innerException)
    {
        RequestName = requestName;
        Error = error;
    }

    public string RequestName { get; }

    public Error? Error { get; }
}
