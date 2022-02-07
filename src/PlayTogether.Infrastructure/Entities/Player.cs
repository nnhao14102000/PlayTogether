using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace PlayTogether.Infrastructure.Entities
{
    public class Player : BaseEntity
    {
        public string IdentityId { get; set; }

        [MaxLength(50)]
        public string Firstname { get; set; }

        [MaxLength(50)]
        public string Lastname { get; set; }

        [Column(TypeName = "date")]
        public DateTime DateOfBirth { get; set; }

        [MaxLength(50)]
        public string City { get; set; }

        public bool Gender { get; set; }


        [MaxLength(50)]
        public string Email { get; set; }

        [Column(TypeName = "float")]
        public float Balance { get; set; }

        public string Description { get; set; }

        [Column(TypeName = "float")]
        public float PricePerHour { get; set; }

        [Range(1, 3)]
        public int MaxHourHire { get; set; } = 1;

        [Column(TypeName = "float")]
        public float Rating { get; set; }

        public string Avatar { get; set; }

        public bool IsActive { get; set; } = true;

        [MaxLength(50)]
        public string Status { get; set; } = "Offline";

        public IList<Order> Orders { get; set; }
        public ICollection<Rating> Ratings { get; set; }
        public ICollection<Report> Reports { get; set; }
        public ICollection<Donate> Donates { get; set; }
        public ICollection<PlayerWithdraw> PlayerWithdraws { get; set; }
        public IList<GameOfPlayer> GamesOfPlayers { get; set; }
        public IList<RankInGameOfPlayer> RankInGameOfPlayers { get; set; }
        public IList<MusicOfPlayer> MusicOfPlayers { get; set; }
        public ICollection<Image> Images { get; set; }
    }
}
