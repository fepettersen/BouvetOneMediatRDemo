using BouvetOne.MediatR.Core.Domain;
using BouvetOne.MediatR.Core.Features.WeatherForecastFeatures;
using MediatR;
using Microsoft.AspNetCore.Mvc;

namespace BouvetOne.MediatR.Controllers;

[ApiController]
[Route("[controller]")]
public class WeatherForecastController : ControllerBase
{
    private static readonly string[] Summaries = new[]
    {
        "Freezing", "Bracing", "Chilly", "Cool", "Mild", "Warm", "Balmy", "Hot", "Sweltering", "Scorching"
    };

    private readonly ILogger<WeatherForecastController> _logger;
    private readonly IMediator _mediator;

    public WeatherForecastController(ILogger<WeatherForecastController> logger, IMediator mediator)
    {
        _logger = logger;
        _mediator = mediator;
    }

    [HttpGet("GetWeatherForecast")]
    public IEnumerable<WeatherForecast> Get()
    {
        return Enumerable.Range(1, 5).Select(index => new WeatherForecast
            {
                Date = DateTime.Now.AddDays(index),
                TemperatureC = Random.Shared.Next(-20, 55),
                Summary = Summaries[Random.Shared.Next(Summaries.Length)]
            })
            .ToArray();
    }
        
    [HttpGet("GetWithMediatR")]
    public async Task<IList<WeatherForecast>> GetWithMediatR()
    {
        return await _mediator.Send(new GetWeatherForecastQuery());
    }

    [HttpPost("AddNewSummary")]
    public async Task<bool> AddNewSummary(string summary)
    {
        var response = await _mediator.Send(new AddSummaryCommand(summary));
        return response.Result;
    }

    [HttpGet("GetSummaries")]
    public async Task<IList<string>> GetSummaries()
    {
        return await _mediator.Send(new GetSummariesQuery());
    }
}


