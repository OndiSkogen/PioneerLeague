namespace PioneerLiganSTHLM.ViewModels
{
    public class LeagueEvent
    {
        public DateTime Date { get; set; }
        public int LeagueID { get; set; }
        public int EventNumber { get; set; }
        public List<Models.EventResult> Results { get; set; } = new List<Models.EventResult>();
    }
}
