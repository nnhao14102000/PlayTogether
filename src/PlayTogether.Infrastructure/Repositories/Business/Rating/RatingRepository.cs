using System.ComponentModel;
using AutoMapper;
using Microsoft.EntityFrameworkCore;
using PlayTogether.Core.Dtos.Incoming.Business.Rating;
using PlayTogether.Core.Dtos.Incoming.Generic;
using PlayTogether.Core.Dtos.Outcoming.Business.Rating;
using PlayTogether.Core.Dtos.Outcoming.Generic;
using PlayTogether.Core.Interfaces.Repositories.Business;
using PlayTogether.Core.Parameters;
using PlayTogether.Infrastructure.Data;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace PlayTogether.Infrastructure.Repositories.Business.Rating
{
    public class RatingRepository : BaseRepository, IRatingRepository
    {
        public RatingRepository(IMapper mapper, AppDbContext context) : base(mapper, context)
        {
        }

        public async Task<bool> CreateRatingFeedbackAsync(string orderId, RatingCreateRequest request)
        {
            var order = await _context.Orders.FindAsync(orderId);

            if (order is null
                || order.Status is OrderStatusConstants.Start
                || order.Status is OrderStatusConstants.Cancel
                || order.Status is OrderStatusConstants.Processing
                || order.Status is OrderStatusConstants.Interrupt
                || DateTime.Now.AddHours(-ValueConstants.HourActiveFeedbackReport) > order.TimeFinish) {
                return false;
            }

            await _context.Entry(order).Collection(x => x.Ratings).LoadAsync();
            await _context.Entry(order).Collection(x => x.Reports).LoadAsync();
            if (order.Ratings.Count >= 1) {
                return false;
            }

            if (order.Reports.Where(x => x.OrderId == order.Id).Any(x => x.UserId == order.UserId)) {
                return false;
            }


            await _context.Entry(order).Reference(x => x.User).LoadAsync();
            var toUser = await _context.AppUsers.FindAsync(order.ToUserId);


            var model = _mapper.Map<Entities.Rating>(request);
            model.OrderId = orderId;
            model.UserId = order.UserId;
            model.ToUserId = order.ToUserId;
            await _context.Ratings.AddAsync(model);
            // order.Status = OrderStatusConstants.Complete;

            await _context.Notifications.AddAsync(
                Helpers.NotificationHelpers.PopulateNotification(order.ToUserId, $"{order.User.Name} đã vote cho bạn {request.Rate} ⭐.", $"{order.User.Name}: {request.Comment}", "")
            );


            await _context.Entry(order).Collection(x => x.GameOfOrders).LoadAsync();
            await _context.Entry(toUser).Collection(x => x.GamesOfUsers).LoadAsync();


            if ((await _context.SaveChangesAsync() >= 0)) {
                var rateOfPlayer = _context.Ratings.Where(x => x.IsActive == true && x.ToUserId == order.ToUserId).Average(x => x.Rate);

                toUser.Rate = rateOfPlayer;
                if ((await _context.SaveChangesAsync() < 0)) {
                    return false;
                }

                foreach (var skill in toUser.GamesOfUsers) {
                    foreach (var game in order.GameOfOrders) {
                        await _context.Recommends.AddAsync(
                            Helpers.RecommendHelpers.PopulateRecommend(order.UserId, GetAge(order.User.DateOfBirth), order.User.Gender, game.Id, toUser.Id, GetAge(toUser.DateOfBirth), toUser.Gender, skill.GameId, request.Rate));
                        if(await _context.SaveChangesAsync() < 0){
                            continue;
                        }
                    }
                }

                return true;
            }
            return false;
        }

        public int GetAge(DateTime dob)
        {
            int age = 0;
            age = DateTime.Now.AddYears(-dob.Year).Year;
            return age;
        }

        public async Task<PagedResult<RatingGetResponse>> GetAllRatingsAsync(string userId, RatingParameters param)
        {
            var player = await _context.AppUsers.FindAsync(userId);
            if (player is null || player.IsActive is false) {
                return null;
            }

            var ratings = await _context.Ratings.Where(x => x.ToUserId == userId).ToListAsync();
            var query = ratings.AsQueryable();

            FilterActiveFeedback(ref query, true);
            FilterByVote(ref query, param.Vote);
            OrderByCreatedDate(ref query, param.IsNew);

            ratings = query.ToList();
            foreach (var item in ratings) {
                await _context.Entry(item).Reference(x => x.User).LoadAsync();
            }
            var response = _mapper.Map<List<RatingGetResponse>>(ratings);
            return PagedResult<RatingGetResponse>.ToPagedList(response, param.PageNumber, param.PageSize);
        }

        public async Task<PagedResult<RatingGetResponse>> GetAllViolateRatingsForAdminAsync(RatingParametersAdmin param)
        {
            var ratings = await _context.Ratings.Where(x => x.IsViolate == true).ToListAsync();
            var query = ratings.AsQueryable();

            FilterActiveFeedback(ref query, param.IsActive);
            OrderByCreatedDate(ref query, param.IsNew);

            ratings = query.ToList();
            foreach (var item in ratings) {
                await _context.Entry(item).Reference(x => x.User).LoadAsync();
            }
            var response = _mapper.Map<List<RatingGetResponse>>(ratings);
            return PagedResult<RatingGetResponse>.ToPagedList(response, param.PageNumber, param.PageSize);
        }

        private void FilterActiveFeedback(ref IQueryable<Entities.Rating> query, bool? isActive)
        {
            if (!query.Any() || isActive is null) {
                return;
            }
            if (isActive is true) {
                query = query.Where(x => x.IsActive == true);
            }
            if (isActive is false) {
                query = query.Where(x => x.IsActive == false);
            }
        }

        private void OrderByCreatedDate(ref IQueryable<Entities.Rating> query, bool? isNew)
        {
            if (!query.Any() || isNew is null) {
                return;
            }
            if (isNew is true) {
                query = query.OrderByDescending(x => x.CreatedDate);
            }
            if (isNew is false) {
                query = query.OrderBy(x => x.CreatedDate);
            }
        }

        private void FilterByVote(ref IQueryable<Entities.Rating> query, float vote)
        {
            if (!query.Any() || vote == 0 || vote > 5 || vote < 0) {
                return;
            }

            query = query.Where(x => x.Rate == vote);
        }

        public async Task<bool> ViolateFeedbackAsync(string ratingId)
        {
            var rating = await _context.Ratings.FindAsync(ratingId);
            if (rating is null) {
                return false;
            }

            rating.IsViolate = true;
            return (await _context.SaveChangesAsync() >= 0);
        }

        public async Task<bool> DisableFeedbackAsync(string ratingId)
        {
            var rating = await _context.Ratings.FindAsync(ratingId);
            if (rating is null) {
                return false;
            }

            var toUser = await _context.AppUsers.FindAsync(rating.ToUserId);
            rating.IsActive = false;

            if ((await _context.SaveChangesAsync() >= 0)) {
                var rateOfPlayer = _context.Ratings.Where(x => x.IsActive == true && x.ToUserId == rating.ToUserId).Average(x => x.Rate);
                toUser.Rate = rateOfPlayer;

                if ((await _context.SaveChangesAsync() < 0)) {
                    return false;
                }
                return true;
            }
            return false;
        }
    }
}