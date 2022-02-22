using System;
namespace PlayTogether.Core.Dtos.Outcoming.Business.Rating
{
    public class RatingGetResponse
    {
        public string Id { get; set; }
        public HirerRateResponse Hirer { get; set; }
        public float Rate { get; set; }
        public DateTime CreatedDate { get; set; }
        public string Comment { get; set; }
    }
}