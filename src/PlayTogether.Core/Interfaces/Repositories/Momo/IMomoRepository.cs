using PlayTogether.Core.Dtos.Outcoming.Generic;
using System.Threading.Tasks;

namespace PlayTogether.Core.Interfaces.Repositories.Momo
{
    public interface IMomoRepository
    {
        Task<Result<string>> GenerateMomoLinkAsync();
    }
}