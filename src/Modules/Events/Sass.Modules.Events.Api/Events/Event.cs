namespace Sass.Modules.Events.Api.Events;

public sealed class Event
{
    public Guid Id { get; init; }
    public required string Title { get; set; }
    public required string Description { get; set; }
    public required string Location { get; set; }
    public DateTime StartAtUtc { get; set; }
    public DateTime? EndAtUtc { get; set; }
    public EventStatus Status { get; set; }
}
