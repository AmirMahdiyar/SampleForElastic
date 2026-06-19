using SampleForElastic.Domain.Base;
using SampleForElastic.Domain.Enums;
using System;

namespace SampleForElastic.Domain.Events.User
{
    public class UserUpdated : EventBase
    {
        public UserUpdated(Guid userId, string username, DateOnly birthDate, string about, ExistanceState state)
        {
            UserId = userId;
            Username = username;
            BirthDate = birthDate;
            About = about;
            State = state;
        }

        public Guid UserId { get; }
        public string Username { get; }
        public DateOnly BirthDate { get; }
        public string About { get; }
        public ExistanceState State { get; }
    }
}
