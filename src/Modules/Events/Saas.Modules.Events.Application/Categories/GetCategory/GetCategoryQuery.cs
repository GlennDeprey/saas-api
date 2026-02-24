using Saas.Modules.Events.Application.Abstractions.Messaging;

namespace Saas.Modules.Events.Application.Categories.GetCategory;

public sealed record GetCategoryQuery(Guid CategoryId) : IQuery<CategoryResponse>;
