using BouvetOne.MediatR.Core.Common.Caching;
using BouvetOne.MediatR.Core.Domain;
using MediatR;

namespace BouvetOne.MediatR.Core.Features.WeatherForecastFeatures;

public class GetWeatherForecastQuery : IRequest<IList<WeatherForecast>> { }

public class GetSummariesQuery : Cacheable, IRequest<IList<string>> { }