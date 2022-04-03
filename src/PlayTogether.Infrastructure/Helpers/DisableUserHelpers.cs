using System;
using PlayTogether.Infrastructure.Entities;

namespace PlayTogether.Infrastructure.Helpers
{
    public static class DisableUserHelpers
    {
        public static DisableUser PopulateDisableUser(string userId, DateTime dateDisable, DateTime dateActive, string note, int numDateDisable){

            return new DisableUser{
                Id = Guid.NewGuid().ToString(),
                UserId = userId,
                DateDisable = dateDisable,
                DateActive = dateActive,
                Note = note,
                NumberDateDisable = numDateDisable,
                IsActive = true,
                CreatedDate = DateTime.UtcNow.AddHours(7)
            };
        }
    }
}