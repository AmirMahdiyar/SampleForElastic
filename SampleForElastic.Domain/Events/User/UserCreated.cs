using SampleForElastic.Domain.Base;
using SampleForElastic.Domain.Enums;
using System;

namespace SampleForElastic.Domain.Events.User
{
    public class UserCreated : EventBase
    {
        public UserCreated(Guid userId, string username, DateOnly birthDate, string about, DateTime createdAt, ExistanceState state)
        {
            UserId = userId;
            Username = username;
            BirthDate = birthDate;
            About = about;
            CreatedAt = createdAt;
            State = state;
        }

        public Guid UserId { get; }
        public string Username { get; }
        public DateOnly BirthDate { get; }
        public string About { get; }
        public DateTime CreatedAt { get; }
        public ExistanceState State { get; }
    }
}
