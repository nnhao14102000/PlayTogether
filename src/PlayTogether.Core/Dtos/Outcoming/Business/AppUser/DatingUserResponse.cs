namespace PlayTogether.Core.Dtos.Outcoming.Business.AppUser
{
    public class DatingUserResponse
    {
        public string Id { get; set; }
        public int FromHour { get; set; }
        public int ToHour { get; set; }

        public bool IsMON { get; set; }
        public bool IsTUE { get; set; }
        public bool IsWED { get; set; }
        public bool IsTHU { get; set; }
        public bool IsFRI { get; set; }
        public bool IsSAT { get; set; }
        public bool IsSUN { get; set; }
    }
}