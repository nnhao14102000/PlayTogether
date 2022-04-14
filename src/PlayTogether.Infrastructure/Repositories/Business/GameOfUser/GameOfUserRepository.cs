using System.Collections.Generic;
using AutoMapper;
using PlayTogether.Core.Dtos.Incoming.Business.GameOfUser;
using PlayTogether.Core.Dtos.Outcoming.Business.GameOfUser;
using PlayTogether.Core.Interfaces.Repositories.Business;
using PlayTogether.Infrastructure.Data;
using System.Threading.Tasks;
using System;
using System.Linq;
using Microsoft.EntityFrameworkCore;
using System.Security.Claims;
using Microsoft.AspNetCore.Identity;
using PlayTogether.Core.Dtos.Outcoming.Business.Rank;
using PlayTogether.Core.Dtos.Incoming.Generic;
using PlayTogether.Core.Dtos.Outcoming.Generic;
using PlayTogether.Core.Parameters;

namespace PlayTogether.Infrastructure.Repositories.Business.GameOfUser
{
    public class GameOfUserRepository : BaseRepository, IGameOfUserRepository
    {
        private readonly UserManager<IdentityUser> _userManager;
        public GameOfUserRepository(
            IMapper mapper,
            AppDbContext context,
            UserManager<IdentityUser> userManager) : base(mapper, context)
        {
            _userManager = userManager;
        }

        public async Task<PagedResult<GamesOfUserResponse>> GetAllGameOfUserAsync(string userId, GameOfUserParameters param)
        {
            var result = new PagedResult<GamesOfUserResponse>();
            var user = await _context.AppUsers.FindAsync(userId);
            if (user is null) {
                result.Error = Helpers.ErrorHelpers.PopulateError(404, APITypeConstants.NotFound_404, ErrorMessageConstants.UserNotFound);
                return result;
            }

            if (user.IsActive is false) {
                result.Error = Helpers.ErrorHelpers.PopulateError(400, APITypeConstants.BadRequest_400, ErrorMessageConstants.DisableUser);
                return result;
            }

            var gamesOfPlayer = await _context.GameOfUsers.Where(x => x.UserId == userId)
                                                            .OrderByDescending(x => x.CreatedDate)
                                                            .ToListAsync();

            foreach (var item in gamesOfPlayer) {
                await _context.Entry(item)
                              .Reference(x => x.Game)
                              .Query()
                              .LoadAsync();
            }
            var response = _mapper.Map<List<GamesOfUserResponse>>(gamesOfPlayer);
            foreach (var item in response) {
                if (String.IsNullOrEmpty(item.RankId) || String.IsNullOrWhiteSpace(item.RankId)) {
                    continue;
                }
                var rank = await _context.Ranks.FindAsync(item.RankId);
                item.Rank = _mapper.Map<RankGetByIdResponse>(rank);
            }
            return PagedResult<GamesOfUserResponse>.ToPagedList(response, param.PageNumber, param.PageSize);

        }

        public async Task<Result<GameOfUserGetByIdResponse>> CreateGameOfUserAsync(
            ClaimsPrincipal principal,
            GameOfUserCreateRequest request)
        {
            var result = new Result<GameOfUserGetByIdResponse>();
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

            if (user.IsActive is false) {
                result.Error = Helpers.ErrorHelpers.PopulateError(400, APITypeConstants.BadRequest_400, ErrorMessageConstants.Disable);
                return result;
            }

            if (user.Status is not UserStatusConstants.Online) {
                result.Error = Helpers.ErrorHelpers.PopulateError(400, APITypeConstants.BadRequest_400, ErrorMessageConstants.NotReady + $" Hiện tài khoản bạn đang ở chế độ {user.Status}. Vui lòng thử lại sau khi tất cả các thao tác hoàn tất.");
                return result;
            }

            var game = await _context.Games.FindAsync(request.GameId);
            if (game is null) {
                result.Error = Helpers.ErrorHelpers.PopulateError(404, APITypeConstants.NotFound_404, ErrorMessageConstants.NotFound + $" game.");
                return result;
            }

            var existGame = await _context.GameOfUsers.Where(x => x.UserId == user.Id).AnyAsync(x => x.GameId == request.GameId);
            if (existGame) {
                result.Error = Helpers.ErrorHelpers.PopulateError(400, APITypeConstants.BadRequest_400, ErrorMessageConstants.Exist + $" game này trong danh sách kĩ năng của bạn.");
                return result;
            }

            var model = _mapper.Map<Core.Entities.GameOfUser>(request);
            model.UserId = user.Id;
            if (String.IsNullOrEmpty(request.RankId) || String.IsNullOrWhiteSpace(request.RankId)) {
                model.RankId = "None";
            }
            else {
                var existRank = await _context.Ranks.Where(x => x.GameId == request.GameId).AnyAsync(x => x.Id == request.RankId);
                if (!existRank) {
                    result.Error = Helpers.ErrorHelpers.PopulateError(404, APITypeConstants.NotFound_404, ErrorMessageConstants.NotFound + $" rank.");
                    return result;
                }
            }
            await _context.GameOfUsers.AddAsync(model);
            if (await _context.SaveChangesAsync() >= 0) {
                var response = _mapper.Map<GameOfUserGetByIdResponse>(model);
                if (!String.IsNullOrEmpty(response.RankId) || !String.IsNullOrWhiteSpace(response.RankId)) {
                    var rank = await _context.Ranks.FindAsync(response.RankId);
                    response.Rank = new RankGetByIdResponse {
                        Id = rank.Id,
                        NO = rank.NO,
                        Name = rank.Name
                    };
                }
                result.Content = response;
                return result;
            }
            result.Error = Helpers.ErrorHelpers.PopulateError(0, APITypeConstants.SaveChangesFailed, ErrorMessageConstants.SaveChangesFailed);
            return result;
        }

