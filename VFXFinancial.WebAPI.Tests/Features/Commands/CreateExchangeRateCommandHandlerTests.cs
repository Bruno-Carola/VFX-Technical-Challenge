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
using VFXFinancial.WebApi.Infrastructure.Messaging;

namespace VFXFinancial.WebAPI.Tests.Features.Commands
{
    public class CreateExchangeRateCommandHandlerTests
    {
        [Fact]
        public async Task Handle_ShouldAddExchangeRateToDatabase_WhenValidRequest()
        {
            // Arrange
            var options = new DbContextOptionsBuilder<VFXFinancialDbContext>()
                .UseInMemoryDatabase(databaseName: "TestDatabase")
                .Options;

            await using var context = new VFXFinancialDbContext(options);
            var mockPublisher = new Mock<RabbitMQPublisher>();
            var mockLogger = new Mock<ILogger<CreateExchangeRateCommandHandler>>();

            var handler = new CreateExchangeRateCommandHandler(context, mockLogger.Object, mockPublisher.Object);

            var command = new CreateExchangeRateCommand
            {
                FromCurrency = "USD",
                ToCurrency = "EUR",
                Bid = 1.1234m,
                Ask = 1.2345m
            };

            // Act
            var result = await handler.Handle(command, CancellationToken.None);

            // Assert
            result.Should().BeGreaterThan(0); // Ensure a valid ID is returned

            var savedRate = await context.ExchangeRates.FirstOrDefaultAsync();
            savedRate.Should().NotBeNull();
            savedRate!.FromCurrency.Should().Be("USD");
            savedRate.ToCurrency.Should().Be("EUR");
            savedRate.Bid.Should().Be(1.1234m);
            savedRate.Ask.Should().Be(1.2345m);

            mockPublisher.Verify(p => p.PublishAsync("ExchangeRateAdded", It.IsAny<string>()), Times.Once); // Ensure that messaging is working
        }
    }
}
