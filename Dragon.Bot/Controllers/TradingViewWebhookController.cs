using Binance.Net.Clients;
using Binance.Net.Objects.Models.Spot;
using CryptoExchange.Net.Objects;
using Google.Cloud.Firestore;
using Microsoft.AspNetCore.Mvc;

namespace Dragon.Bot.Controllers
{
    [ApiController]
    [Route("[controller]")]
    public class DragonSignalController : ControllerBase
    {

        private readonly ILogger<DragonSignalController> _logger;

        public DragonSignalController(ILogger<DragonSignalController> logger)
        {
            _logger = logger;
        }

        [HttpPost]
        [Route("process")]
        public async Task<BinancePlacedOrder> Receive(DragonSignal signal)
        {

            var restClient = new BinanceRestClient();

            var tickerResult = await restClient.SpotApi.Trading.PlaceOrderAsync(
                symbol: signal.Ticker,
                side: signal.Side == "buy" ? Binance.Net.Enums.OrderSide.Buy : Binance.Net.Enums.OrderSide.Sell,
                type: Binance.Net.Enums.SpotOrderType.Limit,
                quantity: Convert.ToDecimal(signal.Qty),
                price: Convert.ToDecimal(signal.Price),
                timeInForce: Binance.Net.Enums.TimeInForce.GoodTillCanceled
                );

            await SaveLog(tickerResult, signal);

            return tickerResult.Data;
        }

        private async Task SaveLog(WebCallResult<BinancePlacedOrder> result, DragonSignal signal)
        {
            string json;
            using (StreamReader r = new StreamReader("ldragonbotdb.json"))
            {
                json = r.ReadToEnd();
            };

            FirestoreDb db = new FirestoreDbBuilder
            {
                ProjectId = "dragonbotdb-fdda7",
                JsonCredentials = json
            }.Build();

            if (result.Success)
            {
                await db.Collection("placed-orders").AddAsync(
                    new OrderPlaced
                    {
                        Price = Convert.ToDouble(result.Data.Price),
                        Quantity = Convert.ToDouble(result.Data.Quantity),
                        OrderId = result.Data.ClientOrderId,
                        CreatedDate = result.Data.CreateTime,
                    });
            }

            await db.Collection("signals").AddAsync(signal);
        }
    }

    [FirestoreData]
    public class OrderPlaced
    {
        [FirestoreProperty()]
        public double Price { get; internal set; }
        [FirestoreProperty()]
        public double Quantity { get; internal set; }
        [FirestoreProperty]
        public string OrderId { get; internal set; }
        [FirestoreProperty]
        public DateTime CreatedDate { get; internal set; }
    }

    [FirestoreData]
    public class DragonSignal

    {
        [FirestoreProperty]
        public string Ticker { get; set; }
        [FirestoreProperty]
        public double Price { get; set; }
        [FirestoreProperty]
        public double Qty { get; set; }
        [FirestoreProperty]
        public string Interval { get; set; }
        [FirestoreProperty]
        public string Side { get; set; }
        [FirestoreProperty]
        public string Date { get; set; }

    }
}