using PlayTogether.Core.Dtos.Incoming.Business.Image;
using PlayTogether.Core.Dtos.Outcoming.Business.Image;
using PlayTogether.Core.Dtos.Outcoming.Generic;
using PlayTogether.Core.Parameters;
using System.Collections.Generic;
using System.Threading.Tasks;

namespace PlayTogether.Core.Interfaces.Services.Business
{
    public interface IImageService
    {
        Task<ImageGetByIdResponse> CreateImageAsync(ImageCreateRequest request);
        Task<ImageGetByIdResponse> GetImageByIdAsync(string imageId);
        Task<bool> DeleteImageAsync(string imageId);
        Task<bool> CreateMultiImageAsync(IList<ImageCreateRequest> request);
        Task<bool> DeleteMultiImageAsync(IList<string> listImageId);
        Task<PagedResult<ImageGetByIdResponse>> GetAllImagesByUserId(string userId, ImageParameters param);
    }
}