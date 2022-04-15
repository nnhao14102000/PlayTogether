using System.ComponentModel.DataAnnotations;

namespace PlayTogether.Core.Entities
{
    public class Image : BaseEntity
    {
        public string ImageLink { get; set; }

        [MaxLength(100)]
        [Required]
        public string UserId { get; set; }
        public AppUser User { get; set; }
    }
}