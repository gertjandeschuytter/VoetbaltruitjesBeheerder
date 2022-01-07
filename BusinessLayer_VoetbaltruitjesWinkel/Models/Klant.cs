using BusinessLayer_VoetbaltruitjesWinkel.Exceptions;
using System;
using System.Collections.Generic;
using System.Text;

namespace BusinessLayer.Model
{
    public class Klant : IKlant
    {
        //properties
        public string Adres { get; private set; }
        public int KlantId { get; private set; }
        public string Naam { get; private set; }

        //fields
        private List<Bestelling> _bestellingen = new List<Bestelling>();

        //ctors
        public Klant(int klantId, string naam, string adres, List<Bestelling> bestellingen) : this(klantId, naam, adres)
        {
            ZetKlantId(klantId);
            ZetNaam(naam);
            ZetAdres(adres);
        }
        public Klant(int klantId, string naam, string adres) : this(naam, adres)
        {
            ZetKlantId(klantId);
            ZetNaam(naam);
            ZetAdres(adres);
        }
        public Klant(string naam, string adres)
        {
            ZetNaam(naam);
            ZetAdres(adres);
        }

        //methods
        public IReadOnlyList<Bestelling> GetBestellingen()
        {
            return _bestellingen.AsReadOnly();
        }
        public void ZegBestellingen()
        {
            foreach (var item in _bestellingen)
            {
                Console.WriteLine(item);
                Console.Write("");
            }
        }
        public void ZetBestellingen(List<Bestelling> bestellingen)
        {
            foreach (var item in bestellingen)
            {
                _bestellingen.Add(item);
            }
        }
        public void VerwijderBestelling(Bestelling bestelling)
        {
            if (bestelling == null) throw new KlantException("Klant : VerwijderBestelling - Bestelling is null");
            if (!_bestellingen.Contains(bestelling)) throw new KlantException("Klant : VerwijderBestelling - bestelling bestaat niet");
            else if (bestelling.Klant == this) bestelling.VerwijderKlant();
            _bestellingen.Remove(bestelling);
        }
        //public string ToText(bool kort = true)
        //{
        //    if (kort)
        //        return $"[Klant] {KlantId},{Naam},{Adres},{_bestellingen.Count}";
        //    else
        //    {
        //        string res = $"[Klant] {KlantId},{Naam},{Adres},{_bestellingen.Count}";
        //        foreach (var x in _bestellingen)
        //        {
        //            res += $"\n{x}";
        //        }
        //        return res;
        //    }
        //}

        public void VoegToeBestelling(Bestelling bestelling)
        {
            if (bestelling == null) throw new BestellingException("Klant : VoegToeBestelling - bestelling is null");
            if (!HeeftBestelling(bestelling))
            {
                _bestellingen.Add(bestelling);
            }
            if (bestelling.Klant != this)
            {
                bestelling.ZetKlant(this);
            }
        }
        public bool HeeftBestelling(Bestelling bestelling)
        {
            if (!_bestellingen.Contains(bestelling)) return false;
            return true;
        }
        public int Korting()
        {
            if (_bestellingen.Count < 5) return 0;
            if (_bestellingen.Count < 10) return 10;
            else return 20;
        }
        public void ZetAdres(string adres)
        {
            if (string.IsNullOrWhiteSpace(adres)) throw new KlantException("Klant: Adres is leeg!");
            this.Adres = adres;
        }
        public void ZetKlantId(int id)
        {
            if (id <= 0) throw new KlantException("Klant: KlantId is onder 1");
            this.KlantId = id;
        }
        public void ZetNaam(string naam)
        {
            if (string.IsNullOrWhiteSpace(naam)) throw new KlantException("Klant: Naam moet langer dan 1 letter zijn!");
            Naam = naam;
        }
        public int GeefAantalBestellingen()
        {
            int result = 0;
            return result = _bestellingen.Count;
        }
        public override string ToString()
        {
            return $"Naam: {Naam}\nKlantenNummer: {KlantId}\nAdres: {Adres}";
        }
    }
}
