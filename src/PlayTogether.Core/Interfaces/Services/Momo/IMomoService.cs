using PlayTogether.Core.Dtos.Incoming.Momo;
using PlayTogether.Core.Dtos.Outcoming.Generic;
using System.Threading.Tasks;

namespace PlayTogether.Core.Interfaces.Services.Momo
{
    public interface IMomoService
    {
        Task<Result<string>> GenerateMomoLinkAsync(WebPaymentRequest request);
    }
}