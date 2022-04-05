using Microsoft.ML;
using Microsoft.ML.Data;

namespace PlayTogether.Core.Dtos.Incoming.Business.Recommend
{
    public class RecommendInputRequest
    {
        [LoadColumn(0)]
        public string userId;

        [LoadColumn(1)]
        public float userAge;

        [LoadColumn(2)]
        public bool userGender;

        [LoadColumn(3)]
        public string gameOrderId;

        [LoadColumn(4)]
        public string playerId;

        [LoadColumn(5)]
        public float playerAge;

        [LoadColumn(6)]
        public bool playerGender;

        [LoadColumn(7)]
        public string gameOfPlayerId;

        [LoadColumn(8)]
        public float Label;
    }
}