using FluentValidation;

namespace SampleForElastic.Application.Commands.Handler.AddCar
{
    public class AddCarCommandValidator : AbstractValidator<AddCarCommand>
    {
        public AddCarCommandValidator()
        {
            RuleFor(x => x.UserId)
                .NotEmpty()
                .WithMessage("UserId cannot be empty.");

            RuleFor(x => x.Name)
                .NotEmpty()
                .WithMessage("Car Name cannot be empty.");

            RuleFor(x => x.Code)
                .NotEmpty()
                .WithMessage("Car Code cannot be empty.");

            RuleFor(x => x.Color)
                .NotEmpty()
                .WithMessage("Car Color cannot be empty.");
        }
    }
}
