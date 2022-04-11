using System.ComponentModel.DataAnnotations;

namespace PlayTogether.Core.Dtos.Incoming.Business.Hobby
{
    public class HobbyDeleteRequest
    {
        [Required]
        [MaxLength(100)]
        public string HobbyId { get; set; }
    }
}