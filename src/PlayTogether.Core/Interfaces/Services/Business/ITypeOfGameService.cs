using PlayTogether.Core.Dtos.Incoming.Business.TypeOfGame;
using PlayTogether.Core.Dtos.Outcoming.Business.TypeOfGame;
using System.Threading.Tasks;

namespace PlayTogether.Core.Interfaces.Services.Business
{
    public interface ITypeOfGameService
    {
        Task<bool> CreateTypeOfGameAsync(TypeOfGameCreateRequest request);
        Task<TypeOfGameGetByIdResponse> GetTypeOfGameByIdAsync(string typeOfGameId);
        Task<bool> DeleteTypeOfGameAsync(string typeOfGameId);
    }
}