namespace PlayTogether.Core.Parameters
{
    public class UserParameters : QueryStringParameters
    {
        public string Search { get; set; }
        public string Name {get; set;}
        public bool? Gender { get; set; }
        public string GameId { get; set; }
        public string Status { get; set; }
        public bool? IsOrderByName { get; set; }
        public bool? IsOrderByRating { get; set; }
        public bool? IsOrderByPricing { get; set; }
        public bool? IsRecentOrder { get; set; }
        public bool? IsSameHobbies { get; set; }
        public bool? IsNewAccount { get; set; }
        public bool? IsPlayer { get; set; }
        public string FromHour { get; set; }
        public string ToHour { get; set; }
        public string Date { get; set; }
    }
}