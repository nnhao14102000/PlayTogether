using System;
using System.ComponentModel.DataAnnotations;

namespace PlayTogether.Core.Dtos.Incoming.Business.GameOfUser
{
    public class GameOfUserUpdateRequest
    {   
        [MaxLength(100)]
        public string RankId { get; set; }

        public DateTime UpdateDate = DateTime.UtcNow.AddHours(7);
    }
}