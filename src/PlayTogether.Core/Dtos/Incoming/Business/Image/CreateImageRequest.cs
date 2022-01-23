using System;

namespace PlayTogether.Core.Dtos.Incoming.Business.Image
{
    public class CreateImageRequest
    {
        public string PlayerId { get; set; }
        public string ImageLink { get; set; }
        public DateTime CreatedDate = DateTime.Now;
    }
}