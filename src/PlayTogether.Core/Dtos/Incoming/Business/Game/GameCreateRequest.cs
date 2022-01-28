using System;
using System.ComponentModel.DataAnnotations;

namespace PlayTogether.Core.Dtos.Incoming.Business.Game
{
    public class GameCreateRequest
    {
        [Required]
        [MaxLength(50)]
        public string Name { get; set; }

        [MaxLength(10)]
        public string DisplayName { get; set; }

        public DateTime CreatedDate = DateTime.Now;
    }
}