using System;

namespace PlayTogether.Core.Dtos.Incoming.Business.GameType
{
    public class GameTypeCreateRequest
    {
        public string TypeName { get; set; }
        public DateTime CreatedDate = DateTime.Now;
    }
}