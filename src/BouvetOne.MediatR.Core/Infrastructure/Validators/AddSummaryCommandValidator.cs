using BouvetOne.MediatR.Core.Features.WeatherForecastFeatures;
using FluentValidation;

namespace BouvetOne.MediatR.Core.Infrastructure.Validators;

public class AddSummaryCommandValidator : AbstractValidator<AddSummaryCommand>
{
    public AddSummaryCommandValidator()
    {
        RuleFor(x => x.Summary)
            .MinimumLength(10)
            .MaximumLength(11);
    }
}