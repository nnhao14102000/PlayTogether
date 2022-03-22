using PlayTogether.Core.Dtos.Outcoming.Business.Order;

namespace PlayTogether.Infrastructure.Helpers
{
    public static class OrderHelpers
    {
        public static OrderUserResponse PopulateOrderUserResponse(
            string id,
            string name,
            string avatar,
            bool isActive,
            bool IsPlayer,
            string status)
        {
            return new OrderUserResponse{
                Id = id,
                Name = name,
                Avatar = avatar, 
                IsActive = isActive,
                IsPlayer = IsPlayer,
                Status = status
            };
        } 
    }
}