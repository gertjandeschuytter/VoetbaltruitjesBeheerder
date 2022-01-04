using BusinessLayer.Model;
using BusinessLayer_VoetbaltruitjesWinkel.Exceptions;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Xunit;

namespace UnitTestVoetbaltruitje {
    public class UnitTestKlant {
        private readonly List<Bestelling> _bestellingnPersoon = new();
        private readonly Dictionary<Voetbaltruitje, int> _voetblatruitjeKeys = new();
        private readonly Voetbaltruitje _voetbaltruitje;
        private readonly Club _club;
        private readonly ClubSet _clubSet;
        private readonly Klant _klant;
        private readonly Bestelling _bestelling;

        #region KlantConfiguratie
        public UnitTestKlant()
        {
            _club = new("Premier League", "Manchester United");
            _clubSet = new(true, 1);
            _voetbaltruitje = new(1, _club, "2019-2020", 90, Kledingmaat.M, _clubSet);
            _voetblatruitjeKeys.Add(_voetbaltruitje, 1);
            _klant = new(1, "Gertjan Deschuytter", "Vrijtijdslaan 15, 9000 Gent", _bestellingnPersoon);
            _bestelling = new(1, _klant, DateTime.Now, 50, false, _voetblatruitjeKeys);
            _bestellingnPersoon.Add(_bestelling);
        }
        #endregion


        [Fact]
        public void Test_ctor_NoId_Valid()
        {
            Klant klantje = new Klant("Gertjan", "Vrijtijdslaan");

            Assert.Equal("Gertjan", klantje.Naam);
            Assert.Equal("Vrijtijdslaan", klantje.Adres);
        }

        [Fact]
        public void Test_ctor_Id_Valid()
        {
            Klant klantje = new Klant(1, "Gertjan", "Vrijtijdslaan");
            Assert.Equal(1, klantje.KlantId);
            Assert.Equal("Gertjan", klantje.Naam);
            Assert.Equal("Vrijtijdslaan", klantje.Adres);
        }

        [Fact]
        public void Test_ctor_Id_InValid()
        {
            Assert.Throws<KlantException>(() => new Klant(-10, "Gertjan", "Vrijtijdslaan"));
        }


        [Fact]
        public void Test_ZetId_Valid()
        {
            Klant klantje = (new Klant(1, "Gertjan", "Vrijtijdslaan"));
            klantje.ZetKlantId(1);
            Assert.Equal(1, klantje.KlantId);
        }

        [Theory]
        [InlineData(-1)]
        [InlineData(0)]
        public void Test_ZetId_InValid(int id)
        {
            Klant klant = new Klant("Gertjan", "Vrijtijdslaan");
            var ex = Assert.Throws<KlantException>(() => klant.ZetKlantId(id));
            Assert.Equal("Klant: KlantId is onder 1", ex.Message);
        }


        [Fact]
        public void Test_ZetNaam_Valid()
        {
            Klant klantje = (new Klant(1, "Gertjan", "Vrijtijdslaan"));
            klantje.ZetNaam("Gertjan");
            Assert.Equal("Gertjan", klantje.Naam);
        }

        [Theory]
        [InlineData("")]
        [InlineData(null)]
        public void Test_ZetNaam_InValid(string naam)
        {
            Klant klant = new Klant("Louis", "Vrijtijdslaan");
            var ex = Assert.Throws<KlantException>(() => klant.ZetNaam(naam));
            Assert.Equal("Klant: Naam moet langer dan 1 letter zijn!", ex.Message);
        }

        [Fact]
        public void Test_ZetAdres_Valid()
        {
            Klant klantje = (new Klant(1, "Gertjan", "Vrijtijdslaan"));
            klantje.ZetAdres("Vrijtijdslaan");
            Assert.Equal("Vrijtijdslaan", klantje.Adres);
        }

        [Theory]
        [InlineData("")]
        [InlineData(null)]
        public void Test_ZetAdres_InValid(string adres)
        {
            Assert.Throws<KlantException>(() => _klant.ZetAdres(adres));
        }

        [Fact]
        public void Test_HeeftBestelling()
        {
            Assert.True(_klant.HeeftBestelling(_bestelling));
        }

        [Fact]
        public void Test_VerwijderBestelling()
        {
            Bestelling bestelling = null;
            Assert.Throws<KlantException>(() => _klant.VerwijderBestelling(bestelling));
        }
    }
}
