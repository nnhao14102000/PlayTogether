using System;

namespace PlayTogether.Core.Dtos.Outcoming.Business.AppUser
{
    public class UserGetByAdminResponse
    {
        public string Id { get; set; }
        public string Name { get; set; }
        public string Email { get; set; }
        public DateTime CreatedDate { get; set; }
        public bool IsActive { get; set; }
        public string Status { get; set; }
    }
}