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
using Microsoft.AspNetCore.Identity;
using System.Security.Claims;
using PlayTogether.Core.Dtos.Outcoming.Business.Order;

namespace PlayTogether.Infrastructure.Repositories.Business.Rating
{
    public class RatingRepository : BaseRepository, IRatingRepository
    {
        private readonly UserManager<IdentityUser> _userManager;

        public RatingRepository(IMapper mapper, AppDbContext context, UserManager<IdentityUser> userManager) : base(mapper, context)
        {
            _userManager = userManager;
        }

        public async Task<Result<bool>> CreateRatingFeedbackAsync(ClaimsPrincipal principal, string orderId, RatingCreateRequest request)
        {
            var result = new Result<bool>();
            var loggedInUser = await _userManager.GetUserAsync(principal);
            if (loggedInUser is null) {
                result.Error = Helpers.ErrorHelpers.PopulateError(400, APITypeConstants.BadRequest_400, ErrorMessageConstants.Unauthenticate);
                return result;
            }
            var identityId = loggedInUser.Id;

            var user = await _context.AppUsers.FirstOrDefaultAsync(x => x.IdentityId == identityId);
            if (user is null) {
                result.Error = Helpers.ErrorHelpers.PopulateError(400, APITypeConstants.BadRequest_400, ErrorMessageConstants.UserNotFound);
                return result;
            }

            var order = await _context.Orders.FindAsync(orderId);

            if (order is null) {
                result.Error = Helpers.ErrorHelpers.PopulateError(404, APITypeConstants.NotFound_404, ErrorMessageConstants.NotFound + $" order này.");
                return result;
            }

            if (order.UserId != user.Id) {
                result.Error = Helpers.ErrorHelpers.PopulateError(400, APITypeConstants.BadRequest_400, ErrorMessageConstants.NotOwnInfo);
                return result;
            }

            if (order.Status is OrderStatusConstants.Start) {
                result.Error = Helpers.ErrorHelpers.PopulateError(400, APITypeConstants.BadRequest_400, "Order chưa kết thúc.");
                return result;
            }

            if (order.Status is OrderStatusConstants.Cancel) {
                result.Error = Helpers.ErrorHelpers.PopulateError(400, APITypeConstants.BadRequest_400, "Order đã bị hủy.");
                return result;
            }

            if (order.Status is OrderStatusConstants.Processing) {
                result.Error = Helpers.ErrorHelpers.PopulateError(400, APITypeConstants.BadRequest_400, "Order đang được xử lí.");
                return result;
            }

            if (order.Status is OrderStatusConstants.Interrupt) {
                result.Error = Helpers.ErrorHelpers.PopulateError(400, APITypeConstants.BadRequest_400, "Order đã bị buộc dừng lại.");
                return result;
            }

            if (order.Status is OrderStatusConstants.Reject) {
                result.Error = Helpers.ErrorHelpers.PopulateError(400, APITypeConstants.BadRequest_400, "Order đã bị từ chối.");
                return result;
            }

            var allowRatingTime = await _context.SystemConfigs.FirstOrDefaultAsync(x => x.NO == 3);
            if (allowRatingTime is null) {
                result.Error = Helpers.ErrorHelpers.PopulateError(404, APITypeConstants.NotFound_404, "Không tìm thấy cấu hình thời gian cho phép đánh giá. Vui lòng thông báo tới quản trị viên. Xin chân thành cảm ơn.");
                return result;
            }

            if (DateTime.UtcNow.AddHours(7).AddHours(-allowRatingTime.Value) > order.TimeFinish
                // || DateTime.UtcNow.AddHours(7).AddMinutes(-ValueConstants.HourActiveFeedbackReportForTest) > order.TimeFinish
                ) {
                result.Error = Helpers.ErrorHelpers.PopulateError(400, APITypeConstants.BadRequest_400, $"Đã hết thời gian cho phép tố cáo. Bạn chỉ được phép đánh giá trong khoản thời gian {(int)allowRatingTime.Value} giờ sau khi kết thúc thuê.");
                return result;
            }

            await _context.Entry(order).Collection(x => x.Ratings).LoadAsync();
            await _context.Entry(order).Collection(x => x.Reports).LoadAsync();
            if (order.Ratings.Count >= 1) {
                result.Error = Helpers.ErrorHelpers.PopulateError(400, APITypeConstants.BadRequest_400, "Bạn đã đánh giá rồi.");
                return result;
            }

            if (order.Reports.Where(x => x.OrderId == order.Id).Any(x => x.UserId == order.UserId)) {
                result.Error = Helpers.ErrorHelpers.PopulateError(400, APITypeConstants.BadRequest_400, "Bạn đã tố cáo người chơi nên bạn không thể đánh giá.");
                return result;
            }


            await _context.Entry(order).Reference(x => x.User).LoadAsync();
            var toUser = await _context.AppUsers.FindAsync(order.ToUserId);
            await _context.Entry(toUser).Reference(x => x.BehaviorPoint).LoadAsync();

            var model = _mapper.Map<Core.Entities.Rating>(request);
            model.OrderId = orderId;
            model.UserId = order.UserId;
            model.ToUserId = order.ToUserId;

            await _context.Ratings.AddAsync(model);

            await _context.Notifications.AddAsync(
                Helpers.NotificationHelpers.PopulateNotification(order.ToUserId, $"{order.User.Name} đã vote cho bạn {request.Rate} ⭐.", $"{order.User.Name}: {request.Comment}", "")
            );
            toUser.NumOfRate += 1;

            await _context.Entry(order).Collection(x => x.GameOfOrders).LoadAsync();
            await _context.Entry(toUser).Collection(x => x.GamesOfUsers).LoadAsync();

            if ((await _context.SaveChangesAsync() < 0)) {
                result.Error = Helpers.ErrorHelpers.PopulateError(0, APITypeConstants.SaveChangesFailed, ErrorMessageConstants.SaveChangesFailed);
                return result;
            }

            if (request.Rate == 5) {
                if (toUser.BehaviorPoint.Point == 100) {

                }
                else if (toUser.BehaviorPoint.Point == 99) {
                    toUser.BehaviorPoint.Point = 100;
                    await _context.BehaviorHistories.AddAsync(Helpers.BehaviorHistoryHelpers.PopulateBehaviorHistory(
                        toUser.BehaviorPoint.Id, BehaviorTypeConstants.Add, BehaviorTypeConstants.RatingHigh, 1, BehaviorTypeConstants.Point, model.Id
                    ));
                }
                else if (toUser.BehaviorPoint.Point == 98) {
                    toUser.BehaviorPoint.Point = 100;
                    await _context.BehaviorHistories.AddAsync(Helpers.BehaviorHistoryHelpers.PopulateBehaviorHistory(
                        toUser.BehaviorPoint.Id, BehaviorTypeConstants.Add, BehaviorTypeConstants.RatingHigh, 2, BehaviorTypeConstants.Point, model.Id
                    ));
                }
                else {
                    toUser.BehaviorPoint.Point += 2;
                    await _context.BehaviorHistories.AddAsync(Helpers.BehaviorHistoryHelpers.PopulateBehaviorHistory(
                        toUser.BehaviorPoint.Id, BehaviorTypeConstants.Add, BehaviorTypeConstants.RatingHigh, 2, BehaviorTypeConstants.Point, model.Id
                    ));
                }

            }
            if (request.Rate == 4) {
                if (toUser.BehaviorPoint.Point == 100) {

                }
                else if (toUser.BehaviorPoint.Point == 99) {
                    toUser.BehaviorPoint.Point = 100;
                    await _context.BehaviorHistories.AddAsync(Helpers.BehaviorHistoryHelpers.PopulateBehaviorHistory(
                        toUser.BehaviorPoint.Id, BehaviorTypeConstants.Add, BehaviorTypeConstants.RatingHigh, 1, BehaviorTypeConstants.Point, model.Id
                    ));
                }
                else {
                    toUser.BehaviorPoint.Point += 1;
                    await _context.BehaviorHistories.AddAsync(Helpers.BehaviorHistoryHelpers.PopulateBehaviorHistory(
                        toUser.BehaviorPoint.Id, BehaviorTypeConstants.Add, BehaviorTypeConstants.RatingHigh, 1, BehaviorTypeConstants.Point, model.Id
                    ));
                }

            }
            if (request.Rate == 3) {

            }
            if (request.Rate == 2) {
                if (toUser.BehaviorPoint.Point == 3) {
                    toUser.BehaviorPoint.Point = 0;
                    await _context.BehaviorHistories.AddAsync(Helpers.BehaviorHistoryHelpers.PopulateBehaviorHistory(
                        toUser.BehaviorPoint.Id, BehaviorTypeConstants.Sub, BehaviorTypeConstants.RatingLow, 3, BehaviorTypeConstants.Point, model.Id
                    ));
                }
                else if (toUser.BehaviorPoint.Point == 2) {
                    toUser.BehaviorPoint.Point = 0;
                    await _context.BehaviorHistories.AddAsync(Helpers.BehaviorHistoryHelpers.PopulateBehaviorHistory(
                        toUser.BehaviorPoint.Id, BehaviorTypeConstants.Sub, BehaviorTypeConstants.RatingLow, 2, BehaviorTypeConstants.Point, model.Id
                    ));
                }
                else if (toUser.BehaviorPoint.Point == 1) {
                    toUser.BehaviorPoint.Point = 0;
                    await _context.BehaviorHistories.AddAsync(Helpers.BehaviorHistoryHelpers.PopulateBehaviorHistory(
                        toUser.BehaviorPoint.Id, BehaviorTypeConstants.Sub, BehaviorTypeConstants.RatingLow, 1, BehaviorTypeConstants.Point, model.Id
                    ));
                }
                else if (toUser.BehaviorPoint.Point == 0) {

                }
                else {
                    toUser.BehaviorPoint.Point -= 3;
                    await _context.BehaviorHistories.AddAsync(Helpers.BehaviorHistoryHelpers.PopulateBehaviorHistory(
                        toUser.BehaviorPoint.Id, BehaviorTypeConstants.Sub, BehaviorTypeConstants.RatingLow, 3, BehaviorTypeConstants.Point, model.Id
                    ));
                }

            }
            if (request.Rate == 1) {
                if (toUser.BehaviorPoint.Point == 5) {
                    toUser.BehaviorPoint.Point = 0;
                    await _context.BehaviorHistories.AddAsync(Helpers.BehaviorHistoryHelpers.PopulateBehaviorHistory(
                        toUser.BehaviorPoint.Id, BehaviorTypeConstants.Sub, BehaviorTypeConstants.RatingLow, 5, BehaviorTypeConstants.Point, model.Id
                    ));
                }
                else if (toUser.BehaviorPoint.Point == 4) {
                    toUser.BehaviorPoint.Point = 0;
                    await _context.BehaviorHistories.AddAsync(Helpers.BehaviorHistoryHelpers.PopulateBehaviorHistory(
                        toUser.BehaviorPoint.Id, BehaviorTypeConstants.Sub, BehaviorTypeConstants.RatingLow, 4, BehaviorTypeConstants.Point, model.Id
                    ));
                }
                else if (toUser.BehaviorPoint.Point == 3) {
                    toUser.BehaviorPoint.Point = 0;
                    await _context.BehaviorHistories.AddAsync(Helpers.BehaviorHistoryHelpers.PopulateBehaviorHistory(
                        toUser.BehaviorPoint.Id, BehaviorTypeConstants.Sub, BehaviorTypeConstants.RatingLow, 3, BehaviorTypeConstants.Point, model.Id
                    ));
                }
                else if (toUser.BehaviorPoint.Point == 2) {
                    toUser.BehaviorPoint.Point = 0;
                    await _context.BehaviorHistories.AddAsync(Helpers.BehaviorHistoryHelpers.PopulateBehaviorHistory(
                        toUser.BehaviorPoint.Id, BehaviorTypeConstants.Sub, BehaviorTypeConstants.RatingLow, 2, BehaviorTypeConstants.Point, model.Id
                    ));
                }
                else if (toUser.BehaviorPoint.Point == 1) {
                    toUser.BehaviorPoint.Point = 0;
                    await _context.BehaviorHistories.AddAsync(Helpers.BehaviorHistoryHelpers.PopulateBehaviorHistory(
                        toUser.BehaviorPoint.Id, BehaviorTypeConstants.Sub, BehaviorTypeConstants.RatingLow, 1, BehaviorTypeConstants.Point, model.Id
                    ));
                }
                else if (toUser.BehaviorPoint.Point == 0) {

                }
                else {
                    toUser.BehaviorPoint.Point -= 5;
                    await _context.BehaviorHistories.AddAsync(Helpers.BehaviorHistoryHelpers.PopulateBehaviorHistory(
                        toUser.BehaviorPoint.Id, BehaviorTypeConstants.Sub, BehaviorTypeConstants.RatingLow, 5, BehaviorTypeConstants.Point, model.Id
                    ));
                }

            }

            if (request.Rate == 5) {
                if (toUser.BehaviorPoint.SatisfiedPoint == 100) {

                }
                else if (toUser.BehaviorPoint.SatisfiedPoint == 99) {
                    toUser.BehaviorPoint.SatisfiedPoint = 100;
                    await _context.BehaviorHistories.AddAsync(Helpers.BehaviorHistoryHelpers.PopulateBehaviorHistory(
                        toUser.BehaviorPoint.Id, BehaviorTypeConstants.Add, BehaviorTypeConstants.RatingHigh, 1, BehaviorTypeConstants.SatisfiedPoint, model.Id
                    ));
                }
                else if (toUser.BehaviorPoint.SatisfiedPoint == 98) {
                    toUser.BehaviorPoint.SatisfiedPoint = 100;
                    await _context.BehaviorHistories.AddAsync(Helpers.BehaviorHistoryHelpers.PopulateBehaviorHistory(
                        toUser.BehaviorPoint.Id, BehaviorTypeConstants.Add, BehaviorTypeConstants.RatingHigh, 2, BehaviorTypeConstants.SatisfiedPoint, model.Id
                    ));
                }
                else if (toUser.BehaviorPoint.SatisfiedPoint == 97) {
                    toUser.BehaviorPoint.SatisfiedPoint = 100;
                    await _context.BehaviorHistories.AddAsync(Helpers.BehaviorHistoryHelpers.PopulateBehaviorHistory(
                        toUser.BehaviorPoint.Id, BehaviorTypeConstants.Add, BehaviorTypeConstants.RatingHigh, 3, BehaviorTypeConstants.SatisfiedPoint, model.Id
                    ));
                }
                else if (toUser.BehaviorPoint.SatisfiedPoint == 96) {
                    toUser.BehaviorPoint.SatisfiedPoint = 100;
                    await _context.BehaviorHistories.AddAsync(Helpers.BehaviorHistoryHelpers.PopulateBehaviorHistory(
                        toUser.BehaviorPoint.Id, BehaviorTypeConstants.Add, BehaviorTypeConstants.RatingHigh, 4, BehaviorTypeConstants.SatisfiedPoint, model.Id
                    ));
                }
                else {
                    toUser.BehaviorPoint.SatisfiedPoint += 4;
                    await _context.BehaviorHistories.AddAsync(Helpers.BehaviorHistoryHelpers.PopulateBehaviorHistory(
                        toUser.BehaviorPoint.Id, BehaviorTypeConstants.Add, BehaviorTypeConstants.RatingHigh, 4, BehaviorTypeConstants.SatisfiedPoint, model.Id
                    ));
                }

            }
            if (request.Rate == 4) {
                if (toUser.BehaviorPoint.SatisfiedPoint == 100) {

                }
                else if (toUser.BehaviorPoint.SatisfiedPoint == 99) {
                    toUser.BehaviorPoint.SatisfiedPoint = 100;
                    await _context.BehaviorHistories.AddAsync(Helpers.BehaviorHistoryHelpers.PopulateBehaviorHistory(
                        toUser.BehaviorPoint.Id, BehaviorTypeConstants.Add, BehaviorTypeConstants.RatingHigh, 1, BehaviorTypeConstants.SatisfiedPoint, model.Id
                    ));
                }
                else if (toUser.BehaviorPoint.SatisfiedPoint == 98) {
                    toUser.BehaviorPoint.SatisfiedPoint = 100;
                    await _context.BehaviorHistories.AddAsync(Helpers.BehaviorHistoryHelpers.PopulateBehaviorHistory(
                        toUser.BehaviorPoint.Id, BehaviorTypeConstants.Add, BehaviorTypeConstants.RatingHigh, 2, BehaviorTypeConstants.SatisfiedPoint, model.Id
                    ));
                }
                else {
                    toUser.BehaviorPoint.SatisfiedPoint += 2;
                    await _context.BehaviorHistories.AddAsync(Helpers.BehaviorHistoryHelpers.PopulateBehaviorHistory(
                        toUser.BehaviorPoint.Id, BehaviorTypeConstants.Add, BehaviorTypeConstants.RatingHigh, 2, BehaviorTypeConstants.SatisfiedPoint, model.Id
                    ));
                }
            }
            if (request.Rate == 3) {

            }
            if (request.Rate == 2) {
                switch (toUser.BehaviorPoint.SatisfiedPoint) {
                    case 6:
                        toUser.BehaviorPoint.SatisfiedPoint = 0;
                        await _context.BehaviorHistories.AddAsync(Helpers.BehaviorHistoryHelpers.PopulateBehaviorHistory(
                            toUser.BehaviorPoint.Id, BehaviorTypeConstants.Sub, BehaviorTypeConstants.RatingLow, 6, BehaviorTypeConstants.SatisfiedPoint, model.Id
                        ));
                        break;
                    case 5:
                        toUser.BehaviorPoint.SatisfiedPoint = 0;
                        await _context.BehaviorHistories.AddAsync(Helpers.BehaviorHistoryHelpers.PopulateBehaviorHistory(
                            toUser.BehaviorPoint.Id, BehaviorTypeConstants.Sub, BehaviorTypeConstants.RatingLow, 5, BehaviorTypeConstants.SatisfiedPoint, model.Id
                        ));
                        break;
                    case 4:
                        toUser.BehaviorPoint.SatisfiedPoint = 0;
                        await _context.BehaviorHistories.AddAsync(Helpers.BehaviorHistoryHelpers.PopulateBehaviorHistory(
                            toUser.BehaviorPoint.Id, BehaviorTypeConstants.Sub, BehaviorTypeConstants.RatingLow, 4, BehaviorTypeConstants.SatisfiedPoint, model.Id
                        ));
                        break;
                    case 3:
                        toUser.BehaviorPoint.SatisfiedPoint = 0;
                        await _context.BehaviorHistories.AddAsync(Helpers.BehaviorHistoryHelpers.PopulateBehaviorHistory(
                            toUser.BehaviorPoint.Id, BehaviorTypeConstants.Sub, BehaviorTypeConstants.RatingLow, 3, BehaviorTypeConstants.SatisfiedPoint, model.Id
                        ));
                        break;
                    case 2:
                        toUser.BehaviorPoint.SatisfiedPoint = 0;
                        await _context.BehaviorHistories.AddAsync(Helpers.BehaviorHistoryHelpers.PopulateBehaviorHistory(
                            toUser.BehaviorPoint.Id, BehaviorTypeConstants.Sub, BehaviorTypeConstants.RatingLow, 2, BehaviorTypeConstants.SatisfiedPoint, model.Id
                        ));
                        break;
                    case 1:
                        toUser.BehaviorPoint.SatisfiedPoint = 0;
                        await _context.BehaviorHistories.AddAsync(Helpers.BehaviorHistoryHelpers.PopulateBehaviorHistory(
                            toUser.BehaviorPoint.Id, BehaviorTypeConstants.Sub, BehaviorTypeConstants.RatingLow, 1, BehaviorTypeConstants.SatisfiedPoint, model.Id
                        ));
                        break;
                    case 0:

                        break;
                    default:
                        toUser.BehaviorPoint.SatisfiedPoint -= 6;
                        await _context.BehaviorHistories.AddAsync(Helpers.BehaviorHistoryHelpers.PopulateBehaviorHistory(
                            toUser.BehaviorPoint.Id, BehaviorTypeConstants.Sub, BehaviorTypeConstants.RatingLow, 6, BehaviorTypeConstants.SatisfiedPoint, model.Id
                        ));
                        break;
                }
            }
            if (request.Rate == 1) {
                switch (toUser.BehaviorPoint.SatisfiedPoint) {
                    case 10:
                        toUser.BehaviorPoint.SatisfiedPoint = 0;
                        await _context.BehaviorHistories.AddAsync(Helpers.BehaviorHistoryHelpers.PopulateBehaviorHistory(
                            toUser.BehaviorPoint.Id, BehaviorTypeConstants.Sub, BehaviorTypeConstants.RatingLow, 10, BehaviorTypeConstants.SatisfiedPoint, model.Id
                        ));
                        break;
                    case 9:
                        toUser.BehaviorPoint.SatisfiedPoint = 0;
                        await _context.BehaviorHistories.AddAsync(Helpers.BehaviorHistoryHelpers.PopulateBehaviorHistory(
                            toUser.BehaviorPoint.Id, BehaviorTypeConstants.Sub, BehaviorTypeConstants.RatingLow, 9, BehaviorTypeConstants.SatisfiedPoint, model.Id
                        ));
                        break;
                    case 8:
                        toUser.BehaviorPoint.SatisfiedPoint = 0;
                        await _context.BehaviorHistories.AddAsync(Helpers.BehaviorHistoryHelpers.PopulateBehaviorHistory(
                            toUser.BehaviorPoint.Id, BehaviorTypeConstants.Sub, BehaviorTypeConstants.RatingLow, 8, BehaviorTypeConstants.SatisfiedPoint, model.Id
                        ));
                        break;
                    case 7:
                        toUser.BehaviorPoint.SatisfiedPoint = 0;
                        await _context.BehaviorHistories.AddAsync(Helpers.BehaviorHistoryHelpers.PopulateBehaviorHistory(
                            toUser.BehaviorPoint.Id, BehaviorTypeConstants.Sub, BehaviorTypeConstants.RatingLow, 7, BehaviorTypeConstants.SatisfiedPoint, model.Id
                        ));
                        break;
                    case 6:
                        toUser.BehaviorPoint.SatisfiedPoint = 0;
                        await _context.BehaviorHistories.AddAsync(Helpers.BehaviorHistoryHelpers.PopulateBehaviorHistory(
                            toUser.BehaviorPoint.Id, BehaviorTypeConstants.Sub, BehaviorTypeConstants.RatingLow, 6, BehaviorTypeConstants.SatisfiedPoint, model.Id
                        ));
                        break;
                    case 5:
                        toUser.BehaviorPoint.SatisfiedPoint = 0;
                        await _context.BehaviorHistories.AddAsync(Helpers.BehaviorHistoryHelpers.PopulateBehaviorHistory(
                            toUser.BehaviorPoint.Id, BehaviorTypeConstants.Sub, BehaviorTypeConstants.RatingLow, 5, BehaviorTypeConstants.SatisfiedPoint, model.Id
                        ));
                        break;
                    case 4:
                        toUser.BehaviorPoint.SatisfiedPoint = 0;
                        await _context.BehaviorHistories.AddAsync(Helpers.BehaviorHistoryHelpers.PopulateBehaviorHistory(
                            toUser.BehaviorPoint.Id, BehaviorTypeConstants.Sub, BehaviorTypeConstants.RatingLow, 4, BehaviorTypeConstants.SatisfiedPoint, model.Id
                        ));
                        break;
                    case 3:
                        toUser.BehaviorPoint.SatisfiedPoint = 0;
                        await _context.BehaviorHistories.AddAsync(Helpers.BehaviorHistoryHelpers.PopulateBehaviorHistory(
                            toUser.BehaviorPoint.Id, BehaviorTypeConstants.Sub, BehaviorTypeConstants.RatingLow, 3, BehaviorTypeConstants.SatisfiedPoint, model.Id
                        ));
                        break;
                    case 2:
                        toUser.BehaviorPoint.SatisfiedPoint = 0;
                        await _context.BehaviorHistories.AddAsync(Helpers.BehaviorHistoryHelpers.PopulateBehaviorHistory(
                            toUser.BehaviorPoint.Id, BehaviorTypeConstants.Sub, BehaviorTypeConstants.RatingLow, 2, BehaviorTypeConstants.SatisfiedPoint, model.Id
                        ));
                        break;
                    case 1:
                        toUser.BehaviorPoint.SatisfiedPoint = 0;
                        await _context.BehaviorHistories.AddAsync(Helpers.BehaviorHistoryHelpers.PopulateBehaviorHistory(
                            toUser.BehaviorPoint.Id, BehaviorTypeConstants.Sub, BehaviorTypeConstants.RatingLow, 1, BehaviorTypeConstants.SatisfiedPoint, model.Id
                        ));
                        break;
                    case 0:

                        break;
                    default:
                        toUser.BehaviorPoint.SatisfiedPoint -= 10;
                        await _context.BehaviorHistories.AddAsync(Helpers.BehaviorHistoryHelpers.PopulateBehaviorHistory(
                            toUser.BehaviorPoint.Id, BehaviorTypeConstants.Sub, BehaviorTypeConstants.RatingLow, 10, BehaviorTypeConstants.SatisfiedPoint, model.Id
                        ));
                        break;
                }
            }


            if ((await _context.SaveChangesAsync() >= 0)) {
                var rateOfPlayer = _context.Ratings.Where(x => x.IsActive == true && x.ToUserId == order.ToUserId).Average(x => x.Rate);

                toUser.Rate = (float)rateOfPlayer;

                if (toUser.NumOfRate != 0) {
                    var date = Math.Ceiling(Helpers.UtilsHelpers.GetDayDiffer(toUser.CreatedDate));
                    var point = (float) (toUser.Rate * toUser.NumOfRate - date);
                    toUser.RankingPoint = point;
                }

                if ((await _context.SaveChangesAsync() < 0)) {
                    result.Error = Helpers.ErrorHelpers.PopulateError(0, APITypeConstants.SaveChangesFailed, ErrorMessageConstants.SaveChangesFailed);
                    return result;
                }

                foreach (var skill in toUser.GamesOfUsers) {
                    foreach (var game in order.GameOfOrders) {
                        await _context.Recommends.AddAsync(
                            Helpers.RecommendHelpers.PopulateRecommend(order.UserId, GetAge(order.User.DateOfBirth), order.User.Gender, game.Id, toUser.Id, GetAge(toUser.DateOfBirth), toUser.Gender, skill.GameId, request.Rate));
                        if (await _context.SaveChangesAsync() < 0) {
                            continue;
                        }
                    }
                }

                result.Content = true;
                return result;
            }
            result.Error = Helpers.ErrorHelpers.PopulateError(0, APITypeConstants.SaveChangesFailed, ErrorMessageConstants.SaveChangesFailed);
            return result;
        }

