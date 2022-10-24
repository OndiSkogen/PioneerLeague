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

        public List<ViewModels.League> Leagues { get; set; } = new List<ViewModels.League>();
        public List<Models.League> LeagueModels { get; set; } = new List<Models.League>();

        public void OnGet()
        {
            if (_context.League != null)
            {
                LeagueModels = _context.League.OrderByDescending(i => i.ID).ToList();

                foreach (var league in LeagueModels)
                {
                    var tempLeague = new ViewModels.League();
                    tempLeague.LeagueModel = league;

                    var events = from e in _context.Event select e;
                    tempLeague.Events = events.Where(i => i.LeagueID == league.ID).OrderBy(ev => ev.EventNumber).ToList();

                    var eventResults = from e in _context.EventResult select e;

                    var players = from p in _context.Player select p;
                    tempLeague.Players = players.ToList();

                    foreach (var ev in tempLeague.Events)
                    {
                        var tempResults = new List<Models.EventResult>();

                        tempResults.AddRange(eventResults.Where(i => i.EventId == ev.ID).OrderBy(p => p.Placement));
                        tempLeague.Results.AddRange(eventResults.Where(i => i.EventId == ev.ID).OrderBy(p => p.Placement));

                        tempLeague.LeagueEventVMs.Add(new ViewModels.LeagueEvent { Date = ev.Date, LeagueID = ev.LeagueID, EventNumber = ev.EventNumber, Results = tempResults, cssId = "collapse" + ev.ID });
                    }

                    tempLeague.LeagueEventVMs = tempLeague.LeagueEventVMs.OrderBy(d => d.Date).ToList();

                    foreach (var player in tempLeague.Players)
                    {
                        int id = 0;
                        ViewModels.PlayerVM tempPlayer = new ViewModels.PlayerVM
                        {
                            Name = player.Name,
                            Events = player.Events,
                            CurrentLeaguePoints = 0,
                            PlayerResults = new List<ResultObject>(),
                            LifeTimePoints = player.Points,
                            Wins = player.Wins,
                            Ties = player.Ties,
                            Losses = player.Losses,
                            AvgPoints = player.Points / (double)player.Events
                        };

                        foreach (var ev in tempLeague.Events)
                        {
                            var er = tempLeague.Results.Where(i => i.PlayerId == player.ID && i.EventId == ev.ID);

                            if (er.Any())
                            {
                                tempPlayer.PlayerResults.Add(new ResultObject(id, er.First().Points, true, true));
                                tempPlayer.CurrentLeaguePoints += er.First().Points;
                                tempPlayer = AddTieBreakers(tempPlayer, er.First().Points);
                            }
                            else
                            {
                                tempPlayer.PlayerResults.Add(new ResultObject(id, 0, true, false));
                            }
                            id++;
                        }
                        tempPlayer = CalculatePoints(tempPlayer);
                        tempLeague.PlayersVMs.Add(tempPlayer);
                        tempLeague.PlayersVMs = tempLeague.PlayersVMs.OrderByDescending(p => p.DiscountedPoints).ThenByDescending(p => p.FourZero).ThenByDescending(p => p.ThreeZeroOne)
                            .ThenByDescending(p => p.ThreeOne).ThenByDescending(p => p.TuTu).ThenByDescending(p => p.Events).ToList();
                    }

                    Leagues.Add(tempLeague);
                }
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