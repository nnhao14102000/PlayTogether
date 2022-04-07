using Microsoft.ML.Data;

namespace PlayTogether.Core.Dtos.Incoming.Business.Recommend
{
    public class RecommendInputRequest
    {
        [LoadColumn(0)]
        public string userId;
    }
}