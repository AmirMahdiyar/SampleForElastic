using SampleForElastic.Domain.Enums;

namespace SampleForElastic.Domain.Base
{
    public class BaseEntity<TId> : DomainEventBase<EventBase>
    {
        public TId Id { get; protected set; }
        public ExistanceState State { get; protected set; } = ExistanceState.Active;

        public void Delete() => State = ExistanceState.Deleted;
    }
}
