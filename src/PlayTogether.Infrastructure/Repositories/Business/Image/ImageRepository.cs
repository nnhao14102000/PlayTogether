using System.Collections;
using AutoMapper;
using PlayTogether.Core.Dtos.Incoming.Business.Image;
using PlayTogether.Core.Dtos.Outcoming.Business.Image;
using PlayTogether.Core.Interfaces.Repositories.Business;
using PlayTogether.Infrastructure.Data;
using System;
using System.Collections.Generic;
using System.Threading.Tasks;

namespace PlayTogether.Infrastructure.Repositories.Business.Image
{
    public class ImageRepository : BaseRepository, IImageRepository
    {
        public ImageRepository(IMapper mapper, AppDbContext context) : base(mapper, context)
        {
        }

        public async Task<ImageGetByIdResponse> CreateImageAsync(ImageCreateRequest request)
        {
            var model = _mapper.Map<Entities.Image>(request);
            _context.Images.Add(model);
            if ((await _context.SaveChangesAsync() >= 0)) {
                return _mapper.Map<ImageGetByIdResponse>(model);
            }
            return null;
        }

        public async Task<bool> CreateMultiImageAsync(IList<ImageCreateRequest> request)
        {
            var model = _mapper.Map<IList<Entities.Image>>(request);
            _context.Images.AddRange(model);

            return await _context.SaveChangesAsync() >= 0;
        }

        public async Task<bool> DeleteImageAsync(string imageId)
        {
            var image = await _context.Images.FindAsync(imageId);
            if (image is null) {
                return false;
            }
            _context.Images.Remove(image);
            return (await _context.SaveChangesAsync() >= 0);
        }

        public async Task<bool> DeleteMultiImageAsync(IList<string> listImageId)
        {
            var listImage = new List<Entities.Image>();
            foreach (var imageId in listImageId) {
                var image = await _context.Images.FindAsync(imageId);
                if (image is null) {
                    return false;
                }
                listImage.Add(image);
            }
            _context.Images.RemoveRange(listImage);
            return (await _context.SaveChangesAsync() >= 0);
        }

        public async Task<ImageGetByIdResponse> GetImageByIdAsync(string imageId)
        {
            var image = await _context.Images.FindAsync(imageId);
            if (image is not null) {
                return _mapper.Map<ImageGetByIdResponse>(image);
            }
            return null;
        }
    }
}