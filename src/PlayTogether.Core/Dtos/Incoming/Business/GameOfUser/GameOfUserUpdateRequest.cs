using System;

namespace PlayTogether.Core.Dtos.Incoming.Business.GameOfUser
{
    public class GameOfUserUpdateRequest
    {    
        public string RankId { get; set; }

        public DateTime UpdateDate = DateTime.Now;
    }
}