using NumberToTextApi.Application.Queries.Converter;
using FluentValidation;

namespace NumberToTextApi.Application.Validators.Converter
{
    /// <summary>
    /// A validator, part of the request processing pipeline
    /// </summary>
    public class ConvertNumberQueryValidator : AbstractValidator<ConvertNumberQuery>
    {
        public ConvertNumberQueryValidator()
        {
            //Validation making sure the value is not negative
            RuleFor(x => x.Number).NotNull().GreaterThanOrEqualTo(0);
        }
    }
}
