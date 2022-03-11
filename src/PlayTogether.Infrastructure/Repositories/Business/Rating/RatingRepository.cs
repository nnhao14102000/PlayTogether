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
            if (order is null || order.Status is not OrderStatusConstants.Finish) {
                return false;
            }

            await _context.Entry(order).Collection(x => x.Ratings).LoadAsync();
            if (order.Ratings.Count >= 1) {
                return false;
            }

            await _context.Entry(order).Reference(x => x.Player).LoadAsync();
            await _context.Entry(order).Reference(x => x.Hirer).LoadAsync();

            var model = _mapper.Map<Entities.Rating>(request);
            model.OrderId = orderId;
            model.PlayerId = order.PlayerId;
            model.HirerId = order.HirerId;
            await _context.Ratings.AddAsync(model);
            order.Status = OrderStatusConstants.Complete;

            await _context.Notifications.AddAsync(
                new Entities.Notification {
                    Id = Guid.NewGuid().ToString(),
                    CreatedDate = DateTime.Now,
                    UpdateDate = null,
                    ReceiverId = order.PlayerId,
                    Title = $"{order.Hirer.Firstname} đã vote cho bạn {request.Rate} ⭐.",
                    Message = $"{order.Hirer.Firstname}: {request.Comment}",
                    Status = NotificationStatusConstants.NotRead
                }
            );


            if ((await _context.SaveChangesAsync() >= 0)) {
                var rateOfPlayer = _context.Ratings.Where(x => x.IsActive == true && x.PlayerId == order.PlayerId).Average(x => x.Rate);

                order.Player.Rate = rateOfPlayer;
                if ((await _context.SaveChangesAsync() < 0)) {
                    return false;
                }
                return true;
            }
            return false;
        }

        public async Task<PagedResult<RatingGetResponse>> GetAllRatingsAsync(string playerId, RatingParameters param)
        {
            var player = await _context.Players.FindAsync(playerId);
            if (player is null || player.IsActive is false) {
                return null;
            }

            var ratings = await _context.Ratings.Where(x => x.PlayerId == playerId).ToListAsync();
            var query = ratings.AsQueryable();

            FilterActiveFeedback(ref query, true);
            FilterByVote(ref query, param.Vote);
            OrderByCreatedDate(ref query, param.IsNew);

            ratings = query.ToList();
            foreach (var item in ratings) {
                await _context.Entry(item).Reference(x => x.Hirer).LoadAsync();
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
                await _context.Entry(item).Reference(x => x.Hirer).LoadAsync();
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

            await _context.Entry(rating).Reference(x => x.Player).LoadAsync();
            rating.IsActive = false;

            if ((await _context.SaveChangesAsync() >= 0)) {
                var rateOfPlayer = _context.Ratings.Where(x => x.IsActive == true && x.PlayerId == rating.PlayerId).Average(x => x.Rate);
                rating.Player.Rate = rateOfPlayer;

                if ((await _context.SaveChangesAsync() < 0)) {
                    return false;
                }
                return true;
            }
            return false;
        }
    }
}