        public int GetAge(DateTime dob)
        {
            int age = 0;
            age = DateTime.UtcNow.AddHours(7).AddYears(-dob.Year).Year;
            return age;
        }

        public async Task<PagedResult<RatingGetResponse>> GetAllRatingsAsync(string userId, RatingParameters param)
        {
            var result = new PagedResult<RatingGetResponse>();
            var user = await _context.AppUsers.FindAsync(userId);
            if (user is null) {
                result.Error = Helpers.ErrorHelpers.PopulateError(400, APITypeConstants.BadRequest_400, ErrorMessageConstants.UserNotFound);
                return result;
            }

            if (user.IsActive is false) {
                result.Error = Helpers.ErrorHelpers.PopulateError(400, APITypeConstants.BadRequest_400, ErrorMessageConstants.DisableUser);
                return result;
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
            var ratings = await _context.Ratings.ToListAsync();
            var query = ratings.AsQueryable();

            FilterByIsViolateFeedback(ref query, param.IsViolate);
            FilterActiveFeedback(ref query, param.IsActive);
            FilterApproveFeedback(ref query, param.IsApprove);
            OrderByCreatedDate(ref query, param.IsNew);

            ratings = query.ToList();
            foreach (var item in ratings) {
                await _context.Entry(item).Reference(x => x.User).LoadAsync();
            }
            var response = _mapper.Map<List<RatingGetResponse>>(ratings);
            return PagedResult<RatingGetResponse>.ToPagedList(response, param.PageNumber, param.PageSize);
        }

        private void FilterByIsViolateFeedback(ref IQueryable<Core.Entities.Rating> query, bool? isViolate)
        {
            if(!query.Any() || isViolate is null){
                return;
            }
            query = query.Where(x => x.IsViolate == isViolate);
        }

        private void FilterApproveFeedback(ref IQueryable<Core.Entities.Rating> query, bool? isApprove)
        {
            if (!query.Any()) {
                return;
            }
            query = query.Where(x => x.IsApprove == isApprove);
        }

        private void FilterActiveFeedback(ref IQueryable<Core.Entities.Rating> query, bool? isActive)
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

        private void OrderByCreatedDate(ref IQueryable<Core.Entities.Rating> query, bool? isNew)
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

        private void FilterByVote(ref IQueryable<Core.Entities.Rating> query, float vote)
        {
            if (!query.Any() || vote == 0 || vote > 5 || vote < 0) {
                return;
            }

            query = query.Where(x => x.Rate == vote);
        }

        public async Task<Result<bool>> ViolateRatingAsync(ClaimsPrincipal principal, string ratingId, RatingViolateRequest request)
        {
            var result = new Result<bool>();

            var loggedInUser = await _userManager.GetUserAsync(principal);
            if (loggedInUser is null) {
                result.Error = Helpers.ErrorHelpers.PopulateError(400, APITypeConstants.BadRequest_400, ErrorMessageConstants.Unauthenticate);
                return result;
            }
            var identityId = loggedInUser.Id;

            var user = await _context.AppUsers.FirstOrDefaultAsync(x => x.IdentityId == identityId);
            if (user is null) {
                result.Error = Helpers.ErrorHelpers.PopulateError(400, APITypeConstants.BadRequest_400, ErrorMessageConstants.UserNotFound);
                return result;
            }

            var rating = await _context.Ratings.FindAsync(ratingId);
            if (rating is null) {
                result.Error = Helpers.ErrorHelpers.PopulateError(404, APITypeConstants.NotFound_404, ErrorMessageConstants.NotFound + $" đánh giá này.");
                return result;
            }
            if (rating.IsActive is false) {
                result.Error = Helpers.ErrorHelpers.PopulateError(404, APITypeConstants.NotFound_404, ErrorMessageConstants.NotFound + $" order này.");
                return result;
            }

            if (rating.IsApprove is not null) {
                result.Error = Helpers.ErrorHelpers.PopulateError(400, APITypeConstants.BadRequest_400, $"Đánh giá này đã được kiểm duyệt.");
                return result;
            }

            if (rating.ToUserId != user.Id) {
                result.Error = Helpers.ErrorHelpers.PopulateError(400, APITypeConstants.BadRequest_400, $"Đánh giá này không phải là của bạn.");
                return result;
            }

            if (rating.IsViolate is true) {
                rating.IsViolate = false;
                rating.Reason = request.Reason;
            }
            else if (rating.IsViolate is false) {
                rating.IsViolate = true;
                rating.Reason = request.Reason;
            }

            if (await _context.SaveChangesAsync() >= 0) {
                result.Content = true;
                return result;
            }

            result.Error = Helpers.ErrorHelpers.PopulateError(0, APITypeConstants.SaveChangesFailed, ErrorMessageConstants.SaveChangesFailed);
            return result;
        }

        public async Task<Result<bool>> ProcessViolateRatingAsync(string ratingId, ProcessViolateRatingRequest request)
        {
            var result = new Result<bool>();
            var rating = await _context.Ratings.FindAsync(ratingId);
            if (rating is null) {
                result.Error = Helpers.ErrorHelpers.PopulateError(404, APITypeConstants.NotFound_404, ErrorMessageConstants.NotFound + $" đánh giá này.");
                return result;
            }
            if (rating.IsApprove is not null) {
                result.Error = Helpers.ErrorHelpers.PopulateError(400, APITypeConstants.BadRequest_400, $"Đánh giá này đã được kiểm duyệt.");
                return result;
            }

            if (request.IsApprove is false) {
                rating.IsViolate = false;
                rating.IsApprove = false;
            }
            else {
                var toUser = await _context.AppUsers.FindAsync(rating.ToUserId);
                await _context.Entry(toUser).Reference(x => x.BehaviorPoint).LoadAsync();
                rating.IsActive = false;
                rating.IsApprove = true;
                rating.IsViolate = true;
                toUser.NumOfRate -= 1;

                if (rating.Rate == 5) {
                    // toUser.BehaviorPoint.Point -= 2;
                    // if (toUser.BehaviorPoint.Point <= 0) {
                    //     toUser.BehaviorPoint.Point = 0;
                    // }
                    // await _context.BehaviorHistories.AddAsync(Helpers.BehaviorHistoryHelpers.PopulateBehaviorHistory(
                    //     toUser.BehaviorPoint.Id, BehaviorTypeConstants.Sub, BehaviorTypeConstants.RatingViolate, 2, BehaviorTypeConstants.Point, rating.Id
                    // ));
                }
                if (rating.Rate == 4) {
                    // toUser.BehaviorPoint.Point -= 1;
                    // if (toUser.BehaviorPoint.Point <= 0) {
                    //     toUser.BehaviorPoint.Point = 0;
                    // }
                    // await _context.BehaviorHistories.AddAsync(Helpers.BehaviorHistoryHelpers.PopulateBehaviorHistory(
                    //     toUser.BehaviorPoint.Id, BehaviorTypeConstants.Sub, BehaviorTypeConstants.RatingViolate, 1, BehaviorTypeConstants.Point, rating.Id
                    // ));
                }
                if (rating.Rate == 3) {

                }
                if (rating.Rate == 2) {
                    switch (toUser.BehaviorPoint.Point) {
                        case 100:
                            break;
                        case 99:
                            toUser.BehaviorPoint.Point = 100;
                            await _context.BehaviorHistories.AddAsync(Helpers.BehaviorHistoryHelpers.PopulateBehaviorHistory(
                                toUser.BehaviorPoint.Id, BehaviorTypeConstants.Add, BehaviorTypeConstants.RatingViolate, 1, BehaviorTypeConstants.Point, rating.Id
                            ));
                            break;
                        case 98:
                            toUser.BehaviorPoint.Point = 100;
                            await _context.BehaviorHistories.AddAsync(Helpers.BehaviorHistoryHelpers.PopulateBehaviorHistory(
                                toUser.BehaviorPoint.Id, BehaviorTypeConstants.Add, BehaviorTypeConstants.RatingViolate, 2, BehaviorTypeConstants.Point, rating.Id
                            ));
                            break;
                        case 97:
                            toUser.BehaviorPoint.Point = 100;
                            await _context.BehaviorHistories.AddAsync(Helpers.BehaviorHistoryHelpers.PopulateBehaviorHistory(
                                toUser.BehaviorPoint.Id, BehaviorTypeConstants.Add, BehaviorTypeConstants.RatingViolate, 3, BehaviorTypeConstants.Point, rating.Id
                            ));
                            break;
                        default:
                            toUser.BehaviorPoint.Point += 3;
                            await _context.BehaviorHistories.AddAsync(Helpers.BehaviorHistoryHelpers.PopulateBehaviorHistory(
                                toUser.BehaviorPoint.Id, BehaviorTypeConstants.Add, BehaviorTypeConstants.RatingViolate, 3, BehaviorTypeConstants.Point, rating.Id
                            ));
                            break;
                    }
                }
                if (rating.Rate == 1) {
                    switch (toUser.BehaviorPoint.Point) {
                        case 100:
                            break;
                        case 99:
                            toUser.BehaviorPoint.Point = 100;
                            await _context.BehaviorHistories.AddAsync(Helpers.BehaviorHistoryHelpers.PopulateBehaviorHistory(
                                toUser.BehaviorPoint.Id, BehaviorTypeConstants.Add, BehaviorTypeConstants.RatingViolate, 1, BehaviorTypeConstants.Point, rating.Id
                            ));
                            break;
                        case 98:
                            toUser.BehaviorPoint.Point = 100;
                            await _context.BehaviorHistories.AddAsync(Helpers.BehaviorHistoryHelpers.PopulateBehaviorHistory(
                                toUser.BehaviorPoint.Id, BehaviorTypeConstants.Add, BehaviorTypeConstants.RatingViolate, 2, BehaviorTypeConstants.Point, rating.Id
                            ));
                            break;
                        case 97:
                            toUser.BehaviorPoint.Point = 100;
                            await _context.BehaviorHistories.AddAsync(Helpers.BehaviorHistoryHelpers.PopulateBehaviorHistory(
                                toUser.BehaviorPoint.Id, BehaviorTypeConstants.Add, BehaviorTypeConstants.RatingViolate, 3, BehaviorTypeConstants.Point, rating.Id
                            ));
                            break;
                        case 96:
                            toUser.BehaviorPoint.Point = 100;
                            await _context.BehaviorHistories.AddAsync(Helpers.BehaviorHistoryHelpers.PopulateBehaviorHistory(
                                toUser.BehaviorPoint.Id, BehaviorTypeConstants.Add, BehaviorTypeConstants.RatingViolate, 4, BehaviorTypeConstants.Point, rating.Id
                            ));
                            break;
                        case 95:
                            toUser.BehaviorPoint.Point = 100;
                            await _context.BehaviorHistories.AddAsync(Helpers.BehaviorHistoryHelpers.PopulateBehaviorHistory(
                                toUser.BehaviorPoint.Id, BehaviorTypeConstants.Add, BehaviorTypeConstants.RatingViolate, 5, BehaviorTypeConstants.Point, rating.Id
                            ));
                            break;
                        default:
                            toUser.BehaviorPoint.Point += 5;
                            await _context.BehaviorHistories.AddAsync(Helpers.BehaviorHistoryHelpers.PopulateBehaviorHistory(
                                toUser.BehaviorPoint.Id, BehaviorTypeConstants.Add, BehaviorTypeConstants.RatingViolate, 5, BehaviorTypeConstants.Point, rating.Id
                            ));
                            break;
                    }
                }


                if (rating.Rate == 5) {
                    // toUser.BehaviorPoint.SatisfiedPoint -= 4;

                    // if (toUser.BehaviorPoint.SatisfiedPoint <= 0) {
                    //     toUser.BehaviorPoint.SatisfiedPoint = 0;
                    // }
                    // await _context.BehaviorHistories.AddAsync(Helpers.BehaviorHistoryHelpers.PopulateBehaviorHistory(
                    //     toUser.BehaviorPoint.Id, BehaviorTypeConstants.Sub, BehaviorTypeConstants.RatingViolate, 4, BehaviorTypeConstants.SatisfiedPoint, rating.Id
                    // ));
                }
                if (rating.Rate == 4) {
                    // toUser.BehaviorPoint.SatisfiedPoint -= 2;

                    // if (toUser.BehaviorPoint.SatisfiedPoint <= 0) {
                    //     toUser.BehaviorPoint.SatisfiedPoint = 0;
                    // }
                    // await _context.BehaviorHistories.AddAsync(Helpers.BehaviorHistoryHelpers.PopulateBehaviorHistory(
                    //     toUser.BehaviorPoint.Id, BehaviorTypeConstants.Sub, BehaviorTypeConstants.RatingViolate, 2, BehaviorTypeConstants.SatisfiedPoint, rating.Id
                    // ));
                }
                if (rating.Rate == 3) {

                }
                if (rating.Rate == 2) {
                    switch (toUser.BehaviorPoint.SatisfiedPoint) {
                        case 100:
                            break;
                        case 99:
                            toUser.BehaviorPoint.SatisfiedPoint = 100;
                            await _context.BehaviorHistories.AddAsync(Helpers.BehaviorHistoryHelpers.PopulateBehaviorHistory(
                                toUser.BehaviorPoint.Id, BehaviorTypeConstants.Add, BehaviorTypeConstants.RatingViolate, 1, BehaviorTypeConstants.SatisfiedPoint, rating.Id
                            ));
                            break;
                        case 98:
                            toUser.BehaviorPoint.SatisfiedPoint = 100;
                            await _context.BehaviorHistories.AddAsync(Helpers.BehaviorHistoryHelpers.PopulateBehaviorHistory(
                                toUser.BehaviorPoint.Id, BehaviorTypeConstants.Add, BehaviorTypeConstants.RatingViolate, 2, BehaviorTypeConstants.SatisfiedPoint, rating.Id
                            ));
                            break;
                        case 97:
                            toUser.BehaviorPoint.SatisfiedPoint = 100;
                            await _context.BehaviorHistories.AddAsync(Helpers.BehaviorHistoryHelpers.PopulateBehaviorHistory(
                                toUser.BehaviorPoint.Id, BehaviorTypeConstants.Add, BehaviorTypeConstants.RatingViolate, 3, BehaviorTypeConstants.SatisfiedPoint, rating.Id
                            ));
                            break;
                        case 96:
                            toUser.BehaviorPoint.SatisfiedPoint = 100;
                            await _context.BehaviorHistories.AddAsync(Helpers.BehaviorHistoryHelpers.PopulateBehaviorHistory(
                                toUser.BehaviorPoint.Id, BehaviorTypeConstants.Add, BehaviorTypeConstants.RatingViolate, 4, BehaviorTypeConstants.SatisfiedPoint, rating.Id
                            ));
                            break;
                        case 95:
                            toUser.BehaviorPoint.SatisfiedPoint = 100;
                            await _context.BehaviorHistories.AddAsync(Helpers.BehaviorHistoryHelpers.PopulateBehaviorHistory(
                                toUser.BehaviorPoint.Id, BehaviorTypeConstants.Add, BehaviorTypeConstants.RatingViolate, 5, BehaviorTypeConstants.SatisfiedPoint, rating.Id
                            ));
                            break;
                        case 94:
                            toUser.BehaviorPoint.SatisfiedPoint = 100;
                            await _context.BehaviorHistories.AddAsync(Helpers.BehaviorHistoryHelpers.PopulateBehaviorHistory(
                                toUser.BehaviorPoint.Id, BehaviorTypeConstants.Add, BehaviorTypeConstants.RatingViolate, 6, BehaviorTypeConstants.SatisfiedPoint, rating.Id
                            ));
                            break;

                        default:
                            toUser.BehaviorPoint.SatisfiedPoint += 6;
                            await _context.BehaviorHistories.AddAsync(Helpers.BehaviorHistoryHelpers.PopulateBehaviorHistory(
                                toUser.BehaviorPoint.Id, BehaviorTypeConstants.Add, BehaviorTypeConstants.RatingViolate, 6, BehaviorTypeConstants.SatisfiedPoint, rating.Id
                            ));
                            break;
                    }

                }
                if (rating.Rate == 1) {
                    switch (toUser.BehaviorPoint.SatisfiedPoint) {
                        case 100:
                            break;
                        case 99:
                            toUser.BehaviorPoint.SatisfiedPoint = 100;
                            await _context.BehaviorHistories.AddAsync(Helpers.BehaviorHistoryHelpers.PopulateBehaviorHistory(
                                toUser.BehaviorPoint.Id, BehaviorTypeConstants.Add, BehaviorTypeConstants.RatingViolate, 1, BehaviorTypeConstants.SatisfiedPoint, rating.Id
                            ));
                            break;
                        case 98:
                            toUser.BehaviorPoint.SatisfiedPoint = 100;
                            await _context.BehaviorHistories.AddAsync(Helpers.BehaviorHistoryHelpers.PopulateBehaviorHistory(
                                toUser.BehaviorPoint.Id, BehaviorTypeConstants.Add, BehaviorTypeConstants.RatingViolate, 2, BehaviorTypeConstants.SatisfiedPoint, rating.Id
                            ));
                            break;
                        case 97:
                            toUser.BehaviorPoint.SatisfiedPoint = 100;
                            await _context.BehaviorHistories.AddAsync(Helpers.BehaviorHistoryHelpers.PopulateBehaviorHistory(
                                toUser.BehaviorPoint.Id, BehaviorTypeConstants.Add, BehaviorTypeConstants.RatingViolate, 3, BehaviorTypeConstants.SatisfiedPoint, rating.Id
                            ));
                            break;
                        case 96:
                            toUser.BehaviorPoint.SatisfiedPoint = 100;
                            await _context.BehaviorHistories.AddAsync(Helpers.BehaviorHistoryHelpers.PopulateBehaviorHistory(
                                toUser.BehaviorPoint.Id, BehaviorTypeConstants.Add, BehaviorTypeConstants.RatingViolate, 4, BehaviorTypeConstants.SatisfiedPoint, rating.Id
                            ));
                            break;
                        case 95:
                            toUser.BehaviorPoint.SatisfiedPoint = 100;
                            await _context.BehaviorHistories.AddAsync(Helpers.BehaviorHistoryHelpers.PopulateBehaviorHistory(
                                toUser.BehaviorPoint.Id, BehaviorTypeConstants.Add, BehaviorTypeConstants.RatingViolate, 5, BehaviorTypeConstants.SatisfiedPoint, rating.Id
                            ));
                            break;
                        case 94:
                            toUser.BehaviorPoint.SatisfiedPoint = 100;
                            await _context.BehaviorHistories.AddAsync(Helpers.BehaviorHistoryHelpers.PopulateBehaviorHistory(
                                toUser.BehaviorPoint.Id, BehaviorTypeConstants.Add, BehaviorTypeConstants.RatingViolate, 6, BehaviorTypeConstants.SatisfiedPoint, rating.Id
                            ));
                            break;
                        case 93:
                            toUser.BehaviorPoint.SatisfiedPoint = 100;
                            await _context.BehaviorHistories.AddAsync(Helpers.BehaviorHistoryHelpers.PopulateBehaviorHistory(
                                toUser.BehaviorPoint.Id, BehaviorTypeConstants.Add, BehaviorTypeConstants.RatingViolate, 7, BehaviorTypeConstants.SatisfiedPoint, rating.Id
                            ));
                            break;
                        case 92:
                            toUser.BehaviorPoint.SatisfiedPoint = 100;
                            await _context.BehaviorHistories.AddAsync(Helpers.BehaviorHistoryHelpers.PopulateBehaviorHistory(
                                toUser.BehaviorPoint.Id, BehaviorTypeConstants.Add, BehaviorTypeConstants.RatingViolate, 8, BehaviorTypeConstants.SatisfiedPoint, rating.Id
                            ));
                            break;
                        case 91:
                            toUser.BehaviorPoint.SatisfiedPoint = 100;
                            await _context.BehaviorHistories.AddAsync(Helpers.BehaviorHistoryHelpers.PopulateBehaviorHistory(
                                toUser.BehaviorPoint.Id, BehaviorTypeConstants.Add, BehaviorTypeConstants.RatingViolate, 9, BehaviorTypeConstants.SatisfiedPoint, rating.Id
                            ));
                            break;
                        case 90:
                            toUser.BehaviorPoint.SatisfiedPoint = 100;
                            await _context.BehaviorHistories.AddAsync(Helpers.BehaviorHistoryHelpers.PopulateBehaviorHistory(
                                toUser.BehaviorPoint.Id, BehaviorTypeConstants.Add, BehaviorTypeConstants.RatingViolate, 10, BehaviorTypeConstants.SatisfiedPoint, rating.Id
                            ));
                            break;
                        default:
                            toUser.BehaviorPoint.SatisfiedPoint += 10;
                            await _context.BehaviorHistories.AddAsync(Helpers.BehaviorHistoryHelpers.PopulateBehaviorHistory(
                                toUser.BehaviorPoint.Id, BehaviorTypeConstants.Add, BehaviorTypeConstants.RatingViolate, 10, BehaviorTypeConstants.SatisfiedPoint, rating.Id
                            ));
                            break;
                    }
                }

                if ((await _context.SaveChangesAsync() >= 0)) {
                    var rateOfPlayer = _context.Ratings.Where(x => x.IsActive == true && x.ToUserId == rating.ToUserId).Average(x => x.Rate);
                    toUser.Rate = (float)rateOfPlayer;
                    if (toUser.NumOfRate != 0) {
                        var date = Math.Ceiling(Helpers.UtilsHelpers.GetDayDiffer(toUser.CreatedDate));
                        var point = (float) (toUser.Rate * toUser.NumOfRate - date);
                        toUser.RankingPoint = point;
                    }
                }
            }

            if (await _context.SaveChangesAsync() >= 0) {
                result.Content = true;
                return result;
            }
            result.Error = Helpers.ErrorHelpers.PopulateError(0, APITypeConstants.SaveChangesFailed, ErrorMessageConstants.SaveChangesFailed);
            return result;
        }

        public async Task<Result<RatingGetResponse>> GetRatingByIdAsync(string ratingId)
        {
            var result = new Result<RatingGetResponse>();
            var rating = await _context.Ratings.FindAsync(ratingId);
            if (rating is null) {
                result.Error = Helpers.ErrorHelpers.PopulateError(404, APITypeConstants.NotFound_404, ErrorMessageConstants.NotFound + $" đánh giá này.");
                return result;
            }
            await _context.Entry(rating).Reference(x => x.User).LoadAsync();
            var response = _mapper.Map<RatingGetResponse>(rating);
            result.Content = response;
            return result;
        }

        public async Task<Result<RatingGetDetailResponse>> GetRatingByDetailAdminAsync(string ratingId)
        {
            var result = new Result<RatingGetDetailResponse>();
            var rating = await _context.Ratings.FindAsync(ratingId);
            if (rating is null) {
                result.Error = Helpers.ErrorHelpers.PopulateError(404, APITypeConstants.NotFound_404, ErrorMessageConstants.NotFound + $" đánh giá này.");
                return result;
            }
            await _context.Entry(rating).Reference(x => x.User).LoadAsync();
            await _context.Entry(rating).Reference(x => x.Order).LoadAsync();

            await _context.Entry(rating.Order).Reference(x => x.User).LoadAsync();

            await _context.Entry(rating.Order)
                .Collection(x => x.Ratings)
                .LoadAsync();

            await _context.Entry(rating.Order)
                .Collection(x => x.Reports)
                .LoadAsync();

            await _context.Entry(rating.Order)
                    .Collection(x => x.GameOfOrders)
                    .Query()
                    .Include(x => x.Game)
                    .LoadAsync();

            var response = _mapper.Map<RatingGetDetailResponse>(rating);

            var toUser = await _context.AppUsers.FindAsync(rating.Order.ToUserId);

            response.Order.ToUser = new OrderUserResponse {
                Id = toUser.Id,
                Name = toUser.Name,
                Email = toUser.Email,
                Avatar = toUser.Avatar,
                IsActive = toUser.IsActive,
                IsPlayer = toUser.IsPlayer,
                Status = toUser.Status
            };
            result.Content = response;
            return result;
        }
    }
}