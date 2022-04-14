using System.ComponentModel.DataAnnotations;

namespace PlayTogether.Core.Entities
{
    public class BehaviorHistory : BaseEntity
    {
        [Required]
        [MaxLength(100)]
        public string BehaviorPointId { get; set; }
        public BehaviorPoint BehaviorPoint { get; set; }
        public string Operation { get; set; }
        public string Type { get; set; }
        public int Num { get; set; }
        public string TypePoint { get; set; }
        public string ReferenceBehaviorId { get; set; }
    }
}
