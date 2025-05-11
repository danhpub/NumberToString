using Ardalis.Result;
using MediatR;

namespace NumberToTextApi.Application.Queries.Converter
{
    /// <summary>
    /// Query object containing request value
    /// </summary>
    public class ConvertNumberQuery : IRequest<Result<string>>
    {
        public decimal Number { get; set; }
        public ConvertNumberQuery(decimal number)
        {
            Number = number;    
        }
    }
}