        public async Task<Result<bool>> DeleteGameOfUserAsync(
            ClaimsPrincipal principal,
            string gameOfUserId)
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

            if (user.IsActive is false) {
                result.Error = Helpers.ErrorHelpers.PopulateError(400, APITypeConstants.BadRequest_400, ErrorMessageConstants.Disable);
                return result;
            }

            if (user.Status is not UserStatusConstants.Online) {
                result.Error = Helpers.ErrorHelpers.PopulateError(400, APITypeConstants.BadRequest_400, ErrorMessageConstants.NotReady + $" Hiện tài khoản bạn đang ở chế độ {user.Status}. Vui lòng thử lại sau khi tất cả các thao tác hoàn tất.");
                return result;
            }

            var gameOfUser = await _context.GameOfUsers.FindAsync(gameOfUserId);
            if (gameOfUser is null) {
                result.Error = Helpers.ErrorHelpers.PopulateError(404, APITypeConstants.NotFound_404, ErrorMessageConstants.NotFound + $" kĩ năng này.");
                return result;
            }

            if (gameOfUser.UserId != user.Id) {
                result.Error = Helpers.ErrorHelpers.PopulateError(400, APITypeConstants.BadRequest_400, ErrorMessageConstants.NotOwnInfo);
                return result;
            }
            _context.GameOfUsers.Remove(gameOfUser);
            if (await _context.SaveChangesAsync() >= 0) {
                result.Content = true;
                return result;
            }
            result.Error = Helpers.ErrorHelpers.PopulateError(0, APITypeConstants.SaveChangesFailed, ErrorMessageConstants.SaveChangesFailed);
            return result;
        }

        public async Task<Result<GameOfUserGetByIdResponse>> GetGameOfUserByIdAsync(string gameOfUserId)
        {
            var result = new Result<GameOfUserGetByIdResponse>();
            var gameOfUser = await _context.GameOfUsers.FindAsync(gameOfUserId);
            if (gameOfUser is null) {
                result.Error = Helpers.ErrorHelpers.PopulateError(404, APITypeConstants.NotFound_404, ErrorMessageConstants.NotFound + $" kĩ năng này.");
                return result;
            }

            await _context.Entry(gameOfUser)
                              .Reference(x => x.Game)
                              .Query()
                              .LoadAsync();
            var response = _mapper.Map<GameOfUserGetByIdResponse>(gameOfUser);

            if (!String.IsNullOrEmpty(response.RankId) || !String.IsNullOrWhiteSpace(response.RankId)) {
                var rank = await _context.Ranks.FindAsync(response.RankId);
                response.Rank = new RankGetByIdResponse {
                    Id = rank.Id,
                    NO = rank.NO,
                    Name = rank.Name
                };
            }
            result.Content = response;
            return result;
        }

        public async Task<Result<bool>> UpdateGameOfUserAsync(
            ClaimsPrincipal principal,
            string gameOfUserId,
            GameOfUserUpdateRequest request)
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

            if (user.IsActive is false) {
                result.Error = Helpers.ErrorHelpers.PopulateError(400, APITypeConstants.BadRequest_400, ErrorMessageConstants.Disable);
                return result;
            }

