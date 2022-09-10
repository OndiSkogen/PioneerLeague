using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.RazorPages;
using Microsoft.EntityFrameworkCore;
using PioneerLiganSTHLM.Data;
using PioneerLiganSTHLM.Models;

namespace PioneerLiganSTHLM.Pages.EventResult
{
    public class DeleteModel : PageModel
    {
        private readonly PioneerLiganSTHLM.Data.PioneerLiganSTHLMContext _context;

        public DeleteModel(PioneerLiganSTHLM.Data.PioneerLiganSTHLMContext context)
        {
            _context = context;
        }

        [BindProperty]
      public Models.EventResult EventResult { get; set; } = default!;

        public async Task<IActionResult> OnGetAsync(int? id)
        {
            if (id == null || _context.EventResult == null)
            {
                return NotFound();
            }

            var eventresult = await _context.EventResult.FirstOrDefaultAsync(m => m.ID == id);

            if (eventresult == null)
            {
                return NotFound();
            }
            else 
            {
                EventResult = eventresult;
            }
            return Page();
        }

        public async Task<IActionResult> OnPostAsync(int? id)
        {
            if (id == null || _context.EventResult == null)
            {
                return NotFound();
            }
            var eventresult = await _context.EventResult.FindAsync(id);

            if (eventresult != null)
            {
                EventResult = eventresult;
                _context.EventResult.Remove(EventResult);
                await _context.SaveChangesAsync();
            }

            return RedirectToPage("./Index");
        }
    }
}
