namespace SampleForElastic.Domain.Base
{
    public abstract class DomainEventBase<TEvent> where TEvent : EventBase
    {
        private List<TEvent> _events = new();

        public IEnumerable<TEvent> Events => _events.AsReadOnly();

        public void AddEvent(TEvent @event)
                            => _events.Add(@event);

        public void ClearEvents()
                            => _events.Clear();
    }
}
