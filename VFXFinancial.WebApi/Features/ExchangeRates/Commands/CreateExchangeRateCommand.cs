using MediatR;

namespace VFXFinancial.WebApi.Features.ExchangeRate.Commands
{

    /// <summary>
    /// CreateExchangeRateCommand
    /// </summary>
    public class CreateExchangeRateCommand : IRequest<int>
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

        /// <summary>
        /// Gets or sets the bid.
        /// </summary>
        /// <value>
        /// The bid.
        /// </value>
        public decimal Bid { get; set; }

        /// <summary>
        /// Gets or sets the ask.
        /// </summary>
        /// <value>
        /// The ask.
        /// </value>
        public decimal Ask { get; set; }
    }
}
