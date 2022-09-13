using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.RazorPages;
using Microsoft.EntityFrameworkCore;
using PioneerLiganSTHLM.Data;
using System.Linq;

namespace PioneerLiganSTHLM.Pages
{
    public class IndexModel : PageModel
    {
        private readonly PioneerLiganSTHLMContext _context;

        public IndexModel(PioneerLiganSTHLMContext context)
        {
            _context = context;
        }

        [BindProperty]
        public ViewModels.League League { get; set; } = new ViewModels.League();
        public Models.League LeagueModel { get; set; } = new Models.League();
        public List<Models.Event> Events { get; set; } = new List<Models.Event>();
        public List<Models.Player> Players { get; set; } = new List<Models.Player>();
        public List<Models.EventResult> Results { get; set; } = new List<Models.EventResult>();
        public List<ViewModels.PlayerVM> PlayersVMs { get; set; } = new List<ViewModels.PlayerVM>();
        public List<ViewModels.LeagueEvent> LeagueEventVMs { get; set; } = new List<ViewModels.LeagueEvent>();

        public void OnGet()
        {
            if (_context.League != null)
            {
                LeagueModel = _context.League.First();
            }

            var events = from e in _context.Event select e;
            Events = events.Where(i => i.LeagueID == LeagueModel.ID).OrderBy(ev => ev.EventNumber).ToList();

            var eventResults = from e in _context.EventResult select e;

            var players = from p in _context.Player select p;
            Players = players.ToList();

            foreach (var ev in Events)
            {
                var tempResults = new List<Models.EventResult>();

                tempResults.AddRange(eventResults.Where(i => i.EventId == ev.ID).OrderBy(p => p.Placement));
                Results.AddRange(eventResults.Where(i => i.EventId == ev.ID).OrderBy(p => p.Placement));

                LeagueEventVMs.Add(new ViewModels.LeagueEvent { Date = ev.Date, LeagueID = ev.LeagueID, EventNumber = ev.EventNumber, Results = tempResults, cssId = "collapse" + ev.ID });
            }

            foreach (var player in Players)
            {
                ViewModels.PlayerVM tempPlayer = new ViewModels.PlayerVM
                {                   
                    Name = player.Name,
                    Events = player.Events,
                    Points = 0,
                    PlayerResults = new List<string>()
                };

                foreach (var ev in Events)
                {
                    var er = Results.Where(i => i.PlayerId == player.ID && i.EventId == ev.ID);

                    if (er.Any())
                    {
                        tempPlayer.PlayerResults.Add(er.First().Points.ToString());
                        tempPlayer.Points += er.First().Points;
                    }
                    else
                    {
                        tempPlayer.PlayerResults.Add("-");
                    }
                }
                PlayersVMs.Add(tempPlayer);
            }
        }
    }
}