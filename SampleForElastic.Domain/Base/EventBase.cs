using MediatR;
using System;

namespace SampleForElastic.Domain.Base
{
    public abstract class EventBase : INotification
    {
        public Guid Id { get; protected set; } = Guid.NewGuid();
        public DateTime SendedAt { get; protected set; } = DateTime.UtcNow;
    }
}
