using System.Collections.Generic;

namespace SampleForElastic.Domain.Base
{
    public interface IAggregateRoot
    {
        IReadOnlyCollection<EventBase> Events { get; }
        void ClearEvents();
    }

    public abstract class AggregateRoot<TId> : BaseEntity<TId>, IAggregateRoot
    {
        private readonly List<EventBase> _events = new();

        public IReadOnlyCollection<EventBase> Events => _events.AsReadOnly();

        protected void AddEvent(EventBase @event)
        {
            _events.Add(@event);
        }

        public void ClearEvents()
        {
            _events.Clear();
        }
    }
}
