using DragonBot.Services.Interfaces;
using Microsoft.AspNetCore.Mvc;
using Models.Domain;
using Models.DTOs.Requests;

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

        [HttpPost]
        [Route("process")]
        [ServiceFilter(typeof(IpBlockingMiddleware))]
        public async Task<IActionResult> Process(DragonSignalRequest signal)
        {
            Order order = null;
            try
            {
                order = await signalHandler.HandleSignalAsync(signal);
            }
            catch (Exception ex)
            {
                BadRequest(ex);
            }

            return Ok(order);
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