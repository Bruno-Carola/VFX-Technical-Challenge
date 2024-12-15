using MediatR;
using VFXFinancial.WebApi.Data;
using VFXFinancial.WebApi.Features.ExchangeRate.Commands;

namespace VFXFinancial.WebApi.Features.ExchangeRates.Handlers
{
    /// <summary>
    /// DeleteExchangeRateCommandHandler
    /// </summary>
    public class DeleteExchangeRateCommandHandler : IRequestHandler<DeleteExchangeRateCommand, bool>
    {
        private readonly VFXFinancialDbContext _context;
        private readonly ILogger<DeleteExchangeRateCommandHandler> _logger;

        /// <summary>
        /// Initializes a new instance of the <see cref="DeleteExchangeRateCommandHandler"/> class.
        /// </summary>
        /// <param name="context">The context.</param>
        /// <param name="logger">The logger.</param>
        public DeleteExchangeRateCommandHandler(VFXFinancialDbContext context, ILogger<DeleteExchangeRateCommandHandler> logger)
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
        public async Task<bool> Handle(DeleteExchangeRateCommand Request, CancellationToken CancellationToken)
        {
            _logger.LogInformation("Handling DeleteExchangeRateCommand for ID {Id}", Request.Id);

            try
            {
                var rate = await _context.ExchangeRates.FindAsync(new object[] { Request.Id }, CancellationToken);
                if (rate == null) {
                    _logger.LogWarning("Exchange rate with ID {Id} not found", Request.Id);
                    return false;
                }

                _context.ExchangeRates.Remove(rate);
                await _context.SaveChangesAsync(CancellationToken);

                _logger.LogInformation("Successfully deleted exchange rate with ID {Id}", Request.Id);

                return true;
            }
            catch (Exception ex)
            {
                _logger.LogError(ex, "Error occurred while deleting exchange rate with ID {Id}", Request.Id);

                throw;
            }
        }
    }
}
