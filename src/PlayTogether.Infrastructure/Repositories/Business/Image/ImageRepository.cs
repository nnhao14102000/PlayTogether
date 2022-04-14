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

        public async Task<Result<ImageGetByIdResponse>> CreateImageAsync(ImageCreateRequest request)
        {
            var result = new Result<ImageGetByIdResponse>();
            var model = _mapper.Map<Core.Entities.Image>(request);
            _context.Images.Add(model);
            if ((await _context.SaveChangesAsync() >= 0)) {
                var response = _mapper.Map<ImageGetByIdResponse>(model);
                result.Content = response;
                return result;
            }
            result.Error = Helpers.ErrorHelpers.PopulateError(0, APITypeConstants.SaveChangesFailed, ErrorMessageConstants.SaveChangesFailed);
            return result;
        }

        public async Task<Result<bool>> CreateMultiImageAsync(IList<ImageCreateRequest> request)
        {
            var result = new Result<bool>();
            var model = _mapper.Map<IList<Core.Entities.Image>>(request);
            _context.Images.AddRange(model);

            if (await _context.SaveChangesAsync() >= 0) {
                result.Content = true;
                return result;
            }
            result.Error = Helpers.ErrorHelpers.PopulateError(0, APITypeConstants.SaveChangesFailed, ErrorMessageConstants.SaveChangesFailed);
            return result;
        }

        public async Task<Result<bool>> DeleteImageAsync(string imageId)
        {
            var result = new Result<bool>();
            var image = await _context.Images.FindAsync(imageId);
            if (image is null) {
                result.Error = Helpers.ErrorHelpers.PopulateError(404, APITypeConstants.NotFound_404, ErrorMessageConstants.NotFound + $" image.");
                return result;
            }
            _context.Images.Remove(image);
            if (await _context.SaveChangesAsync() >= 0) {
                result.Content = true;
                return result;
            }
            result.Error = Helpers.ErrorHelpers.PopulateError(0, APITypeConstants.SaveChangesFailed, ErrorMessageConstants.SaveChangesFailed);
            return result;
        }

        public async Task<Result<bool>> DeleteMultiImageAsync(IList<string> listImageId)
        {
            var result = new Result<bool>();
            var listImage = new List<Core.Entities.Image>();
            foreach (var imageId in listImageId) {
                var image = await _context.Images.FindAsync(imageId);
                if (image is null) {
                    result.Error = Helpers.ErrorHelpers.PopulateError(404, APITypeConstants.NotFound_404, ErrorMessageConstants.NotFound + $" image.");
                    return result;
                }
                listImage.Add(image);
            }
            _context.Images.RemoveRange(listImage);
            if (await _context.SaveChangesAsync() >= 0) {
                result.Content = true;
                return result;
            }
            result.Error = Helpers.ErrorHelpers.PopulateError(0, APITypeConstants.SaveChangesFailed, ErrorMessageConstants.SaveChangesFailed);
            return result;
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

        private void OrderNewImage(ref IQueryable<Core.Entities.Image> query, bool? isNew)
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

        public async Task<Result<ImageGetByIdResponse>> GetImageByIdAsync(string imageId)
        {
            var result = new Result<ImageGetByIdResponse>();
            var image = await _context.Images.FindAsync(imageId);
            if (image is null) {
                result.Error = Helpers.ErrorHelpers.PopulateError(404, APITypeConstants.NotFound_404, ErrorMessageConstants.NotFound + $" image.");
                return result;
            }
            var response = _mapper.Map<ImageGetByIdResponse>(image);
            result.Content = response;
            return result;
        }
    }
}