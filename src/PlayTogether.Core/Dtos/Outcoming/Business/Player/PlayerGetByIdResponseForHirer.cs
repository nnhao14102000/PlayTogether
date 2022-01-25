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

        public float Rating { get; set; }

        // public ICollection<Rating> Ratings { get; set; }
        
        public string Status { get; set; }

        public float PricePerHour { get; set; }

        // public IList<GameOfPlayer> GamesOfPlayers { get; set; }
        // public IList<MusicOfPlayer> MusicOfPlayers { get; set; }
    }
}