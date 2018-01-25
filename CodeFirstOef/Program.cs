using System;
using System.Collections.Generic;
using System.Data.Entity;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace CodeFirstOef
{
    class Program
    {
        static void Main(string[] args)
        {
            Database.SetInitializer(new DropCreateDatabaseIfModelChanges<DABAContext>());
            using (var entities = new DABAContext())
            {
                Artikelgroep groep = new Artikelgroep { Naam = "Melkproduct" };
                Artikel pudding = new FoodArtikel { Naam = "pudding", Artikelgroep = groep, Houdbaarheid = 2 };
                entities.Artikelgroepen.Add(groep);
                entities.Artikels.Add(pudding);
                entities.SaveChanges();
            }
        }
    }
}
