﻿namespace PioneerLiganSTHLM.Models
{
    public class Player
    {
        public int ID { get; set; }
        public string Name { get; set; } = String.Empty;
        public int Events { get; set; } = 0;
        public int Points { get; set; } = 0;
        public int Wins { get; set; } = 0;
        public int Losses { get; set; } = 0;
        public int Ties { get; set; } = 0;
    }
}
