using BouvetOne.MediatR.Core.Domain;
using MediatR;

namespace BouvetOne.MediatR.Core.Features.WeatherForecastFeatures;

public class WeatherForecastQueryHandler : IRequestHandler<GetWeatherForecastQuery, IList<WeatherForecast>>,
    IRequestHandler<GetSummariesQuery, IList<string>>
{
    private static readonly string[] Summaries = {
        "Freezing", "Bracing", "Chilly", "Cool", "Mild", "Warm", "Balmy", "Hot", "Sweltering", "Scorching"
    };

    public WeatherForecastQueryHandler() { }

    public Task<IList<WeatherForecast>> Handle(GetWeatherForecastQuery request, CancellationToken cancellationToken) =>
        Task.FromResult(Enumerable.Range(1, 5).Select(index => new WeatherForecast
        {
            Date = DateTime.Now.AddDays(index),
            TemperatureC = Random.Shared.Next(-20, 55),
            Summary = Summaries[Random.Shared.Next(Summaries.Length)]
        }).ToList() as IList<WeatherForecast>);

    public Task<IList<string>> Handle(GetSummariesQuery request, CancellationToken cancellationToken)
    {
        return Task.FromResult(Summaries as IList<string>);
    }
}