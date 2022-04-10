namespace PlayTogether.Core.Parameters
{
    public class GameParameters : QueryStringParameters
    {
        public string Name { get; set; }
        public bool? IsNew { get; set; }
        public bool? IsMostFavorite { get; set; }
    }
}