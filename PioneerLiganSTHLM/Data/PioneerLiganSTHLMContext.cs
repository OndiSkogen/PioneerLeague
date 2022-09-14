using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.EntityFrameworkCore;
using Microsoft.AspNetCore.Identity.EntityFrameworkCore;
using PioneerLiganSTHLM.Models;
using PioneerLiganSTHLM.Areas.Identity.Data;

namespace PioneerLiganSTHLM.Data
{
    public class PioneerLiganSTHLMContext : IdentityDbContext<User>
    {
        public PioneerLiganSTHLMContext (DbContextOptions<PioneerLiganSTHLMContext> options)
            : base(options)
        {
        }

        public DbSet<PioneerLiganSTHLM.Models.Event> Event { get; set; } = default!;

        public DbSet<PioneerLiganSTHLM.Models.EventResult>? EventResult { get; set; }

        public DbSet<PioneerLiganSTHLM.Models.Player>? Player { get; set; }

        public DbSet<PioneerLiganSTHLM.Models.League>? League { get; set; }
    }
}
