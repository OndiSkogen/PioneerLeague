namespace PioneerLiganSTHLM.ViewModels
{
    public class League
    {
        public string Name { get; set; } = "";
        public List<Models.Event> Events { get; set; } = new List<Models.Event>();
        public List<Models.Player> Players { get; set; } = new List<Models.Player>();
        public List<Models.EventResult> Results { get; set; } = new List<Models.EventResult>();
        public List<PlayerVM> PlayersVMs { get; set; } = new List<ViewModels.PlayerVM>();
        public List<LeagueEvent> LeagueEventVMs { get; set; } = new List<ViewModels.LeagueEvent>();
    }
}
