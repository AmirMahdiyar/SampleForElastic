using SampleForElastic.Domain.Base;
using SampleForElastic.Domain.Models.ValueObjects;

namespace SampleForElastic.Domain.Models.UserAggregate
{
    public class Car : BaseEntity<Guid>
    {
        private Car() { }
        public Car(CarIdentity identity, Guid userId)
        {
            Id = Guid.NewGuid();

            Identity = identity;
            UserId = userId;
        }

        public CarIdentity Identity { get; private set; }
        public DateTime PostedAt = DateTime.Now;


        public Guid UserId { get; private set; }
    }
}
