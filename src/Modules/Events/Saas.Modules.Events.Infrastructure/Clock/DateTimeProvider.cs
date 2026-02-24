using Saas.Modules.Events.Application.Abstractions.Clock;

namespace Saas.Modules.Events.Infrastructure.Clock;

internal sealed class DateTimeProvider : IDateTimeProvider
{
    public DateTime UtcNow => DateTime.UtcNow;
}
