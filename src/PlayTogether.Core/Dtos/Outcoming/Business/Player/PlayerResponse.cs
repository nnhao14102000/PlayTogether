using System;

namespace PlayTogether.Core.Dtos.Outcoming.Business.Player
{
    public class PlayerResponse
    {
        public string Firstname { get; set; }

        public string Lastname { get; set; }

        public DateTime DateOfBirth { get; set; }

        public string City { get; set; }

        public bool Gender { get; set; }

        public string Email { get; set; }

        public float Balance { get; set; }

        public string Description { get; set; }

        public float PricePerHour { get; set; }

        public float Rating { get; set; }

        public string Avatar { get; set; }

        public bool IsActive { get; set; } = true;

        public string Status { get; set; }

        public DateTime? AddedDate { get; set; }

    }
}
