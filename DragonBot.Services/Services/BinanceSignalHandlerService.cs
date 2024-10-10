using AutoMapper;
using Binance.Net.Clients;
using Binance.Net.Objects.Models.Spot;
using CryptoExchange.Net.Objects;
using DragonBot.Services.Interfaces;
using Google.Cloud.Firestore;
using Models;
using Models.Domain;
using Models.DTOs.Binance.Requests;
using Models.DTOs.Binance.Responses;

namespace DragonBot.Services.Services
{
    public class BinanceSignalHandlerService : ISignalHandlerService
    {
        private readonly DragonBotDbContext context;
        private readonly IAssetProvider provider;
        private readonly IMapper mapper;
        private BinanceRestClient restClient;

        public BinanceSignalHandlerService(DragonBotDbContext context, IAssetProvider provider, IMapper autoMapper)
        {
            restClient = new BinanceRestClient();
            this.context = context;
            this.provider = provider;
            this.mapper = autoMapper;
        }

        public async Task<DragonSignalResponse> HandleSignalAsync(DragonSignalRequest signal)
        {
            Order order = null;
            try
            {
                var attempt = 0;
                var price = await provider.GetAssetPriceAsync(signal.Ticker);

                var createOrder = async () =>
                {
                    var order = await restClient.SpotApi.Trading.PlaceOrderAsync(
                        symbol: signal.Ticker,
                        side: signal.Side == "buy" ? Binance.Net.Enums.OrderSide.Buy : Binance.Net.Enums.OrderSide.Sell,
                        type: Binance.Net.Enums.SpotOrderType.Limit,
                        quantity: Convert.ToDecimal(signal.Qty),
                        price: price,
                        timeInForce: Binance.Net.Enums.TimeInForce.FillOrKill);

                    if (order.Success && order.Data.Status == Binance.Net.Enums.OrderStatus.Filled)
                    {
                        var result = await restClient.SpotApi.Trading.PlaceOrderAsync(
                            symbol: signal.Ticker,
                            side: order.Data.Side == Binance.Net.Enums.OrderSide.Buy ? Binance.Net.Enums.OrderSide.Sell : Binance.Net.Enums.OrderSide.Buy,
                            type: Binance.Net.Enums.SpotOrderType.StopLoss,
                            stopPrice: Math.Round(price * .98m, 2),
                            quantity: Convert.ToDecimal(signal.Qty));
                    }

                    return order;

                };

                WebCallResult<BinancePlacedOrder> tickerResult = null;
                while (true)
                {
                    try
                    {
                        attempt++;
                        tickerResult = await createOrder();

                        if (tickerResult.Success && tickerResult.Data.Status == Binance.Net.Enums.OrderStatus.Filled)
                            break;

                        if (!tickerResult.Success)
                            await SaveLog("order-error", tickerResult.Error.Message);
                        else if (tickerResult.Data.Status != Binance.Net.Enums.OrderStatus.Filled)
                            await SaveLog("order-error", $"order not filled {tickerResult.Data.Status}");

                    }
                    catch (Exception ex)
                    {
                        await SaveLog("binance-connection", ex.Message);
                    }

                    if (attempt >= 5)
                        throw new Exception("Impossible to place the signal on Binance");
                }

                await SaveLog("Success", "Order placed " + tickerResult.Data.Id);

                context.Orders.Add(mapper.Map<Order>(tickerResult.Data));

                await context.SaveChangesAsync();

                return mapper.Map<DragonSignalResponse>(tickerResult.Data);
            }
            catch (Exception ex)
            {
                await SaveLog("error", ex.Message);
                throw ex;
            }
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
