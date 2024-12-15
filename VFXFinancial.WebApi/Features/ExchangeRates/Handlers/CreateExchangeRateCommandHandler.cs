using System.Text.Json;
using MediatR;
using VFXFinancial.WebApi.Data;
using VFXFinancial.WebApi.Features.ExchangeRate.Commands;
using VFXFinancial.WebApi.Infrastructure.Messaging;

namespace VFXFinancial.WebApi.Features.ExchangeRates.Handlers
{
    /// <summary>
    /// CreateExchangeRateCommandHandler
    /// </summary>
    public class CreateExchangeRateCommandHandler : IRequestHandler<CreateExchangeRateCommand, int>
    {
        private readonly VFXFinancialDbContext _context;
        private readonly ILogger<CreateExchangeRateCommandHandler> _logger;
        private readonly RabbitMQPublisher _publisher;

        /// <summary>
        /// Initializes a new instance of the <see cref="CreateExchangeRateCommandHandler"/> class.
        /// </summary>
        /// <param name="context">The context.</param>
        /// <param name="logger">The logger.</param>
        /// <param name="publisher">The publisher.</param>
        public CreateExchangeRateCommandHandler(VFXFinancialDbContext context, ILogger<CreateExchangeRateCommandHandler> logger, RabbitMQPublisher publisher)
        {
            _context = context;
            _logger = logger;
            _publisher = publisher;
        }

        /// <summary>
        /// Handles the specified request.
        /// </summary>
        /// <param name="Request">The request.</param>
        /// <param name="CancellationToken">The cancellation token.</param>
        /// <returns></returns>
        public async Task<int> Handle(CreateExchangeRateCommand Request, CancellationToken CancellationToken)
        {
            _logger.LogInformation("Handling CreateExchangeRateCommand for {FromCurrency}/{ToCurrency}", Request.FromCurrency, Request.ToCurrency);

            try
            {
                var rate = new Models.Domain.ExchangeRate
                {
                    FromCurrency = Request.FromCurrency,
                    ToCurrency = Request.ToCurrency,
                    Bid = Request.Bid,
                    Ask = Request.Ask,
                    LastUpdated = DateTime.UtcNow
                };

                _context.ExchangeRates.Add(rate);
                await _context.SaveChangesAsync(CancellationToken);

                var message = new
                {
                    Event = "ExchangeRateAdded",
                    FromCurrency = rate.FromCurrency,
                    ToCurrency = rate.ToCurrency,
                    Bid = rate.Bid,
                    Ask = rate.Ask,
                    LastUpdated = rate.LastUpdated,
                    ExchangeRateId = rate.Id
                };

                await _publisher.PublishAsync("ExchangeRateAdded", JsonSerializer.Serialize(message));

                _logger.LogInformation("Successfully created exchange rate with ID {Id}", rate.Id);

                return rate.Id;
            }
            catch (Exception ex)
            {
                _logger.LogError(ex, "Error occurred while creating exchange rate for {FromCurrency}/{ToCurrency}", Request.FromCurrency, Request.ToCurrency);
                throw;
            }
            
        }
    }
}
