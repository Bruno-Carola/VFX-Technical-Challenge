using FluentAssertions;
using MediatR;
using Microsoft.AspNetCore.Mvc;
using Microsoft.Extensions.Logging;
using Moq;
using VFXFinancial.WebApi.Controllers;
using VFXFinancial.WebApi.Features.ExchangeRate.Commands;
using VFXFinancial.WebApi.Features.ExchangeRate.Queries;
using VFXFinancial.WebApi.Models.DTOs;

namespace VFXFinancial.WebAPI.Tests.Controllers
{
    public class ExchangeRateControllerTests
    {
        private readonly Mock<IMediator> _mediatorMock;
        private readonly Mock<ILogger<ExchangeRateController>> _loggerMock;
        private readonly ExchangeRateController _controller;

        public ExchangeRateControllerTests()
        {
            _mediatorMock = new Mock<IMediator>();
            _loggerMock = new Mock<ILogger<ExchangeRateController>>();
            _controller = new ExchangeRateController(_mediatorMock.Object, _loggerMock.Object);
        }

        [Fact]
        public async Task Create_ShouldReturnCreatedAtAction_WhenRequestIsValid()
        {
            // Arrange
            var request = new CreateExchangeRateRequest
            {
                FromCurrency = "USD",
                ToCurrency = "EUR",
                Bid = 1.1234m,
                Ask = 1.2345m
            };

            _mediatorMock.Setup(m => m.Send(It.IsAny<CreateExchangeRateCommand>(), default))
                         .ReturnsAsync(1); // Return a new ID

            // Act
            var result = await _controller.Create(request);

            // Assert
            var createdResult = result as CreatedAtActionResult;
            createdResult.Should().NotBeNull();
            createdResult!.ActionName.Should().Be(nameof(_controller.Get));
            createdResult.RouteValues.Should().ContainKey("FromCurrency").WhoseValue.Should().Be("USD");
            createdResult.RouteValues.Should().ContainKey("ToCurrency").WhoseValue.Should().Be("EUR");
            createdResult.Value.Should().Be(1);
        }

        [Fact]
        public async Task Create_ShouldReturnInternalServerError_WhenExceptionOccurs()
        {
            // Arrange
            var request = new CreateExchangeRateRequest
            {
                FromCurrency = "USD",
                ToCurrency = "EUR",
                Bid = 1.1234m,
                Ask = 1.2345m
            };

            _mediatorMock.Setup(m => m.Send(It.IsAny<CreateExchangeRateCommand>(), default))
                         .ThrowsAsync(new Exception("Something went wrong"));

            // Act
            var result = await _controller.Create(request);

            // Assert
            var statusResult = result as ObjectResult;
            statusResult.Should().NotBeNull();
            statusResult!.StatusCode.Should().Be(500);
            statusResult.Value.Should().Be("Internal server error");
        }

        [Fact]
        public async Task Get_ShouldReturnOk_WhenExchangeRateExists()
        {
            // Arrange
            var query = new GetExchangeRateQuery
            {
                FromCurrency = "USD",
                ToCurrency = "EUR"
            };

            var expectedRate = new ExchangeRateDto
            {
                FromCurrency = "USD",
                ToCurrency = "EUR",
                Bid = 1.1234m,
                Ask = 1.2345m
            };

            _mediatorMock.Setup(m => m.Send(It.IsAny<GetExchangeRateQuery>(), default))
                         .ReturnsAsync(expectedRate);

            // Act
            var result = await _controller.Get(query.FromCurrency, query.ToCurrency);

            // Assert
            var okResult = result as OkObjectResult;
            okResult.Should().NotBeNull();
            okResult!.Value.Should().Be(expectedRate);
        }

        [Fact]
        public async Task Get_ShouldReturnNotFound_WhenExchangeRateDoesNotExist()
        {
            // Arrange
            var query = new GetExchangeRateQuery
            {
                FromCurrency = "USD",
                ToCurrency = "EUR"
            };

            _mediatorMock.Setup(m => m.Send(It.IsAny<GetExchangeRateQuery>(), default))
                         .ReturnsAsync((ExchangeRateDto)null);

            // Act
            var result = await _controller.Get(query.FromCurrency, query.ToCurrency);

            // Assert
            var notFoundResult = result as NotFoundObjectResult;
            notFoundResult.Should().NotBeNull();
            notFoundResult!.Value.Should().Be($"Exchange rate for {query.FromCurrency}/{query.ToCurrency} not found.");
        }

        [Fact]
        public async Task Update_ShouldReturnNoContent_WhenSuccessful()
        {
            // Arrange
            var request = new UpdateExchangeRateRequest
            {
                Id = 1,
                Bid = 1.3456m,
                Ask = 1.4567m
            };

            _mediatorMock.Setup(m => m.Send(It.IsAny<UpdateExchangeRateCommand>(), default))
                         .ReturnsAsync(true);

            // Act
            var result = await _controller.Update(request.Id, request);

            // Assert
            result.Should().BeOfType<NoContentResult>();
        }

        [Fact]
        public async Task Update_ShouldReturnNotFound_WhenExchangeRateDoesNotExist()
        {
            // Arrange
            var request = new UpdateExchangeRateRequest
            {
                Id = 1,
                Bid = 1.3456m,
                Ask = 1.4567m
            };

            _mediatorMock.Setup(m => m.Send(It.IsAny<UpdateExchangeRateCommand>(), default))
                         .ReturnsAsync(false);

            // Act
            var result = await _controller.Update(request.Id, request);

            // Assert
            var notFoundResult = result as NotFoundObjectResult;
            notFoundResult.Should().NotBeNull();
            notFoundResult!.Value.Should().Be($"Exchange rate with ID {request.Id} not found.");
        }

        [Fact]
        public async Task Delete_ShouldReturnNoContent_WhenSuccessful()
        {
            // Arrange
            var id = 1;

            _mediatorMock.Setup(m => m.Send(It.IsAny<DeleteExchangeRateCommand>(), default))
                         .ReturnsAsync(true);

            // Act
            var result = await _controller.Delete(id);

            // Assert
            result.Should().BeOfType<NoContentResult>();
        }

        [Fact]
        public async Task Delete_ShouldReturnNotFound_WhenExchangeRateDoesNotExist()
        {
            // Arrange
            var id = 1;

            _mediatorMock.Setup(m => m.Send(It.IsAny<DeleteExchangeRateCommand>(), default))
                         .ReturnsAsync(false);

            // Act
            var result = await _controller.Delete(id);

            // Assert
            var notFoundResult = result as NotFoundObjectResult;
            notFoundResult.Should().NotBeNull();
            notFoundResult!.Value.Should().Be($"Exchange rate with ID {id} not found.");
        }

        
    }
}
