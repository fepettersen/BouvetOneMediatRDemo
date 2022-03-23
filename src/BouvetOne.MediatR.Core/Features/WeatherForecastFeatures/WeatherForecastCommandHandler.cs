using BouvetOne.MediatR.Core.Common;
using BouvetOne.MediatR.Core.Events;
using MediatR;

namespace BouvetOne.MediatR.Core.Features.WeatherForecastFeatures;

public class WeatherForecastCommandHandler : IRequestHandler<AddSummaryCommand, CommandResult<bool>>
{
    public Task<CommandResult<bool>> Handle(AddSummaryCommand request, CancellationToken cancellationToken) 
        => Task.FromResult(new CommandResult<bool>
        {
            Events = new List<INotification>
            {
                new SummaryAddedEvent(request.Summary)
            },
            Result = true
        });
}