using SampleForElastic.Domain.Base;

namespace SampleForElastic.Domain.Events.User
{
    public class UserCarAdded : EventBase
    {
        public UserCarAdded(Guid userId, Guid carId, string name, string code, string color)
        {
            Id = Guid.NewGuid();
            UserId = userId;
            CarId = carId;
            Name = name;
            Code = code;
            Color = color;
        }

        public Guid UserId { get; set; }
        public Guid CarId { get; set; }
        public string Name { get; set; }
        public string Code { get; set; }
        public string Color { get; set; }
    }
}
