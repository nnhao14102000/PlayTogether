using System;

namespace PlayTogether.Core.Dtos.Incoming.Business.GameType
{
    public class GameTypeUpdateRequest
    {
        public string TypeName { get; set; }
        public DateTime UpdateDate = DateTime.Now;
    }
}