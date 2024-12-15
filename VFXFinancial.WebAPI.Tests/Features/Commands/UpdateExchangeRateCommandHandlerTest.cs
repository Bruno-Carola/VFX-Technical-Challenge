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
using VFXFinancial.WebApi.Features.ExchangeRate.Commands;
using VFXFinancial.WebApi.Features.ExchangeRates.Handlers;
using VFXFinancial.WebApi.Models.Domain;

namespace VFXFinancial.WebAPI.Tests.Features.Commands
{
    public class UpdateExchangeRateCommandHandlerTest
    {
        private readonly Mock<ILogger<UpdateExchangeRateCommandHandler>> _loggerMock;
        private readonly DbContextOptions<VFXFinancialDbContext> _dbContextOptions;

        public UpdateExchangeRateCommandHandlerTest()
        {
            _loggerMock = new Mock<ILogger<UpdateExchangeRateCommandHandler>>();

            _dbContextOptions = new DbContextOptionsBuilder<VFXFinancialDbContext>()
                .UseInMemoryDatabase(databaseName: "VFXFinancial_UpdateRateTestDB")
                .Options;
        }

        [Fact]
        public async Task Handle_ShouldUpdateExchangeRate_WhenRateExists()
        {
            // Arrange
            await using var context = new VFXFinancialDbContext(_dbContextOptions);

            var existingRate = new ExchangeRate
            {
                Id = 1,
                FromCurrency = "USD",
                ToCurrency = "EUR",
                Bid = 1.1234m,
                Ask = 1.2345m,
                LastUpdated = DateTime.UtcNow
            };

            context.ExchangeRates.Add(existingRate);
            await context.SaveChangesAsync();

            var command = new UpdateExchangeRateCommand
            {
                Id = 1,
                Bid = 1.3456m,
                Ask = 1.4567m
            };

            var handler = new UpdateExchangeRateCommandHandler(context, _loggerMock.Object);

            // Act
            var result = await handler.Handle(command, CancellationToken.None);

            // Assert
            result.Should().BeTrue();

            var updatedRate = await context.ExchangeRates.FindAsync(1);
            updatedRate.Should().NotBeNull();
            updatedRate!.Bid.Should().Be(1.3456m);
            updatedRate.Ask.Should().Be(1.4567m);

            _loggerMock.Verify(
                log => log.Log(
                    LogLevel.Information,
                    It.IsAny<EventId>(),
                    It.Is<It.IsAnyType>((v, t) => v.ToString().Contains("Successfully updated exchange rate")),
                    null,
                    It.IsAny<Func<It.IsAnyType, Exception, string>>()),
                Times.Once);
        }

        [Fact]
        public async Task Handle_ShouldReturnFalse_WhenRateDoesNotExist()
        {
            // Arrange
            await using var context = new VFXFinancialDbContext(_dbContextOptions);

            var command = new UpdateExchangeRateCommand
            {
                Id = 999, // Non-existent ID
                Bid = 1.3456m,
                Ask = 1.4567m
            };

            var handler = new UpdateExchangeRateCommandHandler(context, _loggerMock.Object);

            // Act
            var result = await handler.Handle(command, CancellationToken.None);

            // Assert
            result.Should().BeFalse();

            _loggerMock.Verify(
                log => log.Log(
                    LogLevel.Warning,
                    It.IsAny<EventId>(),
                    It.Is<It.IsAnyType>((v, t) => v.ToString().Contains("Exchange rate with ID")),
                    null,
                    It.IsAny<Func<It.IsAnyType, Exception, string>>()),
                Times.Once);
        }
    }
}
