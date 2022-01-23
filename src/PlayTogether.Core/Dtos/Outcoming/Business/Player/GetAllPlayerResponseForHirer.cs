using System;
using System.Collections.Generic;

namespace PlayTogether.Core.Dtos.Outcoming.Business.Player
{
    public class GetAllPlayerResponseForHirer
    {
        public string Id { get; set; }

        public string Avatar { get; set; }
        
        public string Firstname { get; set; }

        public string Lastname { get; set; }

        // public IList<GameOfPlayer> GamesOfPlayers { get; set; }
        // public IList<MusicOfPlayer> MusicOfPlayers { get; set; }
        public float PricePerHour { get; set; }
        public float Rating { get; set; }
        public string Status { get; set; }
    }
}
