using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using FluentAssertions;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.Logging;
using Moq;
using VFXFinancial.WebApi.Data;
using VFXFinancial.WebApi.Features.ExchangeRate.Queries;
using VFXFinancial.WebApi.Features.ExchangeRates.Handlers;
using VFXFinancial.WebApi.Infrastructure.External;
using VFXFinancial.WebApi.Infrastructure.ThirdParty;
using VFXFinancial.WebApi.Models.Domain;

namespace VFXFinancial.WebAPI.Tests.Features.Queries
{
    public class GetExchangeRateQueryHandlerTests
    {
        private readonly Mock<ILogger<GetExchangeRateQueryHandler>> _loggerMock;
        private readonly Mock<IAlphaVantageClient> _alphaVantageClientMock;
        private readonly DbContextOptions<VFXFinancialDbContext> _dbContextOptions;

        public GetExchangeRateQueryHandlerTests()
        {
            _loggerMock = new Mock<ILogger<GetExchangeRateQueryHandler>>();
            _alphaVantageClientMock = new Mock<IAlphaVantageClient>();

            _dbContextOptions = new DbContextOptionsBuilder<VFXFinancialDbContext>()
                .UseInMemoryDatabase(databaseName: "VFXFinancial_TestDB")
                .Options;
        }

        [Fact]
        public async Task Handle_ShouldReturnExchangeRateFromDatabase_WhenRateExists()
        {
            // Arrange
            await using var context = new VFXFinancialDbContext(_dbContextOptions);
            var existingRate = new ExchangeRate
            {
                FromCurrency = "USD",
                ToCurrency = "EUR",
                Bid = 1.1234m,
                Ask = 1.2345m,
                LastUpdated = System.DateTime.UtcNow
            };
            context.ExchangeRates.Add(existingRate);
            await context.SaveChangesAsync();

            var query = new GetExchangeRateQuery { FromCurrency = "USD", ToCurrency = "EUR" };
            var handler = new GetExchangeRateQueryHandler(context, _loggerMock.Object, _alphaVantageClientMock.Object);

            // Act
            var result = await handler.Handle(query, CancellationToken.None);

            // Assert
            result.Should().NotBeNull();
            result.FromCurrency.Should().Be("USD");
            result.ToCurrency.Should().Be("EUR");
            result.Bid.Should().Be(1.1234m);
            result.Ask.Should().Be(1.2345m);
        }

        [Fact]
        public async Task Handle_ShouldFetchAndSaveExchangeRate_WhenRateNotInDatabase()
        {
            // Arrange
            await using var context = new VFXFinancialDbContext(_dbContextOptions);

            var fetchedRate = new ExchangeRate
            {
                FromCurrency = "GBP",
                ToCurrency = "USD",
                Bid = 1.5432m,
                Ask = 1.6543m,
                LastUpdated = System.DateTime.UtcNow
            };

            _alphaVantageClientMock
                .Setup(client => client.FetchExchangeRateAsync("GBP", "USD"))
                .ReturnsAsync(fetchedRate);

            var query = new GetExchangeRateQuery { FromCurrency = "GBP", ToCurrency = "USD" };
            var handler = new GetExchangeRateQueryHandler(context, _loggerMock.Object, _alphaVantageClientMock.Object);

            // Act
            var result = await handler.Handle(query, CancellationToken.None);

            // Assert
            result.Should().NotBeNull();
            result.FromCurrency.Should().Be("GBP");
            result.ToCurrency.Should().Be("USD");
            result.Bid.Should().Be(1.5432m);
            result.Ask.Should().Be(1.6543m);

            // Check database
            var savedRate = await context.ExchangeRates.FirstOrDefaultAsync(r => r.FromCurrency == "GBP" && r.ToCurrency == "USD");
            savedRate.Should().NotBeNull();
            savedRate!.Bid.Should().Be(1.5432m);
            savedRate.Ask.Should().Be(1.6543m);
        }

        [Fact]
        public async Task Handle_ShouldReturnNull_WhenRateNotInDatabaseAndFetchFails()
        {
            // Arrange
            await using var context = new VFXFinancialDbContext(_dbContextOptions);

            _alphaVantageClientMock
                .Setup(client => client.FetchExchangeRateAsync("JPY", "CHF"))
                .ReturnsAsync((ExchangeRate)null);

            var query = new GetExchangeRateQuery { FromCurrency = "JPY", ToCurrency = "CHF" };
            var handler = new GetExchangeRateQueryHandler(context, _loggerMock.Object, _alphaVantageClientMock.Object);

            // Act
            var result = await handler.Handle(query, CancellationToken.None);

            // Assert
            result.Should().BeNull();
            _alphaVantageClientMock.Verify(client => client.FetchExchangeRateAsync("JPY", "CHF"), Times.Once);
        }

        [Fact]
        public async Task Handle_ShouldLogError_WhenExceptionOccurs()
        {
            // Arrange
            await using var context = new VFXFinancialDbContext(_dbContextOptions);

            var query = new GetExchangeRateQuery { FromCurrency = "USD", ToCurrency = "CAD" };

            _alphaVantageClientMock
                .Setup(client => client.FetchExchangeRateAsync("USD", "CAD"))
                .ThrowsAsync(new System.Exception("Unexpected error"));

            var handler = new GetExchangeRateQueryHandler(context, _loggerMock.Object, _alphaVantageClientMock.Object);

            // Act & Assert
            await FluentActions.Invoking(() => handler.Handle(query, CancellationToken.None))
                .Should().ThrowAsync<System.Exception>()
                .WithMessage("Unexpected error");

            _loggerMock.Verify(
                log => log.Log(
                    LogLevel.Error,
                    It.IsAny<EventId>(),
                    It.Is<It.IsAnyType>((v, t) => v.ToString().Contains("Error occurred while retrieving exchange rate")),
                    It.IsAny<System.Exception>(),
                    It.IsAny<Func<It.IsAnyType, Exception, string>>()),
                Times.Once);
        }
    }
}
