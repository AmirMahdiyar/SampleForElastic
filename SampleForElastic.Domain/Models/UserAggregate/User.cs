using SampleForElastic.Domain.Base;
using SampleForElastic.Domain.Events.User;
using System;
using System.Collections.Generic;
using System.Linq;

namespace SampleForElastic.Domain.Models.UserAggregate
{
    public class User : AggregateRoot<Guid>
    {
        private readonly List<Car> _cars = new();

        private User() { }

        public User(string username, DateOnly birthDate, string about)
        {
            CheckAbout(about);
            CheckUsername(username);

            Id = Guid.NewGuid();
            Username = username;
            BirthDate = birthDate;
            About = about;

            AddEvent(new UserCreated(Id, Username, BirthDate, About, CreatedAt, State));
        }

        public string Username { get; private set; }
        public DateOnly BirthDate { get; private set; }
        public string About { get; private set; }
        public DateTime CreatedAt { get; private set; } = DateTime.UtcNow;

        public IEnumerable<Car> Cars => _cars.AsReadOnly();

        public static User Create(string username, DateOnly birthDate, DateTime createdAt, string about)
                        => new User(username, birthDate, about);

        public void AddCar(Car car)
        {
            if (_cars.Any(x => x.Id == car.Id))
                throw new InvalidOperationException();

            _cars.Add(car);
            AddEvent(new UserCarAdded(Id, car.Id, car.Identity.Name, car.Identity.Code, car.Identity.Color));
        }

        public void BulkAddCar(IEnumerable<Car> cars)
        {
            foreach (var car in cars)
            {
                AddCar(car);
            }
        }

        public void RemoveCar(Car car)
        {
            var carInList = _cars.FirstOrDefault(x => x.Id == car.Id);
            if (carInList == null)
                throw new InvalidOperationException();

            carInList.Delete();
            AddEvent(new UserCarRemoved(Id, car.Id));
        }

        public void UpdateAbout(string about)
        {
            CheckAbout(about);
            About = about;
            AddEvent(new UserUpdated(Id, Username, BirthDate, About, State));
        }

        public void UpdateUsername(string username)
        {
            CheckUsername(username);
            Username = username;
            AddEvent(new UserUpdated(Id, Username, BirthDate, About, State));
        }

        public void CheckAbout(string about)
        {
            if (string.IsNullOrEmpty(about))
                throw new ArgumentException();
        }

        public void CheckUsername(string username)
        {
            if (string.IsNullOrEmpty(username))
                throw new ArgumentException();
        }
    }
}
