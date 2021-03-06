namespace PlayTogether.Core.Parameters
{
    public class PlayerParameters : QueryStringParameters
    {
        public string SearchString { get; set; }
        public string Name {get; set;}
        public bool? Gender { get; set; }
        public string GameId { get; set; }
        public string MusicId { get; set; }
        public string Status { get; set; }
        public bool? IsOrderByFirstName { get; set; } //true is asc, false is dsc
        public bool? IsOrderByRating { get; set; } // true is from highest rating
        public bool? IsOrderByPricing { get; set; } // true is from lowest pricing
        public bool? IsRecentOrder { get; set; }
    }
}
