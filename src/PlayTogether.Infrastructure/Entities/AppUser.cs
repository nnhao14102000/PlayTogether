using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace PlayTogether.Infrastructure.Entities
{
    public class AppUser : BaseEntity
    {
        [MaxLength(100)]
        public string IdentityId { get; set; }

        [MaxLength(50)]
        public string Name { get; set; }

        [Column(TypeName = "date")]
        public DateTime DateOfBirth { get; set; }

        [MaxLength(50)]
        public string City { get; set; }

        public bool Gender { get; set; }

        [MaxLength(50)]
        public string Email { get; set; }

        public string Avatar { get; set; }

        public UserBalance UserBalance { get; set; }
        public BehaviorPoint BehaviorPoint { get; set; }

        public bool IsActive { get; set; } = true;
        public bool IsPlayer { get; set; } = false;

        [MaxLength(50)]
        public string Status { get; set; } = "Online";

        public string Description { get; set; }

        [Column(TypeName = "float")]
        public float PricePerHour { get; set; }

        [Range(1, 5)]
        public int MaxHourHire { get; set; } = 1;

        [Column(TypeName = "float")]
        public float Rate { get; set; }

        public ICollection<Order> Orders { get; set; }
        public ICollection<Rating> Ratings { get; set; }
        public ICollection<Report> Reports { get; set; }
        public ICollection<Donate> Donates { get; set; }
        public IList<GameOfUser> GamesOfUsers { get; set; }
        public ICollection<Image> Images { get; set; }
        public ICollection<SearchHistory> SearchHistories { get; set; }
        public ICollection<SystemFeedback> SystemFeedbacks { get; set; }
        public ICollection<DisableUser> DisableUsers { get; set; }
        public ICollection<Chat> Chats { get; set; }
        public ICollection<Recommend> Recommends { get; set; }
        public ICollection<Dating> Datings { get; set; }
        public ICollection<Ignore> Ignores { get; set; }
        public IList<Hobby> Hobbies { get; set; }
    }
}
