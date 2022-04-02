namespace PlayTogether.Core.Parameters
{
    public class SearchHistoryParameters : QueryStringParameters
    {
        public bool? IsNew { get; set; }
        public bool? IsHotSearch { get; set; }
        public string Content { get; set; }
    }
}