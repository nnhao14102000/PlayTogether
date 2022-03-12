using System.ComponentModel.DataAnnotations;

namespace PlayTogether.Infrastructure.Entities
{
    public class SearchHistory : BaseEntity
    {
        [MaxLength(100)]
        public string HirerId { get; set; }
        public Hirer Hirer { get; set; }
        
        
        public string SearchString { get; set; }
    }
}