using System.Security.Cryptography;
using System.Threading.Tasks;
using MediatR;

namespace VFXFinancial.WebApi.Features.ExchangeRate.Commands
{
    /// <summary>
    /// UpdateExchangeRateCommand
    /// </summary>
    public class UpdateExchangeRateCommand : IRequest<bool>
    {
        /// <summary>
        /// Gets or sets the identifier.
        /// </summary>
        /// <value>
        /// The identifier.
        /// </value>
        public int Id { get; set; }

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
