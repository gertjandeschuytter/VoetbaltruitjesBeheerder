using BusinessLayer.Model;
using BusinessLayer_VoetbaltruitjesWinkel.Exceptions;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Xunit;

namespace UnitTestVoetbaltruitje {
    public class UnitTestBestelling {

        private readonly Klant _klant;
        private readonly Voetbaltruitje _voetbaltruitje;
        private readonly Dictionary<Voetbaltruitje, int> _voetbaltruitjeKeys = new();
        private readonly Bestelling _bestelling;

        public UnitTestBestelling()
        {
            _klant = new(64, "Jan Pietermans", "Kerkstraat");
            _voetbaltruitje = new(1, new("PintjesLiga", "Varsenare United"), "2019-2020", 44, Kledingmaat.M, new(true, 1));
            _voetbaltruitjeKeys.Add(_voetbaltruitje, 1);
            _bestelling = new(1, _klant, DateTime.Now, 40, true, _voetbaltruitjeKeys);
        }

        [Theory]
        [InlineData(-1)]
        [InlineData(-10)]
        public void Test_ctor_noId_InValid(int id)
        {
            Assert.Throws<BestellingException>(() => new Bestelling(id, new Klant(1, "Pieter", "Sint-Martens-Latem"), DateTime.Today, new Dictionary<Voetbaltruitje, int>()));
        }

        [Theory]
        [InlineData(0)]
        [InlineData(-11)]
        public void Test_ctor_noKlantId_InValid(int klantid)
        {
            Assert.Throws<KlantException>(() => new Bestelling(1, new Klant(klantid, "Bavo", "Oostkamp"), DateTime.Today, new Dictionary<Voetbaltruitje, int>()));
        }

        [Fact]
        public void Test_ctor_noKlantNaam_InValid()
        {
            Assert.Throws<KlantException>(() => new Bestelling(1, new Klant(1, null, "Wingene"), DateTime.Today, new Dictionary<Voetbaltruitje, int>()));
        }

        [Fact]
        public void Test_ctor_noKlantAdres_InValid()
        {
            Assert.Throws<KlantException>(() => new Bestelling(1, new Klant(1, "Danny", null), DateTime.Today, new Dictionary<Voetbaltruitje, int>()));
        }

        [Theory]
        [InlineData(-0.25)]
        [InlineData(-11)]
        [InlineData(-11.25)]
        public void Test_ctor_noPrijs_InValid(double prijs)
        {
            Assert.Throws<BestellingException>(() => new Bestelling(1, new Klant(1, "Deschuytter", "Sint-Michiels"), DateTime.Today, prijs, true, new Dictionary<Voetbaltruitje, int>()));
        }

        [Fact]
        public void Test_ZetKlant_Valid()
        {
            Bestelling bestelling = new Bestelling(1, new Klant(1, "Bourgeois", "Sint-Michiels"), DateTime.Today, 22.5, true, new Dictionary<Voetbaltruitje, int>());
            bestelling.ZetKlant(new Klant("Bourgeois", "Sint-Michiels"));
            Assert.Equal("Bourgeois", bestelling.Klant.Naam);
            Assert.Equal("Sint-Michiels", bestelling.Klant.Adres);
        }

        [Fact]
        public void Test_ZetPrijs_Valid()
        {
            Bestelling bestelling = new Bestelling(1, new Klant(1, "Bourgeois", "Koolkerke"), DateTime.Today, 22.5, true, new Dictionary<Voetbaltruitje, int>());
            bestelling.ZetPrijs(22.5);
            Assert.Equal(22.5, bestelling.Prijs);
        }

        [Theory]
        [InlineData(-1)]
        [InlineData(0)]
        public void Test_ZetId_InValid(int id)
        {
            Bestelling bestelling = new Bestelling(1, new Klant(1, "Bourgeois", "Zeebrugge"), DateTime.Today, 22.5, true, new Dictionary<Voetbaltruitje, int>());
            var ex = Assert.Throws<BestellingException>(() => bestelling.ZetBestellingId(id));
            Assert.Equal("Bestelling - invalid id", ex.Message);
        }

        [Fact]
        public void Test_ZetKlant_InValid()
        {
            Bestelling bestelling = new Bestelling(1, new Klant(1, "Bourgeois", "Maldegem"), DateTime.Today, 22.5, true, new Dictionary<Voetbaltruitje, int>());
            var ex = Assert.Throws<BestellingException>(() => bestelling.ZetKlant(null));
            Assert.Equal("Bestelling - invalid klant", ex.Message);
        }

        [Theory]
        [InlineData(-0.25)]
        [InlineData(-11)]
        [InlineData(-11.25)]
        public void Test_ZetPrijs_InValid(double prijs)
        {
            Bestelling bestelling = new Bestelling(1, new Klant(1, "Gheysens", "Zulte"), DateTime.Today, 22.5, true, new Dictionary<Voetbaltruitje, int>());
            var ex = Assert.Throws<BestellingException>(() => bestelling.ZetPrijs(prijs));
            Assert.Equal("Bestelling: Prijs mag niet kleiner zijn dan 0", ex.Message);
        }

        [Fact]
        public void Test_VoegTruitjeToe_Valid()
        {
            Bestelling b = new Bestelling(DateTime.Now);
            Voetbaltruitje tr = new Voetbaltruitje(12, new Club("premier league", "city"), "2021-2022", 87, Kledingmaat.M, new ClubSet(true, 1));

            b.VoegProductToe(tr, 1);
            Assert.True(b.GeefProducten().ContainsKey(tr));
        }

        [Theory()]
        [InlineData(null, 0)]
        public void VerwijderProductTest(Voetbaltruitje voetbaltruitje, int aantal)
        {
            Assert.Throws<BestellingException>(() => _bestelling.VerwijderProduct(voetbaltruitje, aantal));
        }
    }
}
