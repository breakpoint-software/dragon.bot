using Binance.Net.Clients;
using Binance.Net.Enums;
using Binance.Net.Objects.Models.Spot;

namespace DragonBot.Services
{
    public class BinanceService : IAssetProvider
    {
        private BinanceRestClient restClient;

        public BinanceService()
        {
            restClient = new BinanceRestClient();
        }

        public async Task<BinancePlacedOrder> PlaceSpotLongOrder(string symbol, decimal quantity, decimal price)
        {
            var tickerResult = await restClient.SpotApi.Trading.PlaceOrderAsync(
                symbol: symbol,
                side: OrderSide.Buy,
                type: SpotOrderType.Limit,
                quantity: quantity,
                price: price,
                timeInForce: TimeInForce.GoodTillCanceled
                );
            return tickerResult.Data;
        }
    }
}
