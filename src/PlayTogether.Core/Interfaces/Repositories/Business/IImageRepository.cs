using System.Collections.Generic;
using PlayTogether.Core.Dtos.Incoming.Business.Image;
using PlayTogether.Core.Dtos.Outcoming.Business.Image;
using System.Threading.Tasks;
using PlayTogether.Core.Dtos.Outcoming.Generic;
using PlayTogether.Core.Parameters;

namespace PlayTogether.Core.Interfaces.Repositories.Business
{
    public interface IImageRepository
    {
        Task<ImageGetByIdResponse> CreateImageAsync(ImageCreateRequest request);
        Task<ImageGetByIdResponse> GetImageByIdAsync(string imageId);
        Task<bool> DeleteImageAsync(string imageId);
        Task<bool> CreateMultiImageAsync(IList<ImageCreateRequest> request);
        Task<bool> DeleteMultiImageAsync(IList<string> listImageId);
        Task<PagedResult<ImageGetByIdResponse>> GetAllImagesByUserId(string userId, ImageParameters param);

    }
}