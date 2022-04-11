using PlayTogether.Core.Dtos.Incoming.Business.TypeOfGame;
using PlayTogether.Core.Dtos.Outcoming.Business.TypeOfGame;
using PlayTogether.Core.Dtos.Outcoming.Generic;
using System.Threading.Tasks;

namespace PlayTogether.Core.Interfaces.Repositories.Business
{
    public interface ITypeOfGameRepository
    {
        Task<Result<bool>> CreateTypeOfGameAsync(TypeOfGameCreateRequest request);
        Task<Result<TypeOfGameGetByIdResponse>> GetTypeOfGameByIdAsync(string typeOfGameId);
        Task<Result<bool>> DeleteTypeOfGameAsync(string typeOfGameId);
    }
}