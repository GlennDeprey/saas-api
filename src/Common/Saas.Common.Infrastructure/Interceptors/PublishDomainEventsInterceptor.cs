using Saas.Common.Domain;
using MediatR;
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Diagnostics;
using Microsoft.Extensions.DependencyInjection;

namespace Saas.Common.Infrastructure.Interceptors;

public sealed class PublishDomainEventsInterceptor: SaveChangesInterceptor
{
    private readonly IServiceScopeFactory _serviceScopeFactory;
    private List<IDomainEvent> _domainEvents = [];

    public PublishDomainEventsInterceptor(IServiceScopeFactory serviceScopeFactory)
    {
        _serviceScopeFactory = serviceScopeFactory;
    }

    public override async ValueTask<InterceptionResult<int>> SavingChangesAsync(
        DbContextEventData eventData,
        InterceptionResult<int> result,
        CancellationToken cancellationToken = default)
    {
        if (eventData.Context is not null)
        {
            _domainEvents = GetDomainEvents(eventData.Context);
        }

        return await base.SavingChangesAsync(eventData, result, cancellationToken);
    }

    public override async ValueTask<int> SavedChangesAsync(
        SaveChangesCompletedEventData eventData,
        int result,
        CancellationToken cancellationToken = default)
    {
        if (_domainEvents.Count > 0)
        {
            await PublishDomainEventsAsync(_domainEvents);
            _domainEvents = [];
        }

        return await base.SavedChangesAsync(eventData, result, cancellationToken);
    }

    private static List<IDomainEvent> GetDomainEvents(DbContext context)
    {
        return [.. context
            .ChangeTracker
            .Entries<Entity>()
            .Select(entry => entry.Entity)
            .SelectMany(entity =>
            {
                var domainEvents = entity.DomainEvents.ToList();
                entity.ClearDomainEvents();
                return domainEvents;
            })];
    }

    private async Task PublishDomainEventsAsync(List<IDomainEvent> domainEvents)
    {
        using var scope = _serviceScopeFactory.CreateScope();
        var publisher = scope.ServiceProvider.GetRequiredService<IPublisher>();

        foreach (var domainEvent in domainEvents)
        {
            await publisher.Publish(domainEvent);
        }
    }
}
