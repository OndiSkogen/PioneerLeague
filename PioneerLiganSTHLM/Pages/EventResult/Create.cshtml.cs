using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.RazorPages;
using Microsoft.AspNetCore.Mvc.Rendering;
using PioneerLiganSTHLM.Data;
using PioneerLiganSTHLM.Models;

namespace PioneerLiganSTHLM.Pages.EventResult
{
    public class CreateModel : PageModel
    {
        private readonly PioneerLiganSTHLM.Data.PioneerLiganSTHLMContext _context;

        public CreateModel(PioneerLiganSTHLM.Data.PioneerLiganSTHLMContext context)
        {
            _context = context;
        }

        public IActionResult OnGet()
        {
            return Page();
        }

        [BindProperty]
        public Models.EventResult EventResult { get; set; } = default!;
        

        // To protect from overposting attacks, see https://aka.ms/RazorPagesCRUD
        public async Task<IActionResult> OnPostAsync()
        {
          if (!ModelState.IsValid || _context.EventResult == null || EventResult == null)
            {
                return Page();
            }

            _context.EventResult.Add(EventResult);
            await _context.SaveChangesAsync();

            return RedirectToPage("./Index");
        }
    }
}
