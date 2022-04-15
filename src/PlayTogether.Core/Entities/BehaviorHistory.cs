using System.ComponentModel.DataAnnotations;

namespace PlayTogether.Core.Entities
{
    public class BehaviorHistory : BaseEntity
    {
        [Required]
        [MaxLength(100)]
        public string BehaviorPointId { get; set; }
        public BehaviorPoint BehaviorPoint { get; set; }

        [MaxLength(10)]
        public string Operation { get; set; }

        [MaxLength(100)]
        public string Type { get; set; }

        public int Num { get; set; }

        [MaxLength(100)]
        public string TypePoint { get; set; }
        
        [MaxLength(100)]
        public string ReferenceBehaviorId { get; set; }
    }
}
