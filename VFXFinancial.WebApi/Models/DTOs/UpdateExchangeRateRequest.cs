namespace VFXFinancial.WebApi.Models.DTOs
{
    /// <summary>
    /// UpdateExchangeRateRequest
    /// </summary>
    public class UpdateExchangeRateRequest
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
