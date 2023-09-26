using MediatR;

namespace SharedKernel.Domain;

public interface IEventHandler<TDomainEvent> : INotificationHandler<TDomainEvent> where TDomainEvent : DomainEvent
{
}