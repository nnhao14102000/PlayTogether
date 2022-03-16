using System.ComponentModel.DataAnnotations;

namespace PlayTogether.Core.Dtos.Incoming.Business.Hobby
{
    public class HobbyCreateRequest
    {
        [Required]
        [MaxLength(100)]
        public string GameId { get; set; }
    }
}