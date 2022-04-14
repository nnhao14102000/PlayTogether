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
        Task<Result<ImageGetByIdResponse>> CreateImageAsync(ImageCreateRequest request);
        Task<Result<ImageGetByIdResponse>> GetImageByIdAsync(string imageId);
        Task<Result<bool>> DeleteImageAsync(string imageId);
        Task<Result<bool>> CreateMultiImageAsync(IList<ImageCreateRequest> request);
        Task<Result<bool>> DeleteMultiImageAsync(IList<string> listImageId);
        Task<PagedResult<ImageGetByIdResponse>> GetAllImagesByUserId(string userId, ImageParameters param);
    }
}