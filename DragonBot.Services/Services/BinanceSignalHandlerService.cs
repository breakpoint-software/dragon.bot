using AutoMapper;
using Binance.Net.Clients;
using Binance.Net.Objects.Models.Spot;
using CryptoExchange.Net.Objects;
using DragonBot.Services.Interfaces;
using Google.Cloud.Firestore;
using Microsoft.EntityFrameworkCore;
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
            Bot bot = await context.Bots.Where(e => signal.BotShortName == e.ShortName && e.Active).FirstOrDefaultAsync();

            if (bot == null)
                throw new Exception("Bot not active");

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
                        quantity: Math.Round(Convert.ToDecimal(signal.Qty), 5),
                        price: price,
                        timeInForce: Binance.Net.Enums.TimeInForce.FillOrKill);

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
                            await SaveLog("order-error", $"Order not filled {tickerResult.Data.Status}.");

                    }
                    catch (Exception ex)
                    {
                        await SaveLog("binance-connection", ex.Message);
                    }

                    if (attempt >= 5)
                    {
                        var error = tickerResult?.Error?.Message ?? (tickerResult?.Data?.Status != Binance.Net.Enums.OrderStatus.Filled ? "Order not filled" : "");
                        throw new Exception($"Error {error}");
                    }
                }

                await SaveLog("Success", "Order placed " + tickerResult.Data.Id);

                context.Orders.Add(mapper.Map<Order>(tickerResult.Data));

                await context.SaveChangesAsync();

                return mapper.Map<DragonSignalResponse>(tickerResult.Data);
            }
            catch (Exception ex)
            {
                await SaveLog("error", ex.Message);
                throw;
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
