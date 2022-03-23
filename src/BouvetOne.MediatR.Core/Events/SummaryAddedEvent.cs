using MediatR;

namespace BouvetOne.MediatR.Core.Events;

internal class SummaryAddedEvent : INotification
{
    internal SummaryAddedEvent(string summary) => Summary = summary;

    internal string Summary;
}