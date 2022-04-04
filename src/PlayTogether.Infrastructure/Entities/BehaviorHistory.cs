namespace PlayTogether.Infrastructure.Entities
{
    public class BehaviorHistory : BaseEntity
    {
        public string BehaviorPointId { get; set; }
        public BehaviorPoint BehaviorPoint { get; set; }
        public string Operation { get; set; }
        public string Type { get; set; } // Order, Rating, Report
        public string ReferenceBehaviorId { get; set; } // Base on Type, have difference Id
    }
}
