using System.Security.Cryptography;
using System.Threading.Tasks;
using MediatR;

namespace VFXFinancial.WebApi.Features.ExchangeRate.Commands
{
    /// <summary>
    /// DeleteExchangeRateCommand
    /// </summary>
    public class DeleteExchangeRateCommand : IRequest<bool>
    {
        /// <summary>
        /// Gets or sets the identifier.
        /// </summary>
        /// <value>
        /// The identifier.
        /// </value>
        public int Id { get; set; }
    }

}
