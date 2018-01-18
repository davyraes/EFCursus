using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace EFCursus
{
    class Program
    {
        
        static void Main(string[] args)
        {
            Console.Write("minimum wedde: ");
            Decimal minWedde;
            if (Decimal.TryParse(Console.ReadLine(), out minWedde))
            {
                using (var entities = new OpleidingenEntities())
                {
                    var query = from docent in entities.Docenten
                                where docent.Wedde >= minWedde
                                orderby docent.Voornaam, docent.Familienaam
                                select docent;
                    foreach (var docent in query)
                    {
                        Console.WriteLine($"{docent.Naam}: {docent.Wedde}");
                    }
                }
            }
            Console.ReadLine();
        }
    }
}
