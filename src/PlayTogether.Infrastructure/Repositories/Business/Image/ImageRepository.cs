using System.Collections;
using AutoMapper;
using PlayTogether.Core.Dtos.Incoming.Business.Image;
using PlayTogether.Core.Dtos.Outcoming.Business.Image;
using PlayTogether.Core.Interfaces.Repositories.Business;
using PlayTogether.Infrastructure.Data;
using System;
using System.Collections.Generic;
using System.Threading.Tasks;
using PlayTogether.Core.Dtos.Outcoming.Generic;
using PlayTogether.Core.Parameters;
using System.Linq;
using PlayTogether.Core.Dtos.Incoming.Generic;
using Microsoft.EntityFrameworkCore;

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

        public async Task<PagedResult<ImageGetByIdResponse>> GetAllImagesByUserId(string userId, ImageParameters param)
        {
            var result = new PagedResult<ImageGetByIdResponse>();
            var user = await _context.AppUsers.FindAsync(userId);
            if (user is null) {
                result.Error = Helpers.ErrorHelpers.PopulateError(404, APITypeConstants.NotFound_404, ErrorMessageConstants.UserNotFound);
                return result;
            }
            if (user.IsActive is false) {
                result.Error = Helpers.ErrorHelpers.PopulateError(400, APITypeConstants.BadRequest_400, ErrorMessageConstants.DisableUser);
                return result;
            }
            var images = await _context.Images.Where(x => x.UserId == user.Id).ToListAsync();
            var query = images.AsQueryable();
            OrderNewImage(ref query, param.IsNew);
            images = query.ToList();
            var response = _mapper.Map<List<ImageGetByIdResponse>>(images);
            return PagedResult<ImageGetByIdResponse>.ToPagedList(response, param.PageNumber, param.PageSize);
        }

        private void OrderNewImage(ref IQueryable<Entities.Image> query, bool? isNew)
        {
            if (!query.Any() || isNew is null) {
                return;
            }
            if (isNew is true) {
                query = query.OrderByDescending(x => x.CreatedDate);
            }
            else {
                query = query.OrderBy(x => x.CreatedDate);
            }
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