using MediatR;
using VFXFinancial.WebApi.Models.DTOs;

namespace VFXFinancial.WebApi.Features.ExchangeRate.Queries
{
    /// <summary>
    /// GetExchangeRateQuery
    /// </summary>
    public class GetExchangeRateQuery : IRequest<ExchangeRateDto>
    {
        /// <summary>
        /// Gets or sets from currency.
        /// </summary>
        /// <value>
        /// From currency.
        /// </value>
        public string FromCurrency { get; set; }

        /// <summary>
        /// Converts to currency.
        /// </summary>
        /// <value>
        /// To currency.
        /// </value>
        public string ToCurrency { get; set; }
    }

}
