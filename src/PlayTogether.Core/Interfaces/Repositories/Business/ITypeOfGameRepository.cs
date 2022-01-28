using PlayTogether.Core.Dtos.Incoming.Business.TypeOfGame;
using PlayTogether.Core.Dtos.Outcoming.Business.TypeOfGame;
using System.Threading.Tasks;

namespace PlayTogether.Core.Interfaces.Repositories.Business
{
    public interface ITypeOfGameRepository
    {
        Task<TypeOfGameGetByIdResponse> CreateTypeOfGameAsync(TypeOfGameCreateRequest request);
        Task<TypeOfGameGetByIdResponse> GetTypeOfGameByIdAsync(string id);
        Task<bool> DeleteTypeOfGameAsync(string id);
    }
}