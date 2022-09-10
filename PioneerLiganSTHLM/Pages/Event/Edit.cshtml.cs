using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.RazorPages;
using Microsoft.AspNetCore.Mvc.Rendering;
using Microsoft.EntityFrameworkCore;
using PioneerLiganSTHLM.Data;
using PioneerLiganSTHLM.Models;

namespace PioneerLiganSTHLM.Pages.Event
{
    public class EditModel : PageModel
    {
        private readonly PioneerLiganSTHLM.Data.PioneerLiganSTHLMContext _context;

        public EditModel(PioneerLiganSTHLM.Data.PioneerLiganSTHLMContext context)
        {
            _context = context;
        }

        [BindProperty]
        public Models.Event Event { get; set; } = default!;

        public async Task<IActionResult> OnGetAsync(int? id)
        {
            if (id == null || _context.Event == null)
            {
                return NotFound();
            }

            var events =  await _context.Event.FirstOrDefaultAsync(m => m.ID == id);
            if (events == null)
            {
                return NotFound();
            }
            Event = events;
            return Page();
        }

        // To protect from overposting attacks, enable the specific properties you want to bind to.
        // For more details, see https://aka.ms/RazorPagesCRUD.
        public async Task<IActionResult> OnPostAsync()
        {
            if (!ModelState.IsValid)
            {
                return Page();
            }

            _context.Attach(Event).State = EntityState.Modified;

            try
            {
                await _context.SaveChangesAsync();
            }
            catch (DbUpdateConcurrencyException)
            {
                if (!EventExists(Event.ID))
                {
                    return NotFound();
                }
                else
                {
                    throw;
                }
            }

            return RedirectToPage("./Index");
        }

        private bool EventExists(int id)
        {
          return (_context.Event?.Any(e => e.ID == id)).GetValueOrDefault();
        }
    }
}
