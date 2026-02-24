namespace Sass.Modules.Events.Api.Events;

public sealed record EventResponse(
    Guid Id,
    string Title,
    string Description,
    string Location,
    DateTime StartAtUtc,
    DateTime? EndAtUtc);
