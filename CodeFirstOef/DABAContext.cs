using System;
using System.Collections.Generic;
using System.Data.Entity;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace CodeFirstOef
{
    public class DABAContext:DbContext
    {
        public DbSet<Artikelgroep> Artikelgroepen { get; set; }
        public DbSet<Artikel> Artikels { get; set; }
        public DbSet<Leverancier> Leveranciers { get; set; }
        protected override void OnModelCreating(DbModelBuilder modelBuilder)
        {
            modelBuilder.Entity<FoodArtikel>().Map(c => c.Requires("Soort").HasValue("F"));
            modelBuilder.Entity<NonFoodArtikel>().Map(c => c.Requires("Soort").HasValue("N"));
        }
    }
}
