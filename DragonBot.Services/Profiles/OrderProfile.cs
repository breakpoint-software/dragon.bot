using AutoMapper;
using Binance.Net.Enums;
using Binance.Net.Objects.Models.Spot;

using Models.Domain;
using Models.DTOs.Binance.Responses;

namespace DragonBot.Services.Profiles
{
    public class OrderProfile : Profile
    {
        public OrderProfile()
        {
            CreateMap<BinancePlacedOrder, Order>()
                .ForMember(e => e.Id, opt => opt.Ignore())
                .ForAllMembers(x => x.Condition((source, destination, arg3, arg4, resolutionContext) =>
                {

                    destination.Pair = source.Symbol;
                    destination.Position = (OrderPosition)source.Side;
                    destination.SignalOrderId = "dragon-signal";
                    destination.BinanceOrderId = source.Id;
                    destination.CreatedDate = DateTime.Now;

                    return true;
                }));

            CreateMap<DragonSignalResponse, BinancePlacedOrder>().ForMember(e => e.Id, opt => opt.Ignore()).ReverseMap()
                .ForAllMembers(x => x.Condition((source, destination, arg3, arg4, resolutionContext) =>
                {
                    destination.UsdAmount = source.Price * source.Quantity;
                    destination.Status = (source.Status) switch
                    {
                        Binance.Net.Enums.OrderStatus.Filled => "Filled",
                        _ => "Not completed"
                    };
                    destination.Position = source.Side switch
                    {
                        OrderSide.Buy => "Buy",
                        OrderSide.Sell => "Sell",
                        _ => ""
                    };
                    destination.BinanceOrderId = source.Id;

                    return true;
                }));
        }
    }
}
