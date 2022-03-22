using System;
using System.ComponentModel.DataAnnotations;

namespace PlayTogether.Core.Dtos.Incoming.Business.Image
{
    public class ImageCreateRequest
    {
        [Required]
        public string UserId { get; set; }

        [Required]
        public string ImageLink { get; set; }
        
        public DateTime CreatedDate = DateTime.Now;
    }
}