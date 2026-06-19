using SampleForElastic.Domain.Enums;

namespace SampleForElastic.Domain.Base
{
    public abstract class BaseEntity<TId>
    {
        public TId Id { get; protected set; }
        public ExistanceState State { get; protected set; } = ExistanceState.Active;

        public void Delete() => State = ExistanceState.Deleted;
    }
}
