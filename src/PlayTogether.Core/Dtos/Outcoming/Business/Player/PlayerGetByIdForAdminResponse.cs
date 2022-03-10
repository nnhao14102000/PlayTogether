using System;

namespace PlayTogether.Core.Dtos.Outcoming.Business.Player
{
    public class PlayerGetByIdForAdminResponse
    {
        public string Id { get; set; }

        public string Avatar { get; set; }

        public string Email { get; set; }

        public string Firstname { get; set; }

        public string Lastname { get; set; }

        public bool Gender { get; set; }

        public string City { get; set; }

        public DateTime DateOfBirth { get; set; }

        public string Description { get; set; }

        public string OtherSkill { get; set; }

        public string Status { get; set; }

        public bool IsActive { get; set; }
    }
}