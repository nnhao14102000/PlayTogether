using System;

namespace PlayTogether.Core.Dtos.Outcoming.Business.Hirer
{
    public class HirerResponse
    {
        public string Firstname { get; set; }

        public string Lastname { get; set; }

        public DateTime DateOfBirth { get; set; }

        public string City { get; set; }

        public bool Gender { get; set; }

        public string Email { get; set; }

        public float Balance { get; set; }

        public string Avatar { get; set; }

        public bool IsActive { get; set; }

        public string Status { get; set; }
        public DateTime? AddedDate { get; set; }
    }
}
