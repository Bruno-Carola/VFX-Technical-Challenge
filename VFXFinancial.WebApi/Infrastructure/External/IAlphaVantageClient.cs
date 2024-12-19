using VFXFinancial.WebApi.Models.Domain;

namespace VFXFinancial.WebApi.Infrastructure.External
{
    public interface IAlphaVantageClient
    {
        Task<ExchangeRate> FetchExchangeRateAsync(string FromCurrency, string ToCurrency);
    }
}
