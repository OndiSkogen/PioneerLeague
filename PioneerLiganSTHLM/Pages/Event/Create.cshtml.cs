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
        private readonly PioneerLiganSTHLM.Data.PioneerLiganSTHLMContext _context;

        public CreateModel(PioneerLiganSTHLM.Data.PioneerLiganSTHLMContext context)
        {
            _context = context;
        }

        public IActionResult OnGet(string? selectedId)
        {
            var leagues = from l in _context.League select l;
            Leagues = leagues.OrderBy(l => l.ID).ToList();
            var events = from e in _context.Event select e;
            Events = events.OrderBy(l => l.ID).ToList();

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

        public Models.Event Event { get; set; } = default!;
        public List<Models.Event> Events { get; set; } = new List<Models.Event>();
        public List<Models.Event> DisplayEvents { get; set; } = new List<Models.Event>();
        public List<Models.League> Leagues { get; set; } = new List<Models.League>();
        public int SelectedLeague { get; set; }


        // To protect from overposting attacks, see https://aka.ms/RazorPagesCRUD
        public async Task<IActionResult> OnPostAsync()
        {
          if (!ModelState.IsValid || _context.Event == null || Event == null)
            {
                return Page();
            }

            _context.Event.Add(Event);
            await _context.SaveChangesAsync();

            return RedirectToPage("./Index");
        }

        public ActionResult LoadLeagueData(string id)
        {
            SelectedLeague = int.Parse(id);
            DisplayEvents = Events.Where(i => i.LeagueID == SelectedLeague).ToList();
            return Page();
        }
    }
}
