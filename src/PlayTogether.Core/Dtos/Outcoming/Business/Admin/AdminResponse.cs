using System;

namespace PlayTogether.Core.Dtos.Outcoming.Business.Admin
{
    public class AdminResponse
    {
        public string Firstname { get; set; }

        public string Lastname { get; set; }

        public string Email { get; set; }

        public bool IsActive { get; set; }
        public DateTime? AddedDate { get; set; }
    }
}
