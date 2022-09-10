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
    public class DetailsModel : PageModel
    {
        private readonly PioneerLiganSTHLM.Data.PioneerLiganSTHLMContext _context;

        public DetailsModel(PioneerLiganSTHLM.Data.PioneerLiganSTHLMContext context)
        {
            _context = context;
        }

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
    }
}
