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
            if (order is not null || order.Status is not OrderStatusConstant.Finish) {
                return false;
            }
            await _context.Entry(order).Collection(x => x.Ratings).LoadAsync();
            if (order.Ratings.Count >= 1) {
                return false;
            }

            await _context.Entry(order).Reference(x => x.Player).LoadAsync();

            var model = _mapper.Map<Entities.Rating>(request);
            model.OrderId = orderId;
            model.PlayerId = order.PlayerId;
            model.HirerId = order.HirerId;
            await _context.Ratings.AddAsync(model);
            order.Status = OrderStatusConstant.Complete;

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

            var feedbacks = await _context.Ratings.ToListAsync();
            var query = feedbacks.AsQueryable();

            FilterByVote(ref query, param.Vote);
            OrderByCreatedDate(ref query, param.IsNew);
            
            feedbacks = query.ToList();
            var response = _mapper.Map<List<RatingGetResponse>>(feedbacks);
            return PagedResult<RatingGetResponse>.ToPagedList(response, param.PageNumber, param.PageSize);
        }

        private void OrderByCreatedDate(ref IQueryable<Entities.Rating> query, bool? isNew)
        {
            if(!query.Any() || isNew is null){
                return;
            }
            if(isNew is true){
                query = query.OrderByDescending(x => x.CreatedDate);
            }
            if(isNew is false){
                query = query.OrderBy(x => x.CreatedDate);
            }
        }

        private void FilterByVote(ref IQueryable<Entities.Rating> query, float vote)
        {
            if(!query.Any() || vote == 0 || vote > 5 || vote < 0){
                return;
            }

            query = query.Where(x => x.Rate == vote);
        }
    }
}