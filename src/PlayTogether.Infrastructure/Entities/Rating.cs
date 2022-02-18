namespace PlayTogether.Infrastructure.Entities
{
    public class Rating : BaseEntity
    {
        public string OrderId { get; set; }
        public Order Order { get; set; }

        public Player Player { get; set; }
        public string PlayerId { get; set; }

        public Hirer Hirer { get; set; }
        public string HirerId { get; set; }

        public string Comment { get; set; }
        public float Rate { get; set; }
        public bool IsViolate { get; set; }
        public bool IsActive { get; set; }
    }
}
