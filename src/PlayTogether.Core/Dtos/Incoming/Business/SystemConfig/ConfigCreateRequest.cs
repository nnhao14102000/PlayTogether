using System.ComponentModel.DataAnnotations;

namespace PlayTogether.Core.Dtos.Incoming.Business.SystemConfig
{
    public class ConfigCreateRequest
    {
        [Required]
        [MaxLength(100)]
        public string Title { get; set; }

        [Required]
        [Range(0, float.MaxValue)]
        public float Value { get; set; }
    }
}