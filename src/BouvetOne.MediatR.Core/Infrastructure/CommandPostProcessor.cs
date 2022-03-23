using BouvetOne.MediatR.Core.Common;
using MediatR;
using MediatR.Pipeline;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.Extensions.Logging;

namespace BouvetOne.MediatR.Core.Infrastructure;

public class CommandPostProcessor<TRequest, TResponse> : IRequestPostProcessor<TRequest, TResponse> 
    where TRequest : IRequest<TResponse>

{
    private readonly IMediator _mediator;
    private readonly ILogger<CommandPostProcessor<TRequest, TResponse>> _logger;

    public CommandPostProcessor(IServiceScopeFactory factory, ILogger<CommandPostProcessor<TRequest, TResponse>> logger)
    {
        _mediator = factory.CreateScope().ServiceProvider.GetService<IMediator>()!;
        _logger = logger;
    }

    public async Task Process(TRequest request, TResponse response, CancellationToken cancellationToken)
    {
        if (request is IRequest<CommandResult> command && response is CommandResult { Events: { } } commandResult)
        {
            foreach (var @event in commandResult.Events.Where(x => x != null))
            {
                try
                {
                    await _mediator.Publish(@event, cancellationToken).ConfigureAwait(false);
                }
                catch (Exception ex)
                {
                    _logger.LogCritical(ex.Message, ex);
                    throw;
                }
            }
        }
    }
}