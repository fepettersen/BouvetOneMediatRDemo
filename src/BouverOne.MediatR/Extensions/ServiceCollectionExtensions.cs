using BouvetOne.MediatR.Core.Infrastructure;
using BouvetOne.MediatR.Core.Infrastructure.Behaviors;
using MediatR;

namespace BouvetOne.MediatR.Extensions;

public static class ServiceCollectionExtensions
{
    public static IServiceCollection AddMediatR(this IServiceCollection services)
    {
        return services.AddMediatR(configuration =>
        {
            configuration.Using<BouvetOneMediator>();
        //}, AppDomain.CurrentDomain.GetAssemblies())
        }, AppDomain.CurrentDomain.Load("BouvetOne.MediatR.Core"))
        // Note! To ensure it runs AFTER the LoggingPipelineBehavior on the way BACK from the RequestHandler,
        // ErrorHandlingPipelineBehavior must be registered before LoggingPipelineBehavior!
        //.AddTransient(typeof(IPipelineBehavior<,>), typeof(ErrorHandlingPipelineBehavior<,>))
        .AddTransient(typeof(IPipelineBehavior<,>), typeof(CachePipelineBehavior<,>))
        .AddTransient(typeof(IPipelineBehavior<,>), typeof(CommandValidationPipelineBehavior<,>))
        .AddTransient(typeof(IPipelineBehavior<,>), typeof(LoggingPipelineBehavior<,>))
        ;
    }
}