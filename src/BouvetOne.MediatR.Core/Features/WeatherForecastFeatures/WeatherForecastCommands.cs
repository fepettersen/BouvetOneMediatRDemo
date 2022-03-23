using BouvetOne.MediatR.Core.Common;
using MediatR;

namespace BouvetOne.MediatR.Core.Features.WeatherForecastFeatures;

public class AddSummaryCommand : IRequest<CommandResult<bool>>
{
    public string Summary { get; set; }
    public AddSummaryCommand(string summary) => Summary = summary;
}
