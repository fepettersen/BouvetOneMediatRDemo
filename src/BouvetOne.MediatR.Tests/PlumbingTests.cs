using System;
using System.Collections.Generic;
using System.Linq;
using System.Reflection;
using BouvetOne.MediatR.Core.Features.WeatherForecastFeatures;
using MediatR;
using Xunit;
using Xunit.Abstractions;

namespace BouvetOne.MediatR.Tests;

public class PlumbingTests
{
    private readonly ITestOutputHelper _testOutputHelper;

    public PlumbingTests(ITestOutputHelper testOutputHelper)
    {
        _testOutputHelper = testOutputHelper;
    }


    [Fact]
    public void AllRequestsShouldHaveCorrespondingHandler()
    {
        // Arrange
        var assembly = typeof(GetWeatherForecastQuery).GetTypeInfo().Assembly;
        var requests = GetImplementationTypes(assembly, typeof(IRequest<>));
        var handlers = GetImplementationTypes(assembly, typeof(IRequestHandler<>), typeof(IRequestHandler<,>));
        var handledRequestTypes = handlers
            .SelectMany(x => x.GetInterfaces())
            .Select(x => x.GetGenericArguments().First());

        // Act
        var requestsWithoutHandlers = requests.Where(r => !handledRequestTypes.Contains(r)).ToList();

        // Assert
        if (requestsWithoutHandlers.Any())
        {
            _testOutputHelper.WriteLine("Unhandled request types:");
            foreach (var unhandledRequest in requestsWithoutHandlers)
            {
                _testOutputHelper.WriteLine(unhandledRequest.ToString());
            }
        }
        Assert.Empty(requestsWithoutHandlers);
    }

    private static IEnumerable<Type> GetImplementationTypes(Assembly assembly, params Type[] interfaceTypes)
    {
        return from type in assembly.GetTypes()
            where !type.IsAbstract && !type.IsGenericTypeDefinition
            let interfaces =
                from iface in type.GetInterfaces()
                where iface.IsGenericType
                where interfaceTypes.Contains(iface.GetGenericTypeDefinition())
                select iface
            where interfaces.Any()
            select type;
    }
}