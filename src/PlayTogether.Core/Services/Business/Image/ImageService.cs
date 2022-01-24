using Microsoft.Extensions.Logging;
using PlayTogether.Core.Dtos.Incoming.Business.Image;
using PlayTogether.Core.Dtos.Outcoming.Business.Image;
using PlayTogether.Core.Interfaces.Repositories.Business.Image;
using PlayTogether.Core.Interfaces.Services.Business.Image;
using System;
using System.Threading.Tasks;

namespace PlayTogether.Core.Services.Business.Image
{
    public class ImageService : IImageService
    {
        private readonly IImageRepository _imageRepository;
        private readonly ILogger<ImageService> _logger;

        public ImageService(IImageRepository imageRepository, ILogger<ImageService> logger)
        {
            _imageRepository = imageRepository ?? throw new ArgumentNullException(nameof(imageRepository));
            _logger = logger ?? throw new ArgumentNullException(nameof(logger));
        }

        public async Task<ImageGetByIdResponse> CreateImageAsync(ImageCreateRequest request)
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

        public async Task<bool> DeleteImageAsync(string id)
        {
            try {
                if (id is null) {
                    throw new ArgumentNullException(nameof(id));
                }
                return await _imageRepository.DeleteImageAsync(id);
            }
            catch (Exception ex) {
                _logger.LogError($"Error while trying to call DeleteImageAsync in service class, Error Message: {ex}.");
                throw;
            }
        }

        public async Task<ImageGetByIdResponse> GetImageByIdAsync(string id)
        {
            try {
                return await _imageRepository.GetImageByIdAsync(id);
            }
            catch (Exception ex) {
                _logger.LogError($"Error while trying to call GetImageByIdAsync in service class, Error Message: {ex}.");
                throw;
            }
        }
    }
}