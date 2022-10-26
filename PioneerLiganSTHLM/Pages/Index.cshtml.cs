using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Html;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.RazorPages;
using Microsoft.EntityFrameworkCore;
using PioneerLiganSTHLM.Data;
using PioneerLiganSTHLM.HelperClasses;
using PioneerLiganSTHLM.ViewModels;
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

        public List<ViewModels.League> LeagueVMs { get; set; } = new List<ViewModels.League>();
        public List<Models.League> Leagues { get; set; } = new List<Models.League>();

        public void OnGet()
        {
            if (_context.League != null)
            {
                Leagues = _context.League.OrderByDescending(i => i.ID).ToList();

                foreach (var league in Leagues)
                {
                    var events = from e in _context.Event select e;
                    var eventResults = from e in _context.EventResult select e;
                    var players = from p in _context.Player select p;

                    var tempLeague = new ViewModels.League();

                    tempLeague.Events = events.Where(i => i.LeagueID == league.ID).OrderBy(ev => ev.EventNumber).ToList();


                    tempLeague.Players = players.ToList();

                    foreach (var ev in tempLeague.Events)
                    {
                        var tempResults = new List<Models.EventResult>();

                        tempResults.AddRange(eventResults.Where(i => i.EventId == ev.ID).OrderBy(p => p.Placement));
                        tempLeague.Results.AddRange(eventResults.Where(i => i.EventId == ev.ID).OrderBy(p => p.Placement));

                        tempLeague.LeagueEventVMs.Add(new LeagueEvent { Date = ev.Date, LeagueID = ev.LeagueID, EventNumber = ev.EventNumber, Results = tempResults, cssId = "collapse" + ev.ID });
                    }

                    tempLeague.LeagueEventVMs = tempLeague.LeagueEventVMs.OrderBy(d => d.Date).ToList();

                    foreach (var player in tempLeague.Players)
                    {
                        PlayerVM tempPlayer = new PlayerVM(player, tempLeague);                        
                        tempLeague.PlayersVMs.Add(tempPlayer);                        
                    }

                    tempLeague.PlayersVMs = tempLeague.PlayersVMs.OrderByDescending(p => p.DiscountedPoints).ThenByDescending(p => p.FourZero).ThenByDescending(p => p.ThreeZeroOne)
                            .ThenByDescending(p => p.ThreeOne).ThenByDescending(p => p.TuTu).ThenByDescending(p => p.Events).ToList();

                    LeagueVMs.Add(tempLeague);
                }
            }            
        }
    }
}