namespace PlayTogether.Core.Parameters
{
    public class NotificationParameters : QueryStringParameters
    {
        public bool? IsNew { get; set; }
        public bool? IsRead { get; set; }
    }
}