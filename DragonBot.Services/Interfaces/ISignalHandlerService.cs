using Models.Domain;
using Models.DTOs.Requests;

namespace DragonBot.Services.Interfaces
{
    public interface ISignalHandlerService
    {
        Task<Order> HandleSignalAsync(DragonSignalRequest signal);
    }
}