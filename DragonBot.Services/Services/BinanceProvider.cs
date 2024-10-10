using Binance.Net.Clients;

namespace DragonBot.Services.Services
{
    public class BinanceProvider : IAssetProvider
    {
        private BinanceRestClient restClient;

        public BinanceProvider()
        {
            restClient = new BinanceRestClient();
        }
        public async Task<decimal> GetBalance(string currency)
        {
            var balance = await restClient.SpotApi.Account.GetBalancesAsync(currency);

            if (balance.Error != null)
                throw new Exception(balance.Error.Message);

            return balance.Data.FirstOrDefault()?.Available ?? 0;
        }

        public async Task<decimal> GetAssetPriceAsync(string symbol)
        {
            return (await restClient.SpotApi.ExchangeData.GetPriceAsync(symbol))?.Data?.Price ?? 0;
        }
    }

    public interface IAssetProvider
    {
        Task<decimal> GetBalance(string currency);
        Task<decimal> GetAssetPriceAsync(string symbol);
    }
}
