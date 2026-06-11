using SampleForElastic.Domain.Base;

namespace SampleForElastic.Domain.Events.User
{
    public class UserCarRemoved : EventBase
    {
        public UserCarRemoved(Guid userId, Guid carId)
        {
            Id = Guid.NewGuid();
            UserId = userId;
            CarId = carId;
        }

        public Guid UserId { get; set; }
        public Guid CarId { get; set; }
    }
}
