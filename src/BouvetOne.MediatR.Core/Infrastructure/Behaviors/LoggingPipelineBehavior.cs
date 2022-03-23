using MediatR;
using Microsoft.Extensions.Logging;

namespace BouvetOne.MediatR.Core.Infrastructure.Behaviors;

public class LoggingPipelineBehavior<TRequest, TResponse> : IPipelineBehavior<TRequest, TResponse> where TRequest : IRequest<TResponse>
{
    private readonly ILogger<LoggingPipelineBehavior<TRequest, TResponse>> _logger;

    public LoggingPipelineBehavior(ILogger<LoggingPipelineBehavior<TRequest, TResponse>> logger) => _logger = logger;

    public async Task<TResponse> Handle(TRequest request, CancellationToken cancellationToken, RequestHandlerDelegate<TResponse> next)
    {
        try
        {
            return await next().ConfigureAwait(false);

        }
        catch (Exception ex)
        {
            _logger.LogError(ex, $"{request.GetType().FullName} : {ex.Message}", ex.GetBaseException());
            throw;
        }
    }
}