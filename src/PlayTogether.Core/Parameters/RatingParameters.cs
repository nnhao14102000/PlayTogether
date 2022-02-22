namespace PlayTogether.Core.Parameters
{
    public class RatingParameters : QueryStringParameters
    {
        public float Vote { get; set; } = 0;
        public bool? IsNew { get; set; }
    }
}