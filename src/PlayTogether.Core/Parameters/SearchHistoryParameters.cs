namespace PlayTogether.Core.Parameters
{
    public class SearchHistoryParameters : QueryStringParameters
    {
        public bool? SortDsc { get; set; }
        public string Content { get; set; }
    }
}