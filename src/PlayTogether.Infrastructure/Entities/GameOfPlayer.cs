﻿using System.Collections.Generic;

namespace PlayTogether.Infrastructure.Entities
{
    public class GameOfPlayer : BaseEntity
    {
        public string GameId { get; set; }
        public Game Game { get; set; }

        public RankInGameOfPlayer RankInGameOfPlayer { get; set; }

        public string PlayerId { get; set; }
        public Player Player { get; set; }
    }
}
