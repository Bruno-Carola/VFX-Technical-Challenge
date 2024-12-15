using System.Text.Json;
using VFXFinancial.WebApi.Models.Domain;
using VFXFinancial.WebApi.Models.External;

namespace VFXFinancial.WebApi.Infrastructure.ThirdParty
{
    /// <summary>
    /// AlphaVantageClient
    /// </summary>
    public class AlphaVantageClient
    {
        private readonly HttpClient _httpClient;
        private readonly ILogger<AlphaVantageClient> _logger;
        private readonly string _apiKey = "H6P0ISF5SCN9VHQZ";

        /// <summary>
        /// Initializes a new instance of the <see cref="AlphaVantageClient"/> class.
        /// </summary>
        /// <param name="httpClient">The HTTP client.</param>
        /// <param name="logger">The logger.</param>
        public AlphaVantageClient(HttpClient httpClient, ILogger<AlphaVantageClient> logger)
        {
            _httpClient = httpClient;
            _logger = logger;
        }

        /// <summary>
        /// Fetches the exchange rate asynchronous.
        /// </summary>
        /// <param name="FromCurrency">From currency.</param>
        /// <param name="ToCurrency">To currency.</param>
        /// <returns></returns>
        public async Task<ExchangeRate?> FetchExchangeRateAsync(string FromCurrency, string ToCurrency)
        {
            try
            {
                var url = $"https://www.alphavantage.co/query?function=CURRENCY_EXCHANGE_RATE&from_currency={FromCurrency}&to_currency={ToCurrency}&apikey={_apiKey}";
                var response = await _httpClient.GetAsync(url);

                if (!response.IsSuccessStatusCode)
                    return null;

                RealtimeCurrencyExchangeRateResponse content = await response.Content.ReadFromJsonAsync<RealtimeCurrencyExchangeRateResponse>();

                if (content != null)
                {
                    _logger.LogInformation("Exchange rate for {FromCurrency}/{ToCurrency} successfully retrieved from API", FromCurrency, ToCurrency);
                    // Map external response to internal ExchangeRate model
                    ExchangeRate exchangeRate = new ExchangeRate
                    {
                        Bid = content.RealtimeCurrencyExchangeRate.BidPrice,
                        Ask = content.RealtimeCurrencyExchangeRate.AskPrice,
                        FromCurrency = FromCurrency,
                        ToCurrency = ToCurrency,
                    };
                    return exchangeRate;
                }
                else
                {
                    _logger.LogWarning("No content found for exchange rate {FromCurrency}/{ToCurrency} from API response", FromCurrency, ToCurrency);
                    return null;
                }
            }
            catch (Exception ex)
            {
                _logger.LogError(ex, "Error occurred while fetching exchange rate for {FromCurrency}/{ToCurrency}", FromCurrency, ToCurrency);
                return null;
            }
        }
    }
}