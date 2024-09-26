using Binance.Net.Clients;
using Binance.Net.Objects.Models.Spot;
using Microsoft.AspNetCore.Mvc;
using Models;

namespace Dragon.Bot.Controllers
{
    [ApiController]
    [Route("[controller]")]
    public class BinanceController : ControllerBase
    {

        private readonly ILogger<BinanceController> _logger;
        private readonly DragonBotDbContext context;

        public BinanceController(ILogger<BinanceController> logger, DragonBotDbContext context)
        {
            _logger = logger;
            this.context = context;
        }

        [HttpGet("orders/{pair}")]
        public async Task<IEnumerable<BinanceOrder>> GetOrders(string pair)
        {
            var restClient = new BinanceRestClient();

            return (await restClient.SpotApi.Trading.GetOrdersAsync(pair)).Data.OrderByDescending(e => e.CreateTime).ToList();
        }

    }
}