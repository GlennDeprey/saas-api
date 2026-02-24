using Saas.Modules.Events.Application.Abstractions.Messaging;

namespace Saas.Modules.Events.Application.Categories.UpdateCategory;

public sealed record UpdateCategoryCommand(Guid CategoryId, string Name) : ICommand;
