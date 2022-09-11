using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.RazorPages;
using Microsoft.AspNetCore.Mvc.Rendering;
using PioneerLiganSTHLM.Data;
using PioneerLiganSTHLM.Models;

namespace PioneerLiganSTHLM.Pages.Event
{
    public class CreateModel : PageModel
    {
        private readonly PioneerLiganSTHLMContext _context;

        public CreateModel(PioneerLiganSTHLMContext context)
        {
            _context = context;
        }

        public IActionResult OnGet(string? selectedId)
        {
            var leagues = from l in _context.League select l;
            Leagues = leagues.OrderBy(l => l.ID).ToList();
            var events = from e in _context.Event select e;
            Events = events.OrderBy(l => l.ID).ToList();
            var players = from p in _context.Player select p;
            Players = players.ToList();

            if (selectedId != null)
            {
                SelectedLeague = int.Parse(selectedId);
                DisplayEvents = Events.Where(i => i.LeagueID == SelectedLeague).ToList();
            }
            else
            {
                SelectedLeague = 0;
            }

            return Page();
        }

        [BindProperty]
        public Models.Event Event { get; set; } = default!;
        public List<Models.Event> Events { get; set; } = new List<Models.Event>();
        public List<Models.Event> DisplayEvents { get; set; } = new List<Models.Event>();
        public List<Models.League> Leagues { get; set; } = new List<Models.League>();
        public List<Models.Player> Players { get; set; } = new List<Models.Player>();
        public int SelectedLeague { get; set; }


        // To protect from overposting attacks, see https://aka.ms/RazorPagesCRUD
        public async Task<IActionResult> OnPostAsync()
        {
            SelectedLeague = int.Parse(Request.Form["league-id"]);

            if (!ModelState.IsValid || _context.Event == null || _context.EventResult == null || _context.Player == null || SelectedLeague == 0)
            {
                return Page();
            }

            Event.LeagueID = SelectedLeague;
            _context.Event.Add(Event);
            await _context.SaveChangesAsync();

            for (int i = 1; i < 33; i++)
            {
                if (Request.Form["player" + i] == string.Empty)
                {
                    continue;
                }
                
                var eventResult = new Models.EventResult();
                eventResult.PlayerName = Request.Form["player" + i];
                eventResult.Points = int.Parse(Request.Form["points" + i]);
                eventResult.OMW = float.Parse(Request.Form["omw" + i]);
                eventResult.GW = float.Parse(Request.Form["gw" + i]);
                eventResult.OGW = float.Parse(Request.Form["ogw" + i]);
                eventResult.Placement = int.Parse(Request.Form["placement" + i]);
                eventResult.EventId = Event.ID;

                var playerExists = Players.Where(n => n.Name == eventResult.PlayerName).ToList();
                if (playerExists.Any())
                {
                    var playerToUpdate = playerExists.First();
                    playerToUpdate.Events++;
                    playerToUpdate.Points += eventResult.Points;

                    _context.Player.Update(playerToUpdate);
                    await _context.SaveChangesAsync();

                    eventResult.PlayerId = playerToUpdate.ID;

                    _context.EventResult.Add(eventResult);
                    await _context.SaveChangesAsync();
                }
                else
                {
                    var playerToAdd = new Models.Player();

                    playerToAdd.Name = eventResult.PlayerName;
                    playerToAdd.Events = 1;
                    playerToAdd.Points = eventResult.Points;

                    _context.Player.Add(playerToAdd);
                    await _context.SaveChangesAsync();

                    eventResult.PlayerId = playerToAdd.ID;
                    _context.EventResult.Add(eventResult);
                    await _context.SaveChangesAsync();
                }
            }

            return RedirectToPage("./Index");
        }

        private void UpdatePlayer(Models.Player player)
        {

        }

        public string NamePlayer(int id)
        {
            var str = "player" + id.ToString();
            return str;
        }

        public string OmwPlayer(int id)
        {
            var str = "omw" + id.ToString();
            return str;
        }

        public string GwPlayer(int id)
        {
            var str = "gw" + id.ToString();
            return str;
        }

        public string OgwPlayer(int id)
        {
            var str = "ogw" + id.ToString();
            return str;
        }

        public string PlacementPlayer(int id)
        {
            var str = "placement" + id.ToString();
            return str;
        }

        public string PointsPlayer(int id)
        {
            var str = "points" + id.ToString();
            return str;
        }
    }
}
