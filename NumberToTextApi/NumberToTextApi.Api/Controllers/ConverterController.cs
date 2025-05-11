using Ardalis.Result;
using Ardalis.Result.AspNetCore;
using MediatR;
using Microsoft.AspNetCore.Mvc;
using NumberToTextApi.Application.Queries.Converter;

namespace NumberToTextApi.Api.Controllers
{
    /// <summary>
    /// Controller for the number to string converter
    /// </summary>
    /// <param name="logger"></param>
    /// <param name="mediator"></param>
    [ApiController]
    [Route("/api/converter")]
    public class ConverterController(ILogger<ConverterController> logger, ISender mediator) : ControllerBase
    {

        /// <summary>
        /// Request to convert the number
        /// </summary>
        /// <param name="number"></param>
        /// <param name="cancellationToken"></param>
        /// <returns></returns>
        [HttpGet("{number}")]
        [TranslateResultToActionResult]
        [ProducesResponseType(typeof(Result<string>), StatusCodes.Status200OK)]
        public async Task<Result<string>> Get(decimal number, CancellationToken cancellationToken)
        {
            logger.LogDebug($"{nameof(Get)} called");

            var query =  new ConvertNumberQuery(number);
            var response = await mediator.Send(query, cancellationToken);

            logger.LogDebug($"Method {nameof(Get)} returned {response}");
            return response;
        }
    }
}
