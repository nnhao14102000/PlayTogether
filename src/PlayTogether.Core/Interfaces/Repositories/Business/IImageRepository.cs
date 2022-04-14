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
        Task<Result<ImageGetByIdResponse>> CreateImageAsync(ImageCreateRequest request);
        Task<Result<ImageGetByIdResponse>> GetImageByIdAsync(string imageId);
        Task<Result<bool>> DeleteImageAsync(string imageId);
        Task<Result<bool>> CreateMultiImageAsync(IList<ImageCreateRequest> request);
        Task<Result<bool>> DeleteMultiImageAsync(IList<string> listImageId);
        Task<PagedResult<ImageGetByIdResponse>> GetAllImagesByUserId(string userId, ImageParameters param);

    }
}