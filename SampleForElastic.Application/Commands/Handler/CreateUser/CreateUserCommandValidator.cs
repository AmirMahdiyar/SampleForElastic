using FluentValidation;
using FluentValidation.Validators;

namespace SampleForElastic.Application.Commands.Handler.CreateUser
{
    public class CreateUserCommandValidator : AbstractValidator<CreateUserCommand>
    {
        public CreateUserCommandValidator()
        {
            RuleFor(x => x.BirthDate)
                .NotEmpty()
                .WithMessage("BirthDate Can't be Empty");

            RuleFor(x => x.About)
                .SpecialAboutValidation()
                .WithMessage("Just Test");
        }
    }
    public static class ExtensionForFluentValidation
    {
        /// <summary>
        /// Just For Practice
        /// </summary>
        public static IRuleBuilderOptions<T, TPropery> SpecialAboutValidation<T, TPropery>(this IRuleBuilder<T, TPropery> rule)
            => rule.SetValidator(new ExtensionValidation<T, TPropery>());
    }
    public class ExtensionValidation<T, TProperty> : PropertyValidator<T, TProperty>
    {
        public override string Name => "ExtensionValidatorForAbout";

        public override bool IsValid(ValidationContext<T> context, TProperty value)
        {
            if (value is null || string.IsNullOrEmpty(value as string))
            {
                return false;
            }
            else
            {
                return true;
            }
        }
    }
}
