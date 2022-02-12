using System;

namespace PlayTogether.Core.Dtos.Outcoming.Business.Game
{
    public class GameCreateResponse
    {
        public string Id { get; set; }

        public string Name { get; set; }

        public string DisplayName { get; set; } 

        public string OtherName { get; set; }       
    }
}