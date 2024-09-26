using Binance.Net.Clients;
using Binance.Net.Enums;
using DragonBot.Services.Interfaces;
using Google.Cloud.Firestore;
using Models;
using Models.Domain;
using Models.DTOs.Requests;

namespace DragonBot.Services.Services
{
    public class BinanceSignalHandlerService : ISignalHandlerService
    {
        private readonly DragonBotDbContext context;
        private BinanceRestClient restClient;

        public BinanceSignalHandlerService(DragonBotDbContext context)
        {
            restClient = new BinanceRestClient();
            this.context = context;
        }

        public async Task<Order> HandleSignalAsync(DragonSignalRequest signal)
        {
            Order order = null;
            try
            {

                var tickerResult = await restClient.SpotApi.Trading.PlaceOrderAsync(
                    symbol: signal.Ticker,
                    side: signal.Side == "buy" ? Binance.Net.Enums.OrderSide.Buy : Binance.Net.Enums.OrderSide.Sell,
                    type: Binance.Net.Enums.SpotOrderType.Limit,
                    quantity: Convert.ToDecimal(signal.Qty),
                    price: Convert.ToDecimal(signal.Price),
                    timeInForce: Binance.Net.Enums.TimeInForce.GoodTillCanceled);

                if (!tickerResult.Success)
                {
                    await SaveLog("OrderError", tickerResult.Error.Message);
                    throw new Exception(tickerResult.Error.Message);
                }

                await SaveLog("Success", "Order placed " + tickerResult.Data.Id);

                order = new Models.Domain.Order
                {
                    Pair = tickerResult.Data.Symbol,
                    Position = tickerResult.Data.Side == OrderSide.Buy ? OrderPosition.Long : OrderPosition.Short,
                    Price = tickerResult.Data.Price,
                    Quantity = tickerResult.Data.Quantity,
                    Fee = (tickerResult.Data.Price * tickerResult.Data.Quantity) * 0.001m,
                    UsdAmount = (tickerResult.Data.Price * tickerResult.Data.Quantity)
                };

                context.Orders.Add(order);

                await context.SaveChangesAsync();
            }
            catch (Exception ex)
            {
                await SaveLog("error", ex.Message);
                throw ex;
            }

            return order;
        }

        private async Task SaveLog(string type, string message)
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

            await db.Collection("logs").AddAsync(new { Type = type, message = message, Date = DateTime.UtcNow });
        }
    }
}
