using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Transactions;
using System.Data.Entity.Infrastructure;

namespace EFTaken
{
    public class Program
    {
        private static List<Klant> klanten;
        static void Main(string[] args)
        {


        }
        static void personeel()
        {
            using (var entities = new BankEntities)
            {
                var querry = (from personeel in entities.Personeel
                              where personeel.Manager == null
                              select personeel.Manager).ToList();
                             
                foreach (var manager in querry)
                {
                    if(manager.ManagerNr == null)
                    {
                        Console.WriteLine($"{manager.Voornaam}");                        
                    }
                    if(manager.Ondergeschikten.Count!=0)
                    {
                        foreach( var personeel in manager.Ondergeschikten)
                        {
                            Console.WriteLine($"/t{personeel.Voornaam}");
                            if(personeel.Ondergeschikten.Count!=0)

                        }
                    }
                }                
            }
        }
        static void PersoonAfbeelden(PersoneelsLid persoon, int tabs)
        {
            if (persoon.Ondergeschikten.Count != 0)
            {
                foreach (var personeel in persoon.Ondergeschikten)
                {
                    Console.WriteLine(new string('\t',tabs)+$"{personeel.Voornaam}");
                    if (personeel.Ondergeschikten.Count!=0)
                }
            }
        }
        static void KlantWijzigen()
        {
            Console.Write("Geef het klant nr.:");
            int klantNr;
            if(int.TryParse (Console.ReadLine(),out klantNr))
            {
                using (var entities = new BankEntities())
                {
                    var klant = entities.Klanten.Find(klantNr);
                    if (klant!=null)
                    {
                        Console.WriteLine("Nieuw naam :");
                        klant.Voornaam = Console.ReadLine();
                        try
                        {
                            entities.SaveChanges();
                        }
                        catch(DbUpdateConcurrencyException)
                        {
                            Console.WriteLine("Een andere gebruiker wijzigde deze klant");
                        }
                    }
                    else
                    {
                        Console.WriteLine("Klant niet gevonden");
                    }
                }
            }
            else
            {
                Console.WriteLine("Tik een getal");
            }
            Console.ReadLine();
        }
        static List<Klant> FindKlanten()
        {
            using (var entities = new BankEntities())
            {
                return (from klant in entities.Klanten.Include("Rekeningen") 
                        orderby klant.Voornaam
                        select klant).ToList();
            }
        }
        static void VraagSaldosOp()
        {
            foreach (var klant in FindKlanten())
            {
                decimal TotaalSaldo=0m;
                Console.WriteLine(klant.Voornaam);
                if (klant.Rekeningen!=null)
                {
                    foreach(var rekening in klant.Rekeningen)
                    {
                        Console.WriteLine($"{rekening.RekeningNr}:{rekening.Saldo}");
                        TotaalSaldo += rekening.Saldo;
                    }
                }
                Console.WriteLine($"Totaal:{TotaalSaldo}");
                Console.WriteLine();
            }
            Console.Read();

        }
        static void VoegZichtrekeningToe()
        {
            klanten = FindKlanten();
            foreach (var klant in klanten)
            {
                Console.WriteLine($"{klant.KlantNr}:{klant.Voornaam}");
            }

            int getal = 0;
            while (getal == 0 || getal > klanten.Count)
            {
                Console.Write("KlantNr:");
                if (int.TryParse(Console.ReadLine(), out getal))
                {
                    if (getal > klanten.Count)
                    {
                        Console.WriteLine("Klant niet gevonden");
                    }
                    else
                    {
                        Console.Write("Geef nieuw rekeningnr:");
                        Rekening rekening = new Rekening() { KlantNr = getal, RekeningNr = Console.ReadLine(), Saldo = 0m, Soort = "Z" };
                        using (var entities = new BankEntities())
                        {
                            entities.Rekeningen.Add(rekening);
                            entities.SaveChanges();
                        }
                    }
                }
                else
                {
                    Console.WriteLine("tik een getal");
                }
            }
        }
        static void Storten()
        {
            Console.Write("Geef het rekeningnummer:");
            string rekningnr= Console.ReadLine();
            using (var entities = new BankEntities())
            {
                Rekening rekening1= entities.Rekeningen.Find(rekningnr);
                if (rekening1!=null)
                {
                    Console.WriteLine("Te storten bedrag :");
                    decimal bedrag;
                    if (decimal.TryParse(Console.ReadLine(), out bedrag))
                    {
                        if (bedrag > 0)
                        {
                            rekening1.Saldo += bedrag;
                            entities.SaveChanges();
                        }
                        else
                        {
                            Console.WriteLine("tik een positief getal");
                        }
                    }
                    else
                    {
                        Console.WriteLine("tik een getal");
                    }
                }
                else
                {
                    Console.WriteLine("Kan rekening niet vinden");
                }
            }

        }
        static void verwijderen()
        {
            Console.Write("geef het klantNr :");
            int klantnr;
            if(int.TryParse(Console.ReadLine(),out klantnr))
            {
                using (var entities = new BankEntities())
                {
                    Klant klant = entities.Klanten.Find(klantnr);
                    if (klant != null)
                    {
                        if (klant.Rekeningen.Count == 0)
                        {
                            entities.Klanten.Remove(klant);
                            entities.SaveChanges();
                        }
                        else
                        {
                            Console.WriteLine("klant heeft nog rekeningen");
                        }
                    }
                    else
                    {
                        Console.WriteLine("klant niet gevonden");
                    }
                }                
            }
            else
            {
                Console.WriteLine("tik een getal");
            }
            Console.ReadLine();
        }
        static void Overschrijven(string vanRekeningNr,string naarRekeningNr,decimal bedrag)
        {
            var transactionOptions = new TransactionOptions { IsolationLevel = IsolationLevel.RepeatableRead };
            using (var transactionScope = new TransactionScope(TransactionScopeOption.Required, transactionOptions))
            {
                using (var entities = new BankEntities())
                {
                    var OverschrijvenVan = entities.Rekeningen.Find(vanRekeningNr);
                    var OverschrijvenNaar = entities.Rekeningen.Find(naarRekeningNr);
                    if(OverschrijvenVan!=null)
                    {
                        if (bedrag > 0)
                        {
                            if (OverschrijvenVan.Saldo >= bedrag)
                            {
                                OverschrijvenVan.Saldo -= bedrag;
                                if (OverschrijvenNaar != null)
                                {
                                    OverschrijvenNaar.Saldo += bedrag;
                                    entities.SaveChanges();
                                    transactionScope.Complete();
                                }
                                else
                                {
                                    Console.WriteLine("Naar-rekening niet gevonden");
                                }
                            }
                            else
                            {
                                Console.WriteLine("saldo ontoereikend");
                            }
                        }
                        else
                        {
                            Console.WriteLine("Tik een positief bedrag");
                        }
                    }
                    else
                    {
                        Console.WriteLine("Van-rekening niet gevonden");
                    }
                }
            }
        }
        static void OverschrijvenAlgemeen()
        {
            Console.Write("Van rekening nr.:");
            var vanRekNr = Console.ReadLine();
            Console.Write("Naar rekening nr.:");
            var NaarRekNr = Console.ReadLine();
            Console.Write("bedrag :");
            decimal bedrag;
            if (!decimal.TryParse(Console.ReadLine(),out bedrag))
            {
                Console.WriteLine("Tik een getal");
            }
            else
            {
                Overschrijven(vanRekNr, NaarRekNr, bedrag);
            }

        }
    }
}
