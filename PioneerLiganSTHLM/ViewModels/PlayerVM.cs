using PioneerLiganSTHLM.HelperClasses;

namespace PioneerLiganSTHLM.ViewModels
{
    public class PlayerVM
    {
        public string Name { get; set; } = String.Empty;
        public int Events { get; set; } = 0;
        public int Points { get; set; } = 0;
        public int DiscountedPoints { get; set; } = 0;
        public List<ResultObject> PlayerResults { get; set; } = new List<ResultObject>();
        public int Wins { get; set; } = 0;
        public int Losses { get; set; } = 0;
        public int Ties { get; set; } = 0;
        public int FourZero { get; set; } = 0;
        public int ThreeZeroOne { get; set; } = 0;
        public int ThreeOne { get; set; } = 0;
        public int TuTu { get; set; } = 0;
    }
}
