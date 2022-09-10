namespace PioneerLiganSTHLM.ViewModels
{
    public class PlayerVM
    {
        public string Name { get; set; } = String.Empty;
        public int Events { get; set; } = 0;
        public int Points { get; set; } = 0;
        public List<string> PlayerResults { get; set; } = new List<string>();
        //public Dictionary<int, string> PlayerResults { get; set; } = new Dictionary<int, string>();
    }
}
