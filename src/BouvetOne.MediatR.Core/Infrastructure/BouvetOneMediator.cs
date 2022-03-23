using MediatR;
using Microsoft.Extensions.Logging;

namespace BouvetOne.MediatR.Core.Infrastructure;

public class BouvetOneMediator : Mediator
{
    private readonly ILogger<BouvetOneMediator> _logger;
    public BouvetOneMediator(ServiceFactory serviceFactory, ILogger<BouvetOneMediator> logger) : base(serviceFactory) => _logger = logger;

    protected override Task PublishCore(IEnumerable<Func<INotification, CancellationToken, Task>> allHandlers, INotification notification, CancellationToken cancellationToken)
    {
        Parallel.ForEach(allHandlers, async handler =>
        {
            await handler(notification, cancellationToken).ContinueWith(x =>
            {
                if (x.Exception != null)
                {
                    _logger.LogError(x.Exception, x.Exception.GetBaseException().Message);
                }
            }, cancellationToken);
        });

        return Task.CompletedTask;
    }
}