using PlayTogether.Core.Dtos.Incoming.Business.Image;
using PlayTogether.Core.Dtos.Outcoming.Business.Image;
using System.Threading.Tasks;

namespace PlayTogether.Core.Interfaces.Services.Business
{
    public interface IImageService
    {
        Task<ImageGetByIdResponse> CreateImageAsync(ImageCreateRequest request);
        Task<ImageGetByIdResponse> GetImageByIdAsync(string id);
        Task<bool> DeleteImageAsync(string id);
    }
}