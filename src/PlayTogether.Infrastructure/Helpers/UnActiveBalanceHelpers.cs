using System;
using PlayTogether.Infrastructure.Entities;

namespace PlayTogether.Infrastructure.Helpers
{
    public static class UnActiveBalanceHelpers
    {
        public static UnActiveBalance PopulateUnActiveBalance (string userBalanceId, string OrderId, float money, DateTime dateActive){
            return new UnActiveBalance{
                Id = Guid.NewGuid().ToString(),
                CreatedDate = DateTime.UtcNow.AddHours(7),
                UserBalanceId = userBalanceId,
                OrderId = OrderId,
                Money = money,
                DateActive = dateActive,
                IsActive = false                
            };
        }
    }
}