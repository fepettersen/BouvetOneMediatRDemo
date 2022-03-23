using BouvetOne.MediatR.Core.Events;
using MediatR;
using Microsoft.Extensions.Caching.Distributed;

namespace BouvetOne.MediatR.Core.Features.WeatherForecastFeatures;

internal class WeatherForecastNotificationHandler : INotificationHandler<SummaryAddedEvent>
{
    private readonly IDistributedCache _cache;

    public WeatherForecastNotificationHandler(IDistributedCache cache) => _cache = cache;

    public async Task Handle(SummaryAddedEvent notification, CancellationToken cancellationToken)
    {
        await _cache.RemoveAsync(new GetSummariesQuery().CacheId(), cancellationToken);
    }
}