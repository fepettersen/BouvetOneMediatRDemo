using BouvetOne.MediatR.Core.Domain;
using BouvetOne.MediatR.Core.Features.WeatherForecastFeatures;
using BouvetOne.MediatR.Extensions;
using MediatR;
using Microsoft.Extensions.DependencyInjection;
using System.Collections.Generic;
using System.Threading;
using System.Threading.Tasks;
using Xunit;

namespace BouvetOne.MediatR.Tests;

public class WeatherForecastQueryHandlerTests
{
    [Fact]
    public async Task CanHandleQuery()
    {
        var handler = new WeatherForecastQueryHandler();
        var query = new GetWeatherForecastQuery();

        var result = await handler.Handle(query, CancellationToken.None);

        Assert.NotEmpty(result);
    }

    [Fact]
    public void QueryHandlerImplementsIRequestHandler()
    {
        var handler = new WeatherForecastQueryHandler();
        Assert.True(new GetWeatherForecastQuery() is IRequest<IEnumerable<WeatherForecast>>);
        Assert.True(handler is IRequestHandler<GetWeatherForecastQuery, IEnumerable<WeatherForecast>>);
    }

    [Fact]
    public async Task DIWorks()
    {
        var mediator = new ServiceCollection()
            .AddLogging()
            .AddMediatR()
            .BuildServiceProvider()
            .GetRequiredService<IMediator>();
            
        var result = await mediator.Send(new GetWeatherForecastQuery());

        Assert.NotEmpty(result);
    }
}