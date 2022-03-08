namespace PlayTogether.Infrastructure.Entities
{
    public class FavoriteSearch : BaseEntity
    {
        public string HirerId { get; set; }
        public Hirer Hirer { get; set; }
        
        public string SearchString { get; set; }
    }
}