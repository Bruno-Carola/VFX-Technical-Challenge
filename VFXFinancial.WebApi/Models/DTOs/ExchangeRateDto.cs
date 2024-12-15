namespace VFXFinancial.WebApi.Models.DTOs
{
    public class ExchangeRateDto
    {
        /// <summary>
        /// Gets or sets the base currency.
        /// </summary>
        /// <value>
        /// The base currency.
        /// </value>
        public string FromCurrency { get; set; }

        /// <summary>
        /// Gets or sets the target currency.
        /// </summary>
        /// <value>
        /// The target currency.
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
