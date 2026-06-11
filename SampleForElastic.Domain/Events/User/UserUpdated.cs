using SampleForElastic.Domain.Base;
using SampleForElastic.Domain.Enums;

namespace SampleForElastic.Domain.Events.User
{
    public class UserUpdated : EventBase
    {
        public UserUpdated(Guid userId, string username, DateOnly birthDate, string about, ExistanceState state)
        {
            Id = Guid.NewGuid();
            UserId = userId;
            Username = username;
            BirthDate = birthDate;
            About = about;
            State = state;
        }

        public Guid UserId { get; set; }
        public string Username { get; set; }
        public DateOnly BirthDate { get; set; }
        public string About { get; set; }
        public ExistanceState State { get; set; }
    }
}
