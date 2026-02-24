using Saas.Modules.Events.Application.Abstractions.Messaging;
using Saas.Modules.Events.Application.Categories.GetCategory;

namespace Saas.Modules.Events.Application.Categories.GetCategories;

public sealed record GetCategoriesQuery : IQuery<IReadOnlyCollection<CategoryResponse>>;
