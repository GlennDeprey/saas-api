using Saas.Common.Domain;

namespace Saas.Modules.Events.Domain.Categories;

public sealed class CategoryCreatedDomainEvent(Guid categoryId) : DomainEvent
{
    public Guid CategoryId { get; init; } = categoryId;
}
