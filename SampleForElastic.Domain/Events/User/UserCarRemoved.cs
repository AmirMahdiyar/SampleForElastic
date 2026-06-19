using SampleForElastic.Domain.Base;
using System;

namespace SampleForElastic.Domain.Events.User
{
    public class UserCarRemoved : EventBase
    {
        public UserCarRemoved(Guid userId, Guid carId)
        {
            UserId = userId;
            CarId = carId;
        }

        public Guid UserId { get; }
        public Guid CarId { get; }
    }
}
