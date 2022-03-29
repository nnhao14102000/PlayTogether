using System;
using PlayTogether.Infrastructure.Entities;

namespace PlayTogether.Infrastructure.Helpers
{
    public static class RecommendHelpers
    {
        public static Recommend PopulateRecommend(string userId, int uAge, bool uGender, string gameId, string playerId, int pAge, bool pGender, string skillId, float rate){
            return new Recommend{
                Id = Guid.NewGuid().ToString(),
                CreatedDate = DateTime.UtcNow.AddHours(7),
                UserId = userId,
                UserAge = uAge,
                UserGender = uGender, 
                GameOrderId = gameId,
                PlayerId = playerId,
                PlayerAge = pAge,
                PlayerGender = pGender,
                GameOfPlayerId = skillId,
                Rate = rate
            };
        }
    }
}