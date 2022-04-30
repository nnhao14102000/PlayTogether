using Newtonsoft.Json.Linq;
using PlayTogether.Core.Dtos.Incoming.Momo;
using PlayTogether.Core.Dtos.Outcoming.Generic;
using System.Threading.Tasks;

namespace PlayTogether.Core.Interfaces.Repositories.Momo
{
    public interface IMomoRepository
    {
        Task<Result<JObject>> GenerateMomoLinkAsync(WebPaymentRequest request);
    }
}