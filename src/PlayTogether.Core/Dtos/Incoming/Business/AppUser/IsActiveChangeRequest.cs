using System;
using System.ComponentModel.DataAnnotations;

namespace PlayTogether.Core.Dtos.Incoming.Business.AppUser
{
    public class IsActiveChangeRequest
    {
        [Required]
        public bool IsActive { get; set; }
        public string Note { get; set; }
        public DateTime DateDisable { get; set; }
        public DateTime DateActive { get; set; }
        public int NumDateDisable { get; set; }
        public DateTime CreatedDate = DateTime.UtcNow.AddHours(7);
    }
}