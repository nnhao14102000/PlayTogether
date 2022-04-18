using System.ComponentModel.DataAnnotations;

namespace PlayTogether.Core.Dtos.Incoming.Business.SystemConfig
{
    public class ConfigUpdateRequest
    {
        [Required]
        [Range(0, float.MaxValue)]
        public float Value { get; set; }
    }
}