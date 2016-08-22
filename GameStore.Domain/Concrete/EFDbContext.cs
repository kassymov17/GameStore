using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Data.Entity;
using GameStore.Domain.Entities;

namespace GameStore.Domain.Concrete
{
    class EFDbContext:DbContext
    {
        public DbSet<Game> Games { get; set; }
    }
}
