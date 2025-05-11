using Microsoft.Extensions.Logging;
using NumberToTextApi.Api.Controllers;
using NumberToTextApi.Application.Queries.Converter;


namespace NumberToTextApi.ApiTests.Controllers
{
    public class ConverterControllerTests
    {
        private readonly ConverterController _controller;
        private readonly ISender _mediator;
        private readonly ILogger<ConverterController> _logger;
        private readonly string _responseValue = "One hundred twenty three";
        public ConverterControllerTests()
        {
            _logger = Substitute.For<ILogger<ConverterController>>();
            _mediator = Substitute.For<ISender>();
            _controller = new ConverterController(_logger, _mediator);

            _mediator.Send(Arg.Any<ConvertNumberQuery>(), CancellationToken.None).Returns(Task.FromResult(Result.Success(_responseValue)));
        }

        [Fact]
        public async Task ConverterNumberTest_Success()
        {
            var response = await _controller.Get(123, CancellationToken.None);
            Assert.NotNull(response);
            Assert.IsType<Result<string>>(response);
            Assert.Equal(response, _responseValue);
        }
    }
}