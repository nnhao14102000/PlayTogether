using System.ComponentModel.DataAnnotations;

namespace PlayTogether.Infrastructure.Entities
{
    public class Image : BaseEntity
    {
        public string ImageLink { get; set; }

        [MaxLength(100)]
        public string PlayerId { get; set; }
        public Player Player { get; set; }
    }
}