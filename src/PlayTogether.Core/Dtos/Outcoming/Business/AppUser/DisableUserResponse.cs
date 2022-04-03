using System;

namespace PlayTogether.Core.Dtos.Outcoming.Business.AppUser
{
    public class DisableUserResponse
    {
        public DateTime DateDisable { get; set; }
        public DateTime DateActive { get; set; }
        public string Note { get; set; }
        public bool IsActive { get; set; }
    }
}