namespace PlayTogether.Core.Dtos.Outcoming.Business.Order
{
    public class OrderUserResponse
    {
        public string Id { get; set; }
        public string Name { get; set; }
        public string Avatar { get; set; }
        public bool IsActive { get; set; }
        public bool IsPlayer { get; set; }
        public string Status { get; set; }
    }
}