using BusinessLayer.Model;
using BusinessLayer_VoetbaltruitjesWinkel.DATALAYER;
using System;
using System.Collections.Generic;
using System.Linq;

namespace VerkoopTestConsole {
    class Program {
        static void Main(string[] args) {
            //testen clubs
            ClubRepositoryADO Club = new ClubRepositoryADO(@"Data Source=(localdb)\MSSQLLocalDB;Initial Catalog=verkoop;Integrated Security=True");
            //Console.WriteLine(Club.BestaatCompetitie("Premier League"));
            //foreach (var item in Club.GeefClubs("Premier League")) {
            //    Console.WriteLine(item);
            //}
            //foreach (var item in Club.GeefCompetities()) {
            //    Console.WriteLine(item);
            //}

            //testen voetbaltruitje
            VoetbaltruitjeRepositoryADO truitje = new VoetbaltruitjeRepositoryADO(@"Data Source=(localdb)\MSSQLLocalDB;Initial Catalog=verkoop;Integrated Security=True");
            //Voetbaltruitje t1 = new Voetbaltruitje(new Club("Premier League", "Manchester City"), "2021-2022", 85, Kledingmaat.XL, new ClubSet(true, 1));
            //Voetbaltruitje t2 = new Voetbaltruitje(new Club("Premier League", "Manchester City"), "2021-2022", 80, Kledingmaat.XL, new ClubSet(true, 1));
            //Voetbaltruitje t3 = new Voetbaltruitje(new Club("Jupiler Pro League", "Cercle"), "2021-2022", 70, Kledingmaat.L, new ClubSet(true, 1));
            //Voetbaltruitje t4 = new Voetbaltruitje(new Club("Jupiler Pro League", "Union"), "2021-2022", 75, Kledingmaat.S, new ClubSet(true, 1));
            //Voetbaltruitje t5 = new Voetbaltruitje(new Club("Jupiler Pro League", "Antwerp"), "2021-2022", 79, Kledingmaat.S, new ClubSet(false, 1));
            //Voetbaltruitje t6 = new Voetbaltruitje(new Club("Jupiler Pro League", "Eupen"), "2021-2022", 65, Kledingmaat.M, new ClubSet(false, 2));

            //Console.WriteLine(truitje.BestaatVoetbaltruitje(1));
            //Console.WriteLine(truitje.GeefVoetbaltruitje(2));
            //Console.WriteLine(truitje.BestaatVoetbaltruitje(t));
            //truitje.VoegVoetbaltruitjeToe(t6);
            //truitje.UpdateVoetbaltruitje(t6);
            //UPDATE en DELETE werken nog niet omdat deze voorlopig in het geheugen zitten
            //foreach (var item in truitje.GeefVoetbaltruitjes("Jupiler Pro League",null,null,null,null,null,null)) {
            //    Console.WriteLine(item);
            //}

            //testen klant
            KlantRepositoryADO k = new KlantRepositoryADO(@"Data Source=(localdb)\MSSQLLocalDB;Initial Catalog=verkoop;Integrated Security=True");
            //Klant k1 = new Klant("Ludovic Bourgeois", "Legeweg 110 - 8200 Brugge");
            //var tomm = k.GeefKlant(3);
            //Klant k2 = new Klant("Tom Verstraeten", "Zandstraat 12 - 8490 Varsenare");
            //Console.WriteLine(k.bestaatKlant(k1));
            //Console.WriteLine(k.bestaatKlant(1));
            //k.voegKlantToe(k2);
            //foreach (Klant item in k.GeefKlanten("Tom Verstraeten",null)) {
            //    Console.WriteLine(item);
            //}
            //Console.WriteLine(k.GeefKlant(2));
            //k.verwijderKlant(new Klant(1,"Gertjan Deschuytter", "Coudeveldt 2 - 8490 Varsenare"));
            //Console.WriteLine(k.GeefKlant(1));
            BestellingRepositoryADO b = new BestellingRepositoryADO(@"Data Source=(localdb)\MSSQLLocalDB;Initial Catalog=verkoop;Integrated Security=True");
            //var klant = k.GeefKlant(1);
            //Dictionary<Voetbaltruitje, int> prods = new Dictionary<Voetbaltruitje, int>();
            //prods.Add(new Voetbaltruitje(4, new Club("Jupiler Pro League", "Cercle"), "2021-2022", 70, Kledingmaat.L, new ClubSet(true, 1)), 10);
            //Bestelling b1 = new Bestelling(klant, DateTime.Now, true, prods);
            //b.VoegBestellingToe(b1);
            //Console.WriteLine(b.BestaatBestelling(1));
            //Bestelling bestelling = b.GeefBestellingen(1, null, null).First();
            //Bestelling best = new Bestelling(1,k1, DateTime.Now, true, prods);
            //Bestelling best = new Bestelling(1, k1, DateTime.Now, prods);
            //best.ZetBetaald(true);
            //b.UpdateBestelling(best);
            //Bestelling beetje = b.GeefBestelling(2);
            //b.VerwijderBestelling(beetje);
        }
    }
}
