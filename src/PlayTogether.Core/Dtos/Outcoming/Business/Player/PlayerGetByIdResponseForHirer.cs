using System.Collections.Generic;

namespace PlayTogether.Core.Dtos.Outcoming.Business.Player
{
    public class PlayerGetByIdResponseForHirer
    {
        public string Id { get; set; }

        public string Avatar { get; set; }

        public string Firstname { get; set; }

        public string Lastname { get; set; }

        public ICollection<ImagesOfPlayer> Images { get; set; }

        public string Description { get; set; }

        public string OtherSkill { get; set; }

        public float Rate { get; set; }

        public string Status { get; set; }

        public int MaxHourHire { get; set; }

        public float PricePerHour { get; set; }
    }
}