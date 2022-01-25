namespace PlayTogether.Core.Dtos.Outcoming.Business.Player
{
    public class PlayerServiceInfoResponseForPlayer
    {
        public string Id { get; set; }

        public string Status { get; set; }

        public int MaxHourHire { get; set; }

        public float PricePerHour { get; set; }
    }
}