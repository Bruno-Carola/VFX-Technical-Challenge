using MediatR;
using VFXFinancial.WebApi.Data;
using VFXFinancial.WebApi.Features.ExchangeRate.Commands;
using VFXFinancial.WebApi.Models.Domain;

namespace VFXFinancial.WebApi.Features.ExchangeRates.Handlers
{
    /// <summary>
    /// UpdateExchangeRateCommandHandler
    /// </summary>
    public class UpdateExchangeRateCommandHandler : IRequestHandler<UpdateExchangeRateCommand, bool>
    {
        private readonly VFXFinancialDbContext _context;
        private readonly ILogger<UpdateExchangeRateCommandHandler> _logger;

        /// <summary>
        /// Initializes a new instance of the <see cref="UpdateExchangeRateCommandHandler"/> class.
        /// </summary>
        /// <param name="context">The context.</param>
        /// <param name="logger">The logger.</param>
        public UpdateExchangeRateCommandHandler(VFXFinancialDbContext context, ILogger<UpdateExchangeRateCommandHandler> logger)
        {
            _context = context;
            _logger = logger;
        }

        /// <summary>
        /// Handles the specified request.
        /// </summary>
        /// <param name="Request">The request.</param>
        /// <param name="CancellationToken">The cancellation token.</param>
        /// <returns></returns>
        public async Task<bool> Handle(UpdateExchangeRateCommand Request, CancellationToken CancellationToken)
        {
            _logger.LogInformation("Handling UpdateExchangeRateCommand for Exchange rate with ID {Id}", Request.Id);

            try
            {
                var rate = await _context.ExchangeRates.FindAsync(new object[] { Request.Id }, CancellationToken);
                if (rate == null)
                {
                    _logger.LogWarning("Exchange rate with ID {Id} not found", Request.Id);
                    return false;
                }


                rate.Bid = Request.Bid;
                rate.Ask = Request.Ask;
                rate.LastUpdated = DateTime.UtcNow;

                await _context.SaveChangesAsync(CancellationToken);

                _logger.LogInformation("Successfully updated exchange rate with ID {Id}", Request.Id);

                return true;
            }
            catch (Exception ex)
            {
                _logger.LogError(ex, "Error occurred while updating exchange rate with ID {Id}", Request.Id);
                throw;
            }
            
        }
    }
}
