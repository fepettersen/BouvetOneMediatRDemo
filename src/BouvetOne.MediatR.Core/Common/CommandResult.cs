using MediatR;

namespace BouvetOne.MediatR.Core.Common;

public abstract class CommandResult
{
    internal IEnumerable<INotification> Events { get; set; }
}

public class CommandResult<TResult> : CommandResult
{
    public TResult Result;
}