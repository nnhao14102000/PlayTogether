namespace PlayTogether.Core.Parameters
{
    public class RatingParametersAdmin : QueryStringParameters
    {
        public bool? IsNew { get; set; }
        public bool? IsActive {get; set; }
    }
}