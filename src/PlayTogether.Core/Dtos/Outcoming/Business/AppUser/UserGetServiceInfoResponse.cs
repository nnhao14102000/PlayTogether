namespace PlayTogether.Core.Dtos.Outcoming.Business.AppUser
{
    public class UserGetServiceInfoResponse
    {
        public bool IsPlayer { get; set; }
        public float PricePerHour { get; set; }
        public int MaxHourHire { get; set; }
    }
}