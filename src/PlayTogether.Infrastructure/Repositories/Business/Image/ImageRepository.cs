using AutoMapper;
using PlayTogether.Core.Dtos.Incoming.Business.Image;
using PlayTogether.Core.Dtos.Outcoming.Business.Image;
using PlayTogether.Core.Interfaces.Repositories.Business;
using PlayTogether.Infrastructure.Data;
using System;
using System.Threading.Tasks;

namespace PlayTogether.Infrastructure.Repositories.Business.Image
{
    public class ImageRepository : IImageRepository
    {
        private readonly IMapper _mapper;
        private readonly AppDbContext _context;

        public ImageRepository(
            IMapper mapper, AppDbContext context)
        {
            _mapper = mapper;
            _context = context ?? throw new ArgumentNullException(nameof(context));
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

        public async Task<bool> DeleteImageAsync(string id)
        {
            var image = await _context.Images.FindAsync(id);
            if (image is null) {
                return false;
            }
            _context.Images.Remove(image);
            if ((await _context.SaveChangesAsync() >= 0)) {
                return true;
            }
            return false;
        }

        public async Task<ImageGetByIdResponse> GetImageByIdAsync(string id)
        {
            var image = await _context.Images.FindAsync(id);
            if (image is not null) {
                return _mapper.Map<ImageGetByIdResponse>(image);
            }
            return null;
        }
    }
}