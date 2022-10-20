using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.RazorPages;
using Microsoft.EntityFrameworkCore;
using PioneerLiganSTHLM.Data;
using PioneerLiganSTHLM.HelperClasses;
using System.Linq;

namespace PioneerLiganSTHLM.Pages
{
    [AllowAnonymous]
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
                int id = 0;
                ViewModels.PlayerVM tempPlayer = new ViewModels.PlayerVM
                {                   
                    Name = player.Name,
                    Events = player.Events,
                    Points = 0,
                    PlayerResults = new List<ResultObject>()
                };

                foreach (var ev in Events)
                {
                    var er = Results.Where(i => i.PlayerId == player.ID && i.EventId == ev.ID);

                    if (er.Any())
                    {
                        tempPlayer.PlayerResults.Add(new ResultObject(id, er.First().Points, true, true));
                        tempPlayer.Points += er.First().Points;
                        tempPlayer = AddTieBreakers(tempPlayer, er.First().Points);
                    }
                    else
                    {
                        tempPlayer.PlayerResults.Add(new ResultObject(id, 0, true, false));
                    }
                    id++;
                }
                tempPlayer = CalculatePoints(tempPlayer);
                PlayersVMs.Add(tempPlayer);
                PlayersVMs = PlayersVMs.OrderByDescending(p => p.DiscountedPoints).ThenByDescending(p => p.FourZero).ThenByDescending(p => p.ThreeZeroOne)
                    .ThenByDescending(p => p.ThreeOne).ThenByDescending(p => p.TuTu).ThenByDescending(p => p.Events).ToList();
            }
        }

        private ViewModels.PlayerVM AddTieBreakers(ViewModels.PlayerVM player, int points)
        {
            switch (points)
            {
                case 6:
                    player.TuTu++;
                    break;
                case 9:
                    player.ThreeOne++;
                    break;
                case 10:
                    player.ThreeZeroOne++;
                    break;
                case 12:
                    player.FourZero++;
                    break;
                default:
                    break;
            }

            return player;
        }

        private ViewModels.PlayerVM CalculatePoints(ViewModels.PlayerVM tempPlayer)
        {
            int res;
            List<ResultObject> tmp = tempPlayer.PlayerResults;
            tmp = tmp.OrderBy(p => p.Result).ToList();
            tmp = tmp.Take(6).ToList();
            foreach (var r in tempPlayer.PlayerResults)
            {
                if (tmp.Contains(r))
                {
                    r.CountThis = false;
                }

                if (r.CountThis)
                {
                    tempPlayer.DiscountedPoints += r.Result;
                }
            }

            return tempPlayer;
        }
    }
}