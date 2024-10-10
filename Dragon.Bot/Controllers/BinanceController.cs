using Binance.Net.Clients;
using Binance.Net.Objects.Models.Spot;
using DragonBot.Services.Services;
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
        private readonly IAssetProvider provider;

        public BinanceController(ILogger<BinanceController> logger, DragonBotDbContext context, IAssetProvider provider)
        {
            _logger = logger;
            this.context = context;
            this.provider = provider;
        }

        [HttpGet("orders/{pair}")]
        public async Task<IEnumerable<BinanceOrder>> GetOrders(string pair)
        {
            var restClient = new BinanceRestClient();

            return (await restClient.SpotApi.Trading.GetOrdersAsync(pair)).Data.OrderByDescending(e => e.CreateTime).ToList();
        }


        [HttpGet("user/balance/{asset}")]
        public async Task<decimal> GetBalance(string asset)
        {
            return (await provider.GetBalance(asset));
        }

    }
}