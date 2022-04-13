namespace PlayTogether.Infrastructure.Entities
{
    public class BehaviorHistory : BaseEntity
    {
        public string BehaviorPointId { get; set; }
        public BehaviorPoint BehaviorPoint { get; set; }
        public string Operation { get; set; }
        public string Type { get; set; }
        public int Num { get; set; }
        public string TypePoint { get; set; }
        public string ReferenceBehaviorId { get; set; }
    }
}
