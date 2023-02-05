namespace PioneerLiganSTHLM.ViewModels
{
    public class League
    {
        public string Name { get; set; } = string.Empty;
        public List<Models.Event> Events { get; set; } = new List<Models.Event>();
        public List<Models.Player> Players { get; set; } = new List<Models.Player>();
        public List<Models.EventResult> Results { get; set; } = new List<Models.EventResult>();
        public List<PlayerVM> PlayersVMs { get; set; } = new List<PlayerVM>();
        public List<LeagueEvent> LeagueEventVMs { get; set; } = new List<LeagueEvent>();
        public string GroupStageWinner { get; set; } = string.Empty;
        public string Winner { get; set; } = string.Empty;

        public League(List<Models.Event> events, List<Models.Player> players, List<Models.EventResult> eventResults, Models.League league)
        {
            Name = league.Name;
            GroupStageWinner = league.GroupStageWinner;
            Winner = league.Winner;
            Events = events.Where(i => i.LeagueID == league.ID).OrderBy(ev => ev.EventNumber).ToList();
            Players = players.ToList();

            foreach (var ev in Events)
            {
                var tempResults = new List<Models.EventResult>();

                tempResults.AddRange(eventResults.Where(i => i.EventId == ev.ID).OrderBy(p => p.Placement));
                Results.AddRange(eventResults.Where(i => i.EventId == ev.ID).OrderBy(p => p.Placement));

                LeagueEventVMs.Add(new LeagueEvent { Date = ev.Date, LeagueID = ev.LeagueID, EventNumber = ev.EventNumber, Results = tempResults, cssId = "collapse" + ev.ID });
            }

            LeagueEventVMs = LeagueEventVMs.OrderBy(d => d.Date).ToList();
        }
    }
}
