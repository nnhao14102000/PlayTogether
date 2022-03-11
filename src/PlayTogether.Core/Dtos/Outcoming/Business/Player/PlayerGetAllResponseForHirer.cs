using System;
using System.Collections.Generic;

namespace PlayTogether.Core.Dtos.Outcoming.Business.Player
{
    public class PlayerGetAllResponseForHirer
    {
        public string Id { get; set; }

        public string Avatar { get; set; }
        
        public string Firstname { get; set; }

        public string Lastname { get; set; }
        public float PricePerHour { get; set; }
        public float Rate { get; set; }
        public string Status { get; set; }
    }
}
