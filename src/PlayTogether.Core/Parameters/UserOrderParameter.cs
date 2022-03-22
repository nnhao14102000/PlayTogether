namespace PlayTogether.Core.Parameters
{
    public class UserOrderParameter : QueryStringParameters
    {
        public string Status { get; set; }
        public bool? IsNew { get; set; }
    }
}