using System;
using System.ComponentModel.DataAnnotations;

namespace PlayTogether.Core.Dtos.Incoming.Business.GameOfPlayer
{
    public class GameOfPlayerUpdateRequest
    {    
        public string Rank { get; set; }

        public DateTime UpdateDate = DateTime.Now;
    }
}