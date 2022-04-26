using System;

namespace PlayTogether.Core.Dtos.Outcoming.Business.AppUser
{
    public class UserSearchResponse
    {
        public string Id { get; set; }
        public string Avatar { get; set; }
        public string Name { get; set; }
        public bool IsPlayer { get; set; }
        public float Rate { get; set; }
        public int NumOfRate { get; set; }
        public string Status { get; set; }
        public float PricePerHour { get; set; }
        public float RankingPoint { get; set; }
        public DateTime CreatedDate { get; set; }
    }
}