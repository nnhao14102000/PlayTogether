using System;

namespace PlayTogether.Core.Entities
{
    public class DisableUser : BaseEntity
    {
        public string UserId { get; set; }
        public AppUser User { get; set; }

        public DateTime DateDisable { get; set; }
        public DateTime DateActive { get; set; }

        public string Note { get; set; }
        public int NumberDateDisable { get; set; }

        public bool IsActive { get; set; }

    }
}