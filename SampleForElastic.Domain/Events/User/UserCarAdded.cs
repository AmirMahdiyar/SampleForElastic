using SampleForElastic.Domain.Base;
using System;

namespace SampleForElastic.Domain.Events.User
{
    public class UserCarAdded : EventBase
    {
        public UserCarAdded(Guid userId, Guid carId, string name, string code, string color)
        {
            UserId = userId;
            CarId = carId;
            Name = name;
            Code = code;
            Color = color;
        }

        public Guid UserId { get; }
        public Guid CarId { get; }
        public string Name { get; }
        public string Code { get; }
        public string Color { get; }
    }
}
