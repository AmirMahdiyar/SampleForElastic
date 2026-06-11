using FluentValidation.Results;

namespace SampleForElastic.Application.Commands.Base.Extension
{
    public static class ValidateResultExtension
    {
        public static void ThrowIfNeeded(this ValidationResult validationResult)
        {
            var errors = validationResult.Errors;

            if (errors.Any())
                throw new Exception(string.Join(Environment.NewLine, errors.Select(e => e.ErrorMessage)));
        }

    }
}
