using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Data.Entity;
namespace CodeFirstCursus
{
    public class VdabContext : DbContext
    {
        public DbSet<Instructeur> Instructeurs { get; set; }
        public DbSet<Campus> Campussen { get; set; }
        public DbSet<Land> Landen { get; set; }
        public DbSet<Cursus> Cursussen { get; set; }
        public DbSet<Verantwoordelijkheid> Verantwoordelijkeheden { get; set; }
        public DbSet<Cursist> Cursisten { get; set; }
        protected override void OnModelCreating(DbModelBuilder modelBuilder)
        {
            modelBuilder.Entity<KlassikaleCursus>().Map(m => m.Requires("Soort").HasValue("K"));
            modelBuilder.Entity<ZelfstudieCursus>().Map(m => m.Requires("Soort").HasValue("Z"));
            modelBuilder.Entity<Instructeur>().HasMany(i => i.Verantwoodelijkheden).WithMany(v => v.Instructeurs).Map(c => c.ToTable("InstructeursVerantwoordelijkheden").MapLeftKey("InstructeurNr").MapRightKey("verantwoordelijkheidId"));
        }
    }
}
