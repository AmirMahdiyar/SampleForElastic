using SampleForElastic.Application.Commands.Contracts;
using SampleForElastic.Application.Contracts;
using SampleForElastic.Domain.Models.UserAggregate;
using SampleForElastic.Domain.Models.ValueObjects;
using System;
using System.Collections.Generic;
using System.Threading;
using System.Threading.Tasks;

namespace SampleForElastic.Application.Commands.Handler.AddCar
{
    public static class AddCarCommandProccesor
    {
        public static async Task<User> LoadUser(this AddCarCommand command, IUserWriteRepository repository, CancellationToken ct)
        {
            var user = await repository.GetByIdAsync(command.UserId, ct);
            if (user == null)
                throw new KeyNotFoundException($"User with ID {command.UserId} was not found.");
            
            return user;
        }

        public static Car ToCar(this AddCarCommand command)
        {
            var identity = CarIdentity.Create(command.Name, command.Code, command.Color);
            return new Car(identity, command.UserId);
        }

        public static User AddCarToAggregate(this User user, Car car)
        {
            user.AddCar(car);
            return user;
        }

        public static async Task Commit(this User user, IUnitOfWork unitOfWork, CancellationToken ct)
        {
            var result = await unitOfWork.Commit(ct);
            result.ThrowIfNoChanges<Exception>();
        }
    }
}
