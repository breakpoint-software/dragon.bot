using Models.DTOs.Binance.Requests;
using Models.DTOs.Binance.Responses;

namespace DragonBot.Services.Interfaces
{
    public interface ISignalHandlerService
    {
        Task<DragonSignalResponse> HandleSignalAsync(DragonSignalRequest signal);
    }
}