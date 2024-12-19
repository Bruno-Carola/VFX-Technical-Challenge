using MediatR;
using Microsoft.EntityFrameworkCore;
using VFXFinancial.WebApi.Data;
using VFXFinancial.WebApi.Features.ExchangeRate.Queries;
using VFXFinancial.WebApi.Infrastructure.External;
using VFXFinancial.WebApi.Infrastructure.ThirdParty;
using VFXFinancial.WebApi.Models.DTOs;

namespace VFXFinancial.WebApi.Features.ExchangeRates.Handlers
{
    /// <summary>
    /// GetExchangeRateQueryHandler
    /// </summary>
    public class GetExchangeRateQueryHandler : IRequestHandler<GetExchangeRateQuery, ExchangeRateDto>
    {
        private readonly VFXFinancialDbContext _context;
        private readonly ILogger<GetExchangeRateQueryHandler> _logger;
        private readonly IAlphaVantageClient _client;

        /// <summary>
        /// Initializes a new instance of the <see cref="GetExchangeRateQueryHandler"/> class.
        /// </summary>
        /// <param name="context">The context.</param>
        /// <param name="logger">The logger.</param>
        /// <param name="client">The client.</param>
        public GetExchangeRateQueryHandler(VFXFinancialDbContext context, ILogger<GetExchangeRateQueryHandler> logger, IAlphaVantageClient client)
        {
            _context = context;
            _logger = logger;
            _client = client;
        }

        /// <summary>
        /// Handles the specified request.
        /// </summary>
        /// <param name="Request">The request.</param>
        /// <param name="CancellationToken">The cancellation token.</param>
        /// <returns></returns>
        public async Task<ExchangeRateDto> Handle(GetExchangeRateQuery Request, CancellationToken CancellationToken)
        {
            _logger.LogInformation("Handling GetExchangeRateQuery for {FromCurrency}/{ToCurrency}", Request.FromCurrency, Request.ToCurrency);

            try
            {
                var rate = await _context.ExchangeRates.FirstOrDefaultAsync(r => r.FromCurrency == Request.FromCurrency && r.ToCurrency == Request.ToCurrency, CancellationToken);

                if (rate == null)
                {
                    rate = await _client.FetchExchangeRateAsync(Request.FromCurrency, Request.ToCurrency);
                    if (rate != null)
                    {
                        _context.ExchangeRates.Add(rate);
                        await _context.SaveChangesAsync(CancellationToken);
                    }
                    else
                    {
                        _logger.LogWarning("Exchange rate for {FromCurrency}/{ToCurrency} not found", Request.FromCurrency, Request.ToCurrency);
                        return null;
                    }
                }

                _logger.LogInformation("Successfully retrieved exchange rate for {FromCurrency}/{ToCurrency}", Request.FromCurrency, Request.ToCurrency);

                return new ExchangeRateDto { Ask = rate.Ask, Bid = rate.Bid, FromCurrency = rate.FromCurrency, ToCurrency = rate.ToCurrency };
            }
            catch (Exception ex)
            {
                _logger.LogError(ex, "Error occurred while retrieving exchange rate for {FromCurrency}/{ToCurrency}", Request.FromCurrency, Request.ToCurrency);
                throw;
            }
        }
    }
}
