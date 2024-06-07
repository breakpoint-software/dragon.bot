//namespace Dragon.Bot.Bot
//{
//    using Binance.Net.Clients;
//    using Binance.Net.Objects.Options;
//    using CryptoExchange.Net.Authentication;
//    using System;

//    class BinanceBot
//    {
//        private readonly BinanceRestClient client;

//        public BinanceBot()
//        {
//            string apiKey = "YOUR_API_KEY";
//            string apiSecret = "YOUR_API_SECRET";

//            client = new BinanceRestClient(new BinanceRestOptions
//            {
//                ApiCredentials = new ApiCredentials(apiKey, apiSecret)
//            });
//        }

//        public void Start()
//        {
//            MarketData marketData = new MarketData(client);
//            var prices = marketData.GetHistoricalData("BTCUSDT", 50);

//            string signal = TradingStrategy.SimpleMovingAverageStrategy(prices);

//            if (signal == "BUY")
//            {
//                ExecuteTrade("BTCUSDT", "buy", 0.001m);
//            }
//            else if (signal == "SELL")
//            {
//                ExecuteTrade("BTCUSDT", "sell", 0.001m);
//            }
//            else
//            {
//                Console.WriteLine("No trade signal.");
//            }
//        }

//        private void ExecuteTrade(string symbol, string side, decimal quantity)
//        {
//            if (side == "buy")
//            {
//                var result = client.Spot.Order.PlaceOrder(symbol, Binance.Net.Enums.OrderSide.Buy, Binance.Net.Enums.OrderType.Market, quantity: quantity);
//                if (result.Success)
//                {
//                    Console.WriteLine($"Bought {quantity} {symbol}");
//                }
//                else
//                {
//                    Console.WriteLine($"Buy order failed: {result.Error.Message}");
//                }
//            }
//            else if (side == "sell")
//            {
//                var result = client.Spot.Order.PlaceOrder(symbol, Binance.Net.Enums.OrderSide.Sell, Binance.Net.Enums.OrderType.Market, quantity: quantity);
//                if (result.Success)
//                {
//                    Console.WriteLine($"Sold {quantity} {symbol}");
//                }
//                else
//                {
//                    Console.WriteLine($"Sell order failed: {result.Error.Message}");
//                }
//            }
//        }
//    }

//    public class TradingStrategy
//    {
//        public static string SimpleMovingAverageStrategy(decimal[] prices)
//        {
//            decimal sma5 = CalculateSMA(prices, 5);
//            decimal sma10 = CalculateSMA(prices, 10);

//            if (sma5 > sma10)
//                return "BUY";
//            else if (sma5 < sma10)
//                return "SELL";
//            else
//                return "HOLD";
//        }

//        private static decimal CalculateSMA(decimal[] prices, int period)
//        {
//            if (prices.Length < period)
//                return 0;

//            decimal sum = 0;
//            for (int i = prices.Length - period; i < prices.Length; i++)
//            {
//                sum += prices[i];
//            }

//            return sum / period;
//        }
//    }

//    public class MarketData
//    {
//        private BinanceSocketClient client;

//        public MarketData(BinanceSocketClient client)
//        {
//            this.client = client;
//        }

//        public decimal[] GetHistoricalData(string symbol, int period)
//        {
//            var klines = client.SpotApi..GetKlines(symbol, Binance.Net.Enums.KlineInterval.OneHour, limit: period);
//            if (klines.Success)
//            {
//                return klines.Data.Select(k => k.Close).ToArray();
//            }
//            else
//            {
//                Console.WriteLine($"Error fetching data: {klines.Error.Message}");
//                return new decimal[0];
//            }
//        }
//    }


//}
