using System.Security;

namespace VFXFinancial.WebApi.Models.Domain
{
    /// <summary>
    /// ExchangeRate
    /// </summary>
    public class ExchangeRate
    {
        /// <summary>
        /// Gets or sets the identifier.
        /// </summary>
        /// <value>
        /// The identifier.
        /// </value>
        public int Id { get; set; }

        /// <summary>
        /// Gets or sets the base currency.
        /// </summary>
        /// <value>
        /// The base currency.
        /// </value>
        public string FromCurrency { get; set; } = string.Empty;

        /// <summary>
        /// Gets or sets the target currency.
        /// </summary>
        /// <value>
        /// The target currency.
        /// </value>
        public string ToCurrency { get; set; } = string.Empty;

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

        /// <summary>
        /// Gets or sets the last updated.
        /// </summary>
        /// <value>
        /// The last updated.
        /// </value>
        public DateTime LastUpdated { get; set; }
    }
}
