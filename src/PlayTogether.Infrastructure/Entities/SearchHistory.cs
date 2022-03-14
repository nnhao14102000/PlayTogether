using System.ComponentModel.DataAnnotations;

namespace PlayTogether.Infrastructure.Entities
{
    public class SearchHistory : BaseEntity
    {
        [MaxLength(100)]
        public string UserId { get; set; }
        public AppUser Hirer { get; set; }
        
        public string SearchString { get; set; }
    }
}