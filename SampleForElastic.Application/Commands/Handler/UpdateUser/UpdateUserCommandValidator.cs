using FluentValidation;

namespace SampleForElastic.Application.Commands.Handler.UpdateUser
{
    public class UpdateUserCommandValidator : AbstractValidator<UpdateUserCommand>
    {
        public UpdateUserCommandValidator()
        {
            RuleFor(x => x.UserId)
                .NotEmpty()
                .WithMessage("UserId cannot be empty.");

            RuleFor(x => x.Username)
                .NotEmpty()
                .WithMessage("Username cannot be empty.");

            RuleFor(x => x.About)
                .NotEmpty()
                .WithMessage("About information cannot be empty.");
        }
    }
}
