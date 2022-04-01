namespace PlayTogether.Core.Parameters
{
    public class AdminUserParameters : QueryStringParameters
    {
        public string Name { get; set; }
        public bool? IsActive { get; set; }
        public string Status { get; set; }
    }
}