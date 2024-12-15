using MediatR;
using Microsoft.AspNetCore.Mvc;
using VFXFinancial.WebApi.Features.ExchangeRate.Commands;
using VFXFinancial.WebApi.Features.ExchangeRate.Queries;
using VFXFinancial.WebApi.Models.DTOs;

namespace VFXFinancial.WebApi.Controllers
{
    [ApiController]
    [Route("api/[controller]")]
    public class ExchangeRateController : ControllerBase
    {
        private readonly ILogger<ExchangeRateController> _logger;
        private readonly IMediator _mediator;

        /// <summary>
        /// Initializes a new instance of the <see cref="ExchangeRateController"/> class.
        /// </summary>
        /// <param name="mediator">The mediator.</param>
        /// <param name="logger">The logger.</param>
        public ExchangeRateController(IMediator mediator, ILogger<ExchangeRateController> logger)
        {
            _logger = logger;
            _mediator = mediator;
        }

        /// <summary>
        /// Creates the specified request.
        /// </summary>
        /// <param name="Request">The request.</param>
        /// <returns></returns>
        [HttpPost]
        public async Task<IActionResult> Create([FromBody] CreateExchangeRateRequest Request)
        {
            _logger.LogInformation("Received request to create a new exchange rate");

            try
            {
                var id = await _mediator.Send(new CreateExchangeRateCommand { FromCurrency = Request.FromCurrency, ToCurrency = Request.ToCurrency, Bid = Request.Bid, Ask = Request.Ask });
                _logger.LogInformation("Successfully created exchange rate with ID {Id}", id);

                return CreatedAtAction(nameof(Get), new { FromCurrency = Request.FromCurrency, ToCurrency = Request.ToCurrency }, id);
            }
            catch (Exception ex)
            {
                _logger.LogError(ex, "Error occurred while creating exchange rate");
                return StatusCode(500, "Internal server error");
            }
        }

        /// <summary>
        /// Gets the specified from currency.
        /// </summary>
        /// <param name="FromCurrency">From currency.</param>
        /// <param name="ToCurrency">To currency.</param>
        /// <returns></returns>
        [HttpGet("{FromCurrency}/{ToCurrency}")]
        public async Task<IActionResult> Get(string FromCurrency, string ToCurrency)
        {
            _logger.LogInformation("Received request to get exchange rate for {FromCurrency}/{ToCurrency}", FromCurrency, ToCurrency);

            try
            {
                var rate = await _mediator.Send(new GetExchangeRateQuery { FromCurrency = FromCurrency, ToCurrency = ToCurrency });
                if (rate == null)
                {
                    _logger.LogWarning("Exchange rate for {FromCurrency}/{ToCurrency} not found", FromCurrency, ToCurrency);
                    return NotFound($"Exchange rate for {FromCurrency}/{ToCurrency} not found.");
                }

                return Ok(rate);
            }
            catch (Exception ex)
            {
                _logger.LogError(ex, "Error occurred while getting exchange rate for {FromCurrency}/{ToCurrency}", FromCurrency, ToCurrency);
                return StatusCode(500, "Internal server error");
            }
        }

        /// <summary>
        /// Updates the specified identifier.
        /// </summary>
        /// <param name="Id">The identifier.</param>
        /// <param name="Request">The request.</param>
        /// <returns></returns>
        [HttpPut("{Id}")]
        public async Task<IActionResult> Update(int Id, [FromBody] UpdateExchangeRateRequest Request)
        {
            _logger.LogInformation("Received request to PUT update on exchange rate with ID {Id}", Id);

            try
            {
                if (Id != Request.Id)
                {
                    _logger.LogWarning("ID in the route ({RouteId}) does not match ID in the body ({BodyId})", Id, Request.Id);
                    return BadRequest("ID in the route does not match ID in the body.");
                }

                var result = await _mediator.Send(new UpdateExchangeRateCommand { Id = Request.Id, Ask = Request.Ask, Bid = Request.Bid});

                if (!result)
                {
                    _logger.LogWarning("Exchange rate with ID {Id} not found", Id);
                    return NotFound($"Exchange rate with ID {Id} not found.");
                }

                _logger.LogInformation("Successfully updated exchange rate with ID {Id}", Id);
                return NoContent();
            }
            catch (Exception ex)
            {
                _logger.LogError(ex, "Error occurred while updating exchange rate with ID {Id}", Id);
                return StatusCode(500, "Internal server error.");
            }
        }

        /// <summary>
        /// Deletes the specified identifier.
        /// </summary>
        /// <param name="Id">The identifier.</param>
        /// <returns></returns>
        [HttpDelete("{Id}")]
        public async Task<IActionResult> Delete(int Id)
        {
            _logger.LogInformation("Received request to DELETE exchange rate with ID {Id}", Id);

            try
            {
                var result = await _mediator.Send(new DeleteExchangeRateCommand { Id = Id });
                if (!result)
                {
                    _logger.LogWarning("Exchange rate with ID {Id} not found", Id);
                    return NotFound($"Exchange rate with ID {Id} not found.");
                }

                _logger.LogInformation("Successfully deleted exchange rate with ID {Id}", Id);
                return NoContent();
            }
            catch (Exception ex)
            {
                _logger.LogError(ex, "Error occurred while deleting exchange rate with ID {Id}", Id);
                return StatusCode(500, "Internal server error.");
            }
        }
    }
}
   
