using SampleForElastic.Domain.Base;
using SampleForElastic.Domain.Events.User;

namespace SampleForElastic.Domain.Models.UserAggregate
{
    public class User : BaseEntity<Guid>
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

            AddEvent(new UserCreated(Id, BirthDate, about, DateTime.Now, State));
        }
        
        public string Username { get; private set; }
        public DateOnly BirthDate { get; private set; }
        public string About { get; private set; }
        public DateTime CreatedAt { get; private set; } = DateTime.Now;

        public IEnumerable<Car> Cars => _cars.AsReadOnly();

        public static User Create(string username, DateOnly birthDate, DateTime createdAt, string about)
                        => new User(username, birthDate, about);

        #region Methods
        public void AddCar(Car car)
        {
            if (_cars.Any(x => x.Id == car.Id))
                throw new InvalidOperationException("Car already exists under this user aggregate.");
            
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
                throw new InvalidOperationException("Car not found in this user aggregate.");
                
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
        #endregion

        #region Validations
        public void CheckAbout(string about)
        {
            if (string.IsNullOrEmpty(about))
                throw new ArgumentException("About information cannot be null or empty.");
        }
        
        public void CheckUsername(string username)
        {
            if (string.IsNullOrEmpty(username))
                throw new ArgumentException("Username cannot be null or empty.");
        }
        #endregion
    }
}
