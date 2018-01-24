using System;
using System.Collections.Generic;
using System.Data.Entity;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace CodeFirstCursus
{
    class Program
    {
        static void Main(string[] args)
        {
            Database.SetInitializer(new DropCreateDatabaseIfModelChanges<VdabContext>());
            using (var context = new VdabContext())
            {
                Cursist joe = new Cursist { Voornaam = "Joe", Familienaam = "Dalton" };
                Cursist averell = new Cursist { Voornaam = "Averell", Familienaam = "Dalton", Mentor = joe };
                context.Cursisten.Add(joe);
                context.Cursisten.Add(averell);               
                context.SaveChanges();
            }
        }
    }
}
