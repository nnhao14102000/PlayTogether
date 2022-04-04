using System;
using System.Collections.Generic;

namespace PlayTogether.Core.Dtos.Outcoming.Business.AppUser
{
    public class PersonalInfoResponse
    {
        public string Id { get; set; }
        public string Avatar { get; set; }
        public string Name { get; set; }
        public bool IsPlayer { get; set; } = false;
        public DateTime DateOfBirth { get; set; }
        public string City { get; set; }
        public bool Gender { get; set; }
        public string Email { get; set; }
        public ICollection<ImageUserResponse> Images { get; set; }
        public string Description { get; set; }
        public float Rate { get; set; }
        public int NumOfRate { get; set; }
        public int NumOfOrder { get; set; }
        public int TotalTimeOrder { get; set; }
        public int NumOfFinishOnTime { get; set; }
        public UserBalanceResponse UserBalance { get; set; }
        public BehaviorPointResponse BehaviorPoint { get; set; }
        public string Status { get; set; }
        public bool IsActive { get; set; } = true;
    }
}