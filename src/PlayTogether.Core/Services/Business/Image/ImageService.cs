using Microsoft.Extensions.Logging;
using PlayTogether.Core.Dtos.Incoming.Business.Image;
using PlayTogether.Core.Dtos.Outcoming.Business.Image;
using PlayTogether.Core.Dtos.Outcoming.Generic;
using PlayTogether.Core.Interfaces.Repositories.Business;
using PlayTogether.Core.Interfaces.Services.Business;
using PlayTogether.Core.Parameters;
using System;
using System.Collections.Generic;
using System.Threading.Tasks;

namespace PlayTogether.Core.Services.Business.Image
{
    public class ImageService : IImageService
    {
        private readonly IImageRepository _imageRepository;
        private readonly ILogger<ImageService> _logger;

        public ImageService(
            IImageRepository imageRepository,
            ILogger<ImageService> logger)
        {
            _imageRepository = imageRepository;
            _logger = logger;
        }

        public async Task<Result<ImageGetByIdResponse>> CreateImageAsync(ImageCreateRequest request)
        {
            try {
                if (request is null) {
                    throw new ArgumentNullException(nameof(request));
                }
                return await _imageRepository.CreateImageAsync(request);
            }
            catch (Exception ex) {
                _logger.LogError($"Error while trying to call CreateImageAsync in service class, Error Message: {ex}.");
                throw;
            }
        }

        public async Task<Result<bool>> CreateMultiImageAsync(IList<ImageCreateRequest> request)
        {
            try {
                if (request is null) {
                    throw new ArgumentNullException(nameof(request));
                }
                return await _imageRepository.CreateMultiImageAsync(request);
            }
            catch (Exception ex) {
                _logger.LogError($"Error while trying to call CreateMultiImageAsync in service class, Error Message: {ex}.");
                throw;
            }
        }

        public async Task<Result<bool>> DeleteImageAsync(string imageId)
        {
            try {
                if (String.IsNullOrEmpty(imageId)) {
                    throw new ArgumentNullException(nameof(imageId));
                }
                return await _imageRepository.DeleteImageAsync(imageId);
            }
            catch (Exception ex) {
                _logger.LogError($"Error while trying to call DeleteImageAsync in service class, Error Message: {ex}.");
                throw;
            }
        }

        public async Task<Result<bool>> DeleteMultiImageAsync(IList<string> listImageId)
        {
            try {
                if (listImageId is null) {
                    throw new ArgumentNullException(nameof(listImageId));
                }
                return await _imageRepository.DeleteMultiImageAsync(listImageId);
            }
            catch (Exception ex) {
                _logger.LogError($"Error while trying to call DeleteMultiImageAsync in service class, Error Message: {ex}.");
                throw;
            }
        }

        public async Task<PagedResult<ImageGetByIdResponse>> GetAllImagesByUserId(string userId, ImageParameters param)
        {
            try {
                if (String.IsNullOrEmpty(userId)) {
                    throw new ArgumentNullException(nameof(userId));
                }
                return await _imageRepository.GetAllImagesByUserId(userId, param);
            }
            catch (Exception ex) {
                _logger.LogError($"Error while trying to call GetAllImagesByUserId in service class, Error Message: {ex}.");
                throw;
            }
        }

        public async Task<Result<ImageGetByIdResponse>> GetImageByIdAsync(string imageId)
        {
            try {
                if (String.IsNullOrEmpty(imageId)) {
                    throw new ArgumentNullException(nameof(imageId));
                }
                return await _imageRepository.GetImageByIdAsync(imageId);
            }
            catch (Exception ex) {
                _logger.LogError($"Error while trying to call GetImageByIdAsync in service class, Error Message: {ex}.");
                throw;
            }
        }
    }
}