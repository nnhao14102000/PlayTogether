namespace PlayTogether.Core.Dtos.Outcoming.Business.Order
{
    public class PlayerOrderResponse
    {
        public string Id { get; set; }
        public string Firstname { get; set; }
        public string Lastname { get; set; }
        public string Avatar { get; set; }
        public bool IsActive { get; set; }
        public string Status { get; set; }
    }
}