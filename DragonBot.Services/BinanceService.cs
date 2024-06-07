using Binance.Net.Clients;

namespace DragonBot.Services
{
    public class BinanceService
    {
        public BinanceService()
        {

        }

        public async Task GetAsync()
        {
            var restClient = new BinanceRestClient();
            var tickerResult = await restClient.SpotApi.Trading.PlaceOrderAsync(
                "BTCUSDT",
                Binance.Net.Enums.OrderSide.Buy,
                Binance.Net.Enums.SpotOrderType.Limit,
                quantity: 0.00001m,
                price: 67500
                );

        }
    }

    interface IBinanceService
    {

    }
}
