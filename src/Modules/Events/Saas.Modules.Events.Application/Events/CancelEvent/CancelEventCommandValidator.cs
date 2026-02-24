using FluentValidation;

namespace Saas.Modules.Events.Application.Events.CancelEvent;

internal sealed class CancelEventCommandValidator : AbstractValidator<CancelEventCommand>
{
    public CancelEventCommandValidator()
    {
        RuleFor(c => c.EventId).NotEmpty();
    }
}
