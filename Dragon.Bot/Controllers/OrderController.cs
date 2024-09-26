using Binance.Net.Clients;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using Models;
using Models.Domain;
using Models.DTOs.Responses;

namespace Dragon.Bot.Controllers
{
    [ApiController]
    [Route("[controller]")]
    public class OrderController : ControllerBase
    {

        private readonly ILogger<OrderController> _logger;
        private readonly DragonBotDbContext context;

        public OrderController(ILogger<OrderController> logger, DragonBotDbContext context)
        {
            _logger = logger;
            this.context = context;
        }

        [HttpGet("{pair}")]
        public async Task<IEnumerable<Order>> GetAll(string pair)
        {
            var restClient = new BinanceRestClient();

            return (await context.Orders.OrderByDescending(e => e.CreatedDate).ToListAsync());
        }


        [HttpGet("tradebalance/{pair}")]
        public async Task<IActionResult> GetTradeBalance(string pair)
        {

            var restClient = new BinanceRestClient();

            return Ok(new { TradeBalance = await context.Orders.SumAsync(e => e.Position == Models.Domain.OrderPosition.Long ? -1 * e.UsdAmount : e.UsdAmount) });
        }

        [HttpGet("tradebalancedetailed/{pair}")]
        public IActionResult GetTradeBalanceDetatailed(string pair)
        {

            var restClient = new BinanceRestClient();
            var currentTotal = 0.0m;

            return Ok(context.Orders.ToList().Select(i =>
            {
                currentTotal += i.Position == Models.Domain.OrderPosition.Long ? -1 * i.UsdAmount : i.UsdAmount;
                return new BalanceDetailResponse
                {
                    OrderId = i.Id,
                    Position = i.Position == Models.Domain.OrderPosition.Long ? "buy" : "sell",
                    Quantity = i.Quantity,
                    Price = i.Price,
                    UsdAmount = i.Position == OrderPosition.Long ? -1 * i.UsdAmount : i.UsdAmount,
                    AccountBalance = currentTotal
                };
            }).ToList());
        }

    }


}