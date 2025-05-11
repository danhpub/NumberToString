using Ardalis.Result;
using MediatR;
using Microsoft.Extensions.Logging;
using NumberToTextApi.Application.Queries.Converter;
using System.Text;

namespace NumberToTextApi.Application.Handlers.Converter
{
    /// <summary>
    /// A pipeline request handler converting number to its string representation
    /// </summary>
    public class ConvertNumberHandler(ILogger<ConvertNumberHandler> logger) : IRequestHandler<ConvertNumberQuery, Result<string>>
    {
        private readonly string HundredConversion = "hundred";
        private readonly string Dollar = "dollars";
        private readonly string Cents = "cents";
        private readonly string AndWithSpaces = " and ";
        private readonly string[] UnitsConversions = new[] { "zero", "one", "two", "three", "four", "five", "six", "seven", "eight", "nine", "ten", "eleven", "twelve", "thirteen", "fourteen", "fifteen", "sixteen", "seventeen", "eighteen", "nineteen" };
        private readonly string[] TensConversions = new[] { "", "ten", "twenty", "thirty", "forty", "fifty", "sixty", "seventy", "eighty", "ninety" };
        private readonly string[] ThousandsConversions = new[] { "", "thousand", "million", "billion" };

        public async Task<Result<string>> Handle(ConvertNumberQuery request, CancellationToken cancellationToken)
        {
            if (request.Number < 0)
                return Result<string>.Error("Incorrect number");

            var roundedNumber = Math.Round(request.Number, 2);

            var fullNumber = (long)roundedNumber;
            var fractionNumber = roundedNumber - fullNumber;
            var fractionFullNumber = (long)(fractionNumber * (long)Math.Pow(10, fractionNumber.Scale));

            var wholeStringifiedNumber = ConvertNumberToString(fullNumber);
            wholeStringifiedNumber.Append($" {Dollar}");
            if (fractionFullNumber > 0)
            {
                AppendNewValue(wholeStringifiedNumber, AndWithSpaces, ConvertNumberToString(fractionFullNumber)?.ToString());
                wholeStringifiedNumber.Append($" {Cents}");
            }

            return Result<string>.Success(wholeStringifiedNumber?.ToString());
        }

        private StringBuilder ConvertNumberToString(long fullNumber)
        {
            StringBuilder fullResult = new StringBuilder();
            for (int i = ThousandsConversions.Length - 1; i >= 0; i--)
            {
                long sectionDivider = (long)Math.Pow(1000, i);
                if (sectionDivider > fullNumber && i > 0)
                    continue;

                var valueNumber = fullNumber / sectionDivider;
                fullNumber = fullNumber - (valueNumber * sectionDivider);
                var convertedValue = ThreeDigitNumberToString((int)valueNumber);
                AppendNewValue(fullResult, " ", convertedValue?.ToString());
                if (convertedValue?.Length > 0 && !string.IsNullOrEmpty(ThousandsConversions[i]))
                {
                    AppendNewValue(fullResult, " ", ThousandsConversions[i]);
                }
                if (fullNumber == 0)
                    break;
            }
            return fullResult;
        }

        private StringBuilder ThreeDigitNumberToString(int fullNumber)
        {
            if (fullNumber < 0)
                return new StringBuilder(UnitsConversions[0]);

            int hundreds = fullNumber / 100;

            int decimals = fullNumber - (hundreds * 100);
            int tenths = decimals / 10;

            int units = (decimals - (tenths * 10));

            StringBuilder stringifiedNumber = new();
            if (hundreds > 0)
            {
                stringifiedNumber.Append($"{UnitsConversions[hundreds]} {HundredConversion}");
            }

            if (tenths > 1)
            {
                if (tenths > 0)
                {
                    AppendNewValue(stringifiedNumber, AndWithSpaces, TensConversions[tenths]);
                    if (units > 0)
                    {
                        AppendNewValue(stringifiedNumber, "-", UnitsConversions[units]);
                    }
                }
            }
            else
            {
                AppendNewValue(stringifiedNumber, AndWithSpaces, UnitsConversions[decimals]);
            }
            return stringifiedNumber;
        }

        private StringBuilder AppendNewValue(StringBuilder currentValue, string separator, string? newValue)
        {
            if (!string.IsNullOrEmpty(newValue))
            {
                if (currentValue.Length > 0)
                {
                    currentValue.Append(separator);
                }
                currentValue.Append(newValue);
            }
            return currentValue;
        }
    }
}
