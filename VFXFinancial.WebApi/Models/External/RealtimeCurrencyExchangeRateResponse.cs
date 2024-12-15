using System.Text.Json.Serialization;

namespace VFXFinancial.WebApi.Models.External
{
    /// <summary>
    /// RealtimeCurrencyExchangeRateResponse
    /// </summary>
    public class RealtimeCurrencyExchangeRateResponse
    {
        /// <summary>
        /// Gets or sets the realtime currency exchange rate.
        /// </summary>
        /// <value>
        /// The realtime currency exchange rate.
        /// </value>
        [JsonPropertyName("Realtime Currency Exchange Rate")]
        public RealtimeCurrencyExchangeRate RealtimeCurrencyExchangeRate { get; set; }
    }

    public class RealtimeCurrencyExchangeRate
    {
        /// <summary>
        /// Gets or sets from currency code.
        /// </summary>
        /// <value>
        /// From currency code.
        /// </value>
        [JsonPropertyName("1. From_Currency Code")]
        public string FromCurrencyCode { get; set; }

        /// <summary>
        /// Gets or sets the name of from currency.
        /// </summary>
        /// <value>
        /// The name of from currency.
        /// </value>
        [JsonPropertyName("2. From_Currency Name")]
        public string FromCurrencyName { get; set; }

        /// <summary>
        /// Converts to currencycode.
        /// </summary>
        /// <value>
        /// To currency code.
        /// </value>
        [JsonPropertyName("3. To_Currency Code")]
        public string ToCurrencyCode { get; set; }

        /// <summary>
        /// Converts to currencyname.
        /// </summary>
        /// <value>
        /// The name of to currency.
        /// </value>
        [JsonPropertyName("4. To_Currency Name")]
        public string ToCurrencyName { get; set; }

        /// <summary>
        /// Gets or sets the exchange rate.
        /// </summary>
        /// <value>
        /// The exchange rate.
        /// </value>
        [JsonPropertyName("5. Exchange Rate")]
        public decimal ExchangeRate { get; set; }

        /// <summary>
        /// Gets or sets the last refreshed.
        /// </summary>
        /// <value>
        /// The last refreshed.
        /// </value>
        [JsonPropertyName("6. Last Refreshed")]
        public string LastRefreshed { get; set; }

        /// <summary>
        /// Gets or sets the time zone.
        /// </summary>
        /// <value>
        /// The time zone.
        /// </value>
        [JsonPropertyName("7. Time Zone")]
        public string TimeZone { get; set; }

        /// <summary>
        /// Gets or sets the bid price.
        /// </summary>
        /// <value>
        /// The bid price.
        /// </value>
        [JsonPropertyName("8. Bid Price")]
        public decimal BidPrice { get; set; }

        /// <summary>
        /// Gets or sets the ask price.
        /// </summary>
        /// <value>
        /// The ask price.
        /// </value>
        [JsonPropertyName("9. Ask Price")]
        public decimal AskPrice { get; set; }
    }
}
