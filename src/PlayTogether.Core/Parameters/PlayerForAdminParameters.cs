namespace PlayTogether.Core.Parameters
{
    public class PlayerForAdminParameters : QueryStringParameters
    {
        public string Name { get; set; }
        public string Status { get; set; }
        public bool? IsActive { get; set; }
    }
}