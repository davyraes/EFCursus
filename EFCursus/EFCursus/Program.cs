using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Transactions;
using System.Data.Entity.Infrastructure;

namespace EFCursus
{
    class Program
    {
        
        static void Main(string[] args)
        {
            BestBetaaldeDocentPerCampusAfbeelden();
            Console.ReadLine();
        }
        static void BestBetaaldeDocentPerCampusAfbeelden()
        {
            using (var entities = new OpleidingenEntities())
            {
                var query = from bestBetaaldeDocentPerCampus in entities.BestBetaaldeDocentenPerCampus
                            orderby bestBetaaldeDocentPerCampus.CampusNr,
                            bestBetaaldeDocentPerCampus.Voornaam, bestBetaaldeDocentPerCampus.Familienaam
                            select bestBetaaldeDocentPerCampus;
                var vorigCampusNr = 0;
                foreach(var bestbetaaldeDocentPerCampus in query)
                {
                    if(bestbetaaldeDocentPerCampus.CampusNr != vorigCampusNr)
                    {
                        Console.WriteLine($"{bestbetaaldeDocentPerCampus.Naam} {bestbetaaldeDocentPerCampus.GrootsteWedde} Grootste wedde :");
                        vorigCampusNr = bestbetaaldeDocentPerCampus.CampusNr;
                    }
                    Console.WriteLine($"\t{bestbetaaldeDocentPerCampus.Voornaam} {bestbetaaldeDocentPerCampus.Familienaam}");
                }
            }
        }
        static void InformeleBegroetingCursisten()
        {
            using (var entities = new OpleidingenEntities())
            {
               foreach(var cursist in (from cursist in entities.Cursisten select cursist))
                {
                    Console.WriteLine(cursist.Naam.InformeleBegroeting);
                }
            }
        }
        static void mentoren()
        {
            using (var entities = new OpleidingenEntities())
            {
                var query = from mentor in entities.Cursisten.Include("Beschermelingen")
                            where mentor.Beschermelingen.Count!=0
                            orderby mentor.Naam.Voornaam, mentor.Naam.Familienaam
                            select mentor;
                foreach (var mentor in query)
                {
                    Console.WriteLine($"{mentor.Naam.Voornaam} {mentor.Naam.Familienaam}");
                    foreach(var beschermeling in mentor.Beschermelingen)
                    {
                        Console.WriteLine($"\t{beschermeling.Naam.Voornaam} {beschermeling.Naam.Familienaam}");
                    }
                }
            }
            Console.ReadLine();

        }
        //static void BoekenPerCursus()
        //{
        //    using (var entities = new OpleidingenEntities())
        //    {
        //        var query = from cursus in entities.Cursussen.Include("BoekenCursussen.Boek")
        //                    orderby cursus.Naam
        //                    select cursus;
        //        foreach(var cursus in query)
        //        {
        //            Console.WriteLine(cursus.Naam);
        //            foreach(var boekCursus in cursus.BoekenCursussen)
        //            {
        //                Console.WriteLine($"\t{boekCursus.VolgNr}:{boekCursus.Boek.Titel}");
        //            }
        //        }
        //    }
        //    Console.ReadLine();

        //}
        static void VoorraadBijvulling()
        {
            try
            {
                Console.Write("Artikel nr.:");
                var artikelnr = int.Parse(Console.ReadLine());
                Console.Write("Magazijn nr.:");
                var magazijnNr = int.Parse(Console.ReadLine());
                Console.Write("Aantal stuks:");
                var aantalstuks = int.Parse(Console.ReadLine());
                VoorraadBijvulling(artikelnr, magazijnNr, aantalstuks);
                Console.ReadLine();
            }
            catch (FormatException)
            {
                Console.WriteLine("tik een getal");
            }

        }
        static void VoorraadBijvulling(int artikelNr,int magazijnNr,int aantalStuks)
        {
            using (var entities = new OpleidingenEntities())
            {
                var voorrraad = entities.Voorraden.Find(magazijnNr, artikelNr);
                if (voorrraad!=null)
                {
                    voorrraad.AantalStuks += aantalStuks;
                    Console.WriteLine("Pas nu de voorraad aan met de ServerExplorer, druk daarna op enter");
                    Console.ReadLine();
                    try
                    {
                        entities.SaveChanges();
                    }
                    catch(DbUpdateConcurrencyException)
                    {
                        Console.WriteLine("voorraad werd door andere applicatie aagepast.");
                    }
                    
                }
                else
                {
                    Console.WriteLine("Voorraad niet gevonden");
                }
            }
        }
        static void VoorraadTransfer()
        {
            try
            {
                Console.Write("Artikel nr.:");
                var artikelnr = int.Parse(Console.ReadLine());
                Console.Write("Van magazijn nr.:");
                var vanMagazijnNr = int.Parse(Console.ReadLine());
                Console.Write("Naar magazijn nr.:");
                var naarMagazijnNr = int.Parse(Console.ReadLine());
                Console.Write("Aantal stuks:");
                var aantalstuks = int.Parse(Console.ReadLine());
                VoorraadTransfer(artikelnr, vanMagazijnNr, naarMagazijnNr, aantalstuks);
            }
            catch(FormatException)
            {
                Console.WriteLine("tik een getal");
            }

        }
        static void VoorraadTransfer(int artikelNr, int vanMagazijnNr, int naarMagazijnNr, int aantalStuks)
        {            
                var transactionOptions = new TransactionOptions { IsolationLevel = IsolationLevel.RepeatableRead };
                using (var transactionScope = new TransactionScope(TransactionScopeOption.Required, transactionOptions))
                {
                    using (var entities = new OpleidingenEntities())
                    {
                        var vanVoorraad = entities.Voorraden.Find(vanMagazijnNr, artikelNr);
                        if (vanVoorraad!=null)
                        {
                            if (vanVoorraad.AantalStuks >= aantalStuks)
                            {
                                vanVoorraad.AantalStuks -= aantalStuks;
                                var naarVoorraad = entities.Voorraden.Find(naarMagazijnNr, artikelNr);
                                if (naarVoorraad!=null)
                                {
                                    naarVoorraad.AantalStuks += aantalStuks;
                                }
                                else
                                {
                                    naarVoorraad = new Voorraad { ArtikelNr = artikelNr, MagazijnNr = naarMagazijnNr, AantalStuks = aantalStuks };
                                    entities.Voorraden.Add(naarVoorraad);
                                }
                                entities.SaveChanges();
                                transactionScope.Complete();
                            }
                            else
                            {
                                Console.WriteLine("Te weinig voorraad voor transfer");
                            }
                        }
                        else
                        {
                            Console.WriteLine($"Artikel niet gevonden in magazijn {vanMagazijnNr}");
                        }
                    }
                }
        }
        static void opzoeken()
        {
            Console.Write("minimum wedde: ");
            Decimal minWedde;
            if (Decimal.TryParse(Console.ReadLine(), out minWedde))
            {
                using (var entities = new OpleidingenEntities())
                {
                    var query = from docent in entities.Docenten
                                where docent.Wedde >= minWedde
                                orderby docent.Naam.Voornaam, docent.Naam.Familienaam
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
