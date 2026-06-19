using SampleForElastic.Application.Commands.Contracts;
using SampleForElastic.Application.Contracts;
using SampleForElastic.Domain.Models.UserAggregate;
using System;
using System.Collections.Generic;
using System.Threading;
using System.Threading.Tasks;

namespace SampleForElastic.Application.Commands.Handler.UpdateUser
{
    public static class UpdateUserCommandProccesor
    {
        public static async Task<User> LoadUser(this UpdateUserCommand command, IUserWriteRepository repository, CancellationToken ct)
        {
            var user = await repository.GetByIdAsync(command.UserId, ct);
            if (user == null)
                throw new KeyNotFoundException($"User with ID {command.UserId} was not found.");
            
            return user;
        }

        public static User UpdateDomain(this User user, UpdateUserCommand command)
        {
            user.UpdateUsername(command.Username);
            user.UpdateAbout(command.About);
            return user;
        }

        public static async Task<bool> Commit(this User user, IUnitOfWork unitOfWork, CancellationToken ct)
        {
            var result = await unitOfWork.Commit(ct);
            return result.IsSucceeded;
        }
    }
}
