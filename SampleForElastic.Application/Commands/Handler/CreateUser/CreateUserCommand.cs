using MediatR;
using SampleForElastic.Application.Commands.Base;
using SampleForElastic.Application.Commands.Base.Extension;

namespace SampleForElastic.Application.Commands.Handler.CreateUser
{
    public class CreateUserCommand : CommandBase, IRequest<CreateUserCommandResponse>
    {
        public string Username { get; set; }
        public DateOnly BirthDate { get; set; }
        public string About { get; set; }

        public override void Validate()
        {
            new CreateUserCommandValidator().Validate(this).ThrowIfNeeded();
        }
    }
}
