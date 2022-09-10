namespace PioneerLiganSTHLM.Models
{
    public class League
    {
        public int ID { get; set; }
        public string Name { get; set; } = string.Empty;
        public string Winner { get; set; } = string.Empty;
        public string GroupStageWinner { get; set; } = string.Empty;
    }
}
