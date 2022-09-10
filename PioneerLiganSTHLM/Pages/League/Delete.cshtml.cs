using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.RazorPages;
using Microsoft.EntityFrameworkCore;
using PioneerLiganSTHLM.Data;
using PioneerLiganSTHLM.Models;

namespace PioneerLiganSTHLM.Pages.League
{
    public class DeleteModel : PageModel
    {
        private readonly PioneerLiganSTHLM.Data.PioneerLiganSTHLMContext _context;

        public DeleteModel(PioneerLiganSTHLM.Data.PioneerLiganSTHLMContext context)
        {
            _context = context;
        }

        [BindProperty]
      public Models.League League { get; set; } = default!;

        public async Task<IActionResult> OnGetAsync(int? id)
        {
            if (id == null || _context.League == null)
            {
                return NotFound();
            }

            var league = await _context.League.FirstOrDefaultAsync(m => m.ID == id);

            if (league == null)
            {
                return NotFound();
            }
            else 
            {
                League = league;
            }
            return Page();
        }

        public async Task<IActionResult> OnPostAsync(int? id)
        {
            if (id == null || _context.League == null)
            {
                return NotFound();
            }
            var league = await _context.League.FindAsync(id);

            if (league != null)
            {
                League = league;
                _context.League.Remove(League);
                await _context.SaveChangesAsync();
            }

            return RedirectToPage("./Index");
        }
    }
}
