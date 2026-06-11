using SampleForElastic.Application.Commands.Contracts;
using SampleForElastic.Application.Contracts;
using SampleForElastic.Domain.Models.UserAggregate;

namespace SampleForElastic.Application.Commands.Handler.CreateUser
{
    public static class CreateUserCommandProccesor
    {
        public static async Task<CreateUserCommand> WithCheckingUserExistance(this CreateUserCommand command, IUserWriteRepository repository, CancellationToken ct)
        {
            var result = await repository.IsExistAsync(command.Username, ct);
            if (result)
                throw new Exception();
            return command;
        }

        public static User ToUser(this CreateUserCommand command)
            => new User(command.Username, command.BirthDate, command.About);

        public static async Task Commit(this User user, IUnitOfWork unitOfWork, CancellationToken ct)
        {
            var result = await unitOfWork.Commit(ct);
            result.ThrowIfNoChanges<Exception>();
        }
    }
}
