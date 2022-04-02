using System;

namespace PlayTogether.Core.Dtos.Outcoming.Business.Recommend
{
    [Serializable]
    public class RecommendSerialize
    {
        public string UserId { get; set; }
        public int UserAge { get; set; }
        public bool UserGender { get; set; }
        public string GameOrderId { get; set; }
        public string PlayerId { get; set; }
        public int PlayerAge { get; set; }
        public bool PlayerGender { get; set; }
        public string GameOfPlayerId { get; set; }
        public float Rate { get; set; }
    }
}