using System.ComponentModel.DataAnnotations;

namespace PlayTogether.Core.Entities
{
    public class SearchHistory : BaseEntity
    {
        [Required]
        [MaxLength(100)]
        public string UserId { get; set; }
        public AppUser User { get; set; }
        
        public string SearchString { get; set; }
        public bool IsActive { get; set; } = true;
    }
}