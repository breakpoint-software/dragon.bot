using Binance.Net.Clients;
using Binance.Net.Objects.Models.Spot;
using Microsoft.AspNetCore.Mvc;

namespace Dragon.Bot.Controllers
{
    [ApiController]
    [Route("[controller]")]
    public class OrderController : ControllerBase
    {

        private readonly ILogger<OrderController> _logger;

        public OrderController(ILogger<OrderController> logger)
        {
            _logger = logger;
        }

        [HttpGet("{pair}")]
        public async Task<IEnumerable<BinanceOrder>> GetAll(string pair)
        {

            var restClient = new BinanceRestClient();

            return (await restClient.SpotApi.Trading.GetOrdersAsync(pair)).Data;
        }
    }
}