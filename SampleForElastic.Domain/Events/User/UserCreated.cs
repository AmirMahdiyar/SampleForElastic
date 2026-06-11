using SampleForElastic.Domain.Base;
using SampleForElastic.Domain.Enums;

namespace SampleForElastic.Domain.Events.User
{
    public class UserCreated : EventBase
    {
        public UserCreated(Guid userId, DateOnly birthDate, string about, DateTime createdAt, ExistanceState state)
        {
            Id = Guid.NewGuid();
            UserId = userId;
            BirthDate = birthDate;
            About = about;
            CreatedAt = createdAt;
            State = state;
        }

        public Guid UserId { get; set; }
        public DateOnly BirthDate { get; set; }
        public string About { get; set; }
        public DateTime CreatedAt { get; set; }
        public ExistanceState State { get; set; }

    }
}
