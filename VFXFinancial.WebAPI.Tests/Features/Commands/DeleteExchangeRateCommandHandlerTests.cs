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
    public class DeleteExchangeRateCommandHandlerTests
    {
        [Fact]
        public async Task Handle_ShouldDeleteExchangeRate_WhenIdExists()
        {
            // Arrange
            var options = new DbContextOptionsBuilder<VFXFinancialDbContext>()
                .UseInMemoryDatabase(databaseName: "TestDatabase")
                .Options;

            var mockLogger = new Mock<ILogger<DeleteExchangeRateCommandHandler>>();

            await using var context = new VFXFinancialDbContext(options);

            var rate = new ExchangeRate
            {
                FromCurrency = "USD",
                ToCurrency = "EUR",
                Bid = 1.1234m,
                Ask = 1.2345m
            };
            context.ExchangeRates.Add(rate);
            await context.SaveChangesAsync();

            var handler = new DeleteExchangeRateCommandHandler(context, mockLogger.Object);
            var command = new DeleteExchangeRateCommand { Id = rate.Id };

            // Act
            await handler.Handle(command, CancellationToken.None);

            // Assert
            var deletedRate = await context.ExchangeRates.FindAsync(rate.Id);
            deletedRate.Should().BeNull();
        }

        [Fact]
        public async Task Handle_ShouldThrowException_WhenIdDoesNotExist()
        {
            // Arrange
            var options = new DbContextOptionsBuilder<VFXFinancialDbContext>()
                .UseInMemoryDatabase(databaseName: "TestDatabase")
                .Options;

            var mockLogger = new Mock<ILogger<DeleteExchangeRateCommandHandler>>();

            await using var context = new VFXFinancialDbContext(options);

            var handler = new DeleteExchangeRateCommandHandler(context, mockLogger.Object);
            var command = new DeleteExchangeRateCommand { Id = 999 };

            // Act & Assert
            var result = await handler.Handle(command, CancellationToken.None);
            result.Should().BeFalse();
        }
    }
}
