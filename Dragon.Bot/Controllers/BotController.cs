using DragonBot.Services.Services;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using Models;

namespace Dragon.Bot.Controllers
{
    [ApiController]
    [Route("[controller]")]
    public class BotController : ControllerBase
    {

        private readonly ILogger<BinanceController> _logger;
        private readonly DragonBotDbContext context;
        private readonly IAssetProvider provider;

        public BotController(ILogger<BinanceController> logger, DragonBotDbContext context, IAssetProvider provider)
        {
            _logger = logger;
            this.context = context;
            this.provider = provider;
        }

        [HttpGet("{shortName}/activate/{newValue}")]
        public async Task<IActionResult> Activate(string shortName, bool newValue)
        {
            var bot = await context.Bots.Where(e => e.ShortName == shortName).FirstOrDefaultAsync();

            if (bot != null)
            {
                bot.Active = newValue;
                await context.SaveChangesAsync();
                return Ok("Bot status " + (newValue ? "Active" : "Unactive"));
            }
            return BadRequest("Bot doesn't exists");
        }
    }
}