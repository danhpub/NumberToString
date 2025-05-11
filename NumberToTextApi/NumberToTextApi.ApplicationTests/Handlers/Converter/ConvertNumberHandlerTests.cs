using Microsoft.Extensions.Logging;
using NumberToTextApi.Application.Handlers.Converter;
using NumberToTextApi.Application.Queries.Converter;

namespace NumberToTextApi.ApplicationTests.Handlers.Converter
{
    public class ConvertNumberHandlerTests
    {
        private readonly ILogger<ConvertNumberHandler> _logger = Substitute.For<ILogger<ConvertNumberHandler>>();
        private readonly ConvertNumberHandler _handler;

        public ConvertNumberHandlerTests()
        {
            _handler = Substitute.For<ConvertNumberHandler>(_logger);
        }

        [Theory]
        [InlineData(123.34, "one hundred and twenty-three dollars and thirty-four cents")]
        [InlineData(3.341, "three dollars and thirty-four cents")]//rounding
        [InlineData(12345.6789, "twelve thousand three hundred and forty-five dollars and sixty-eight cents")]//rounding
        [InlineData(1000.0, "one thousand dollars")]
        [InlineData(0, "zero dollars")]
        public async Task CheckAnswe_Handle_Test_Success(decimal number, string result)
        {
            var query = new ConvertNumberQuery(number);

            var resp = await _handler.Handle(query, default);

            Assert.NotNull(resp);
            Assert.NotNull(resp.Value);
            Assert.Equal(result, resp.Value);
        }
    }
}