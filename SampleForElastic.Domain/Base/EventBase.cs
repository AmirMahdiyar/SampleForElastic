using MediatR;

namespace SampleForElastic.Domain.Base
{
    public abstract class EventBase : INotification
    {
        public Guid Id { get; protected set; }
        public DateTime SendedAt { get; protected set; } = DateTime.Now;
    }
}