            if (user.Status is not UserStatusConstants.Online) {
                result.Error = Helpers.ErrorHelpers.PopulateError(400, APITypeConstants.BadRequest_400, ErrorMessageConstants.NotReady + $" Hiện tài khoản bạn đang ở chế độ {user.Status}. Vui lòng thử lại sau khi tất cả các thao tác hoàn tất.");
                return result;
            }

            var gameOfUser = await _context.GameOfUsers.FindAsync(gameOfUserId);
            if (gameOfUser is null) {
                result.Error = Helpers.ErrorHelpers.PopulateError(404, APITypeConstants.NotFound_404, ErrorMessageConstants.NotFound + $" kĩ năng này.");
                return result;
            }

            if (gameOfUser.UserId != user.Id) {
                result.Error = Helpers.ErrorHelpers.PopulateError(400, APITypeConstants.BadRequest_400, ErrorMessageConstants.NotOwnInfo);
                return result;
            }

            var model = _mapper.Map(request, gameOfUser);
            if (String.IsNullOrEmpty(request.RankId) || String.IsNullOrWhiteSpace(request.RankId)) {
                model.RankId = "None";
            }
            else {
                var existRank = await _context.Ranks.Where(x => x.GameId == gameOfUser.GameId).AnyAsync(x => x.Id == request.RankId);
                if (!existRank) {
                    result.Error = Helpers.ErrorHelpers.PopulateError(404, APITypeConstants.NotFound_404, ErrorMessageConstants.NotFound + $" rank.");
                    return result;
                }
            }
            _context.GameOfUsers.Update(model);
            if (await _context.SaveChangesAsync() >= 0) {
                result.Content = true;
                return result;
            }
            result.Error = Helpers.ErrorHelpers.PopulateError(0, APITypeConstants.SaveChangesFailed, ErrorMessageConstants.SaveChangesFailed);
            return result;
        }

        public async Task<Result<bool>> CreateMultiGameOfUserAsync(ClaimsPrincipal principal, List<GameOfUserCreateRequest> requests)
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

            if (user.IsActive is false) {
                result.Error = Helpers.ErrorHelpers.PopulateError(400, APITypeConstants.BadRequest_400, ErrorMessageConstants.Disable);
                return result;
            }

            if (user.Status is not UserStatusConstants.Online) {
                result.Error = Helpers.ErrorHelpers.PopulateError(400, APITypeConstants.BadRequest_400, ErrorMessageConstants.NotReady + $" Hiện tài khoản bạn đang ở chế độ {user.Status}. Vui lòng thử lại sau khi tất cả các thao tác hoàn tất.");
                return result;
            }

            foreach (var request in requests) {
                var game = await _context.Games.FindAsync(request.GameId);
                if (game is null) {
                    result.Error = Helpers.ErrorHelpers.PopulateError(404, APITypeConstants.NotFound_404, ErrorMessageConstants.NotFound + $" game.");
                    return result;
                }

                var existGame = await _context.GameOfUsers.Where(x => x.UserId == user.Id).AnyAsync(x => x.GameId == request.GameId);
                if (existGame) {
                    continue;
                }

                var model = _mapper.Map<Core.Entities.GameOfUser>(request);
                model.UserId = user.Id;

                var ranksOfGame = await _context.Ranks.Where(x => x.GameId == game.Id).ToListAsync();
                if (ranksOfGame.Count() > 0) {
                    var no0Rank = ranksOfGame.FirstOrDefault(x => x.NO == 0);
                    if (no0Rank is null) {
                        model.RankId = "None";
                    }
                    model.RankId = no0Rank.Id;
                }
                else {
                    model.RankId = "None";
                }
                // if (String.IsNullOrEmpty(request.RankId) || String.IsNullOrWhiteSpace(request.RankId)) {
                //     model.RankId = "None";

                // }
                // else {
                //     var existRank = await _context.Ranks.Where(x => x.GameId == request.GameId).AnyAsync(x => x.Id == request.RankId);
                //     if (!existRank) {
                //         result.Error = Helpers.ErrorHelpers.PopulateError(404, APITypeConstants.NotFound_404, ErrorMessageConstants.NotFound + $" rank.");
                //         return result;
                //     }
                // }
                await _context.GameOfUsers.AddAsync(model);
                if (await _context.SaveChangesAsync() < 0) {
                    result.Error = Helpers.ErrorHelpers.PopulateError(0, APITypeConstants.SaveChangesFailed, ErrorMessageConstants.SaveChangesFailed);
                    return result;
                }
            }
            result.Content = true;
            return result;
        }
    }
}