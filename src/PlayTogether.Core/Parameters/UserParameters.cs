using System;
namespace PlayTogether.Core.Parameters
{
    public class UserParameters : QueryStringParameters
    {
        public string Search { get; set; }
        public bool? Gender { get; set; }
        public string GameId { get; set; }
        public string Status { get; set; }
        public bool? IsOrderByName { get; set; }
        public bool? IsOrderByRating { get; set; }
        public bool? IsOrderByPricing { get; set; }
        public bool? IsRecentOrder { get; set; }
        public bool? IsSkillSameHobbies { get; set; }
        public bool? IsNewAccount { get; set; }
        public bool? IsPlayer { get; set; }
        public string FromHour { get; set; }
        public string ToHour { get; set; }
        public int DayInWeek { get; set; }
        public float? FromPrice { get; set; }
        public float? ToPrice { get; set; }
    }
}