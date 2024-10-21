using DragonBot.Services.Interfaces;
using Microsoft.AspNetCore.Mvc;
using Models.DTOs.Binance.Requests;

namespace Dragon.Bot.Controllers
{
    [ApiController]
    [Route("[controller]")]
    public class DragonSignalController : ControllerBase
    {

        private readonly ILogger<DragonSignalController> _logger;
        private readonly ISignalHandlerService signalHandler;

        public DragonSignalController(ILogger<DragonSignalController> logger, ISignalHandlerService signalHandler)
        {
            _logger = logger;
            this.signalHandler = signalHandler;
        }


        //[HttpGet]
        //[Route("process-simple-signal")]
        //[ServiceFilter(typeof(IpBlockingMiddleware))]
        //public async Task<IActionResult> ProcessSimpleSignal([FromQuery] string action, [FromQuery] decimal quantity)
        //{
        //    return await Process(new DragonSignalRequest
        //    {
        //        OrderId = "simply-action",
        //        Price = 0,
        //        Qty = quantity,
        //        Side = action,
        //        Ticker = "BTCUSDT"

        //    });
        //}

        [HttpPost]
        [Route("process")]
        //[ServiceFilter(typeof(ClientIpCheckActionFilter))]
        public async Task<IActionResult> Process(DragonSignalRequest signal)
        {
            try
            {
                return Ok(await signalHandler.HandleSignalAsync(signal));
            }
            catch (Exception ex)
            {
                return BadRequest(ex.Message);
            }
        }

    }

    public class OrderPlaced
    {
        public double Price { get; internal set; }
        public double Quantity { get; internal set; }
        public string OrderId { get; internal set; }
        public DateTime CreatedDate { get; internal set; }
    }


}