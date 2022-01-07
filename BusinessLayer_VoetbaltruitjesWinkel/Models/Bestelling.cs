using BusinessLayer_VoetbaltruitjesWinkel.Exceptions;
using System;
using System.Collections.Generic;

namespace BusinessLayer.Model {
    public class Bestelling : IBestelling {
        //properties
        public int BestellingId { get; private set; }
        public bool Betaald { get; private set; }
        public Klant Klant { get; private set; }
        public double Prijs { get; private set; }
        public DateTime Tijdstip { get; private set; }

        //fields
        private Dictionary<Voetbaltruitje, int> _producten = new Dictionary<Voetbaltruitje, int>();

        //ctors
        public Bestelling(int bestellingId, DateTime tijdstip) : this(tijdstip) {
            ZetBestellingId(bestellingId);
            ZetTijdstip(tijdstip);
        }
        public Bestelling(int bestellingId, Klant klant, DateTime tijdstip) {
            ZetBestellingId(bestellingId);
            ZetTijdstip(tijdstip);
            ZetKlant(klant);
        }
        public Bestelling(int bestellingId, Klant klant, DateTime tijdstip, Dictionary<Voetbaltruitje, int> producten) {
            ZetBestellingId(bestellingId);
            ZetTijdstip(tijdstip);
            ZetKlant(klant);
            _producten = producten;
        }
        public Bestelling(int bestellingId, Klant klant, DateTime tijdstip, double prijs, bool betaald, Dictionary<Voetbaltruitje, int> producten) {
            ZetBestellingId(bestellingId);
            ZetTijdstip(tijdstip);
            ZetKlant(klant);
            ZetBetaald(betaald);
            _producten = producten;
            ZetPrijs(prijs);
        }
        public Bestelling(Klant klant, DateTime tijdstip, bool betaald, Dictionary<Voetbaltruitje, int> producten, double prijs) : this(tijdstip) {
            _producten = producten;
            ZetKlant(klant);
            ZetPrijs(prijs);
            ZetBetaald(betaald);
        }
        public Bestelling(DateTime tijdstip) {
            ZetTijdstip(tijdstip);
        }

        public void ZetPrijs(double prijs)
        {
            if (prijs <= 0) throw new BestellingException("Bestelling: Prijs mag niet kleiner zijn dan 0");
            this.Prijs = Kostprijs();
        }
        // methods (done)
        public void ZetTijdstip(DateTime tijdstip) {
            if (tijdstip.GetHashCode() == 0) throw new BestellingException("Bestelling: Tijdstip klopt niet");
            Tijdstip = tijdstip;
        }
        public IReadOnlyDictionary<Voetbaltruitje, int> GeefProducten() {
            return _producten;
        }
        public double Kostprijs() {
            double prijs = 0;
            int korting;
            if (Klant is null) {
                korting = 0;
            } else {
                korting = Klant.Korting();
            }

            foreach (KeyValuePair<Voetbaltruitje, int> kvp in _producten) {
                prijs += (kvp.Key.Prijs * kvp.Value) * (1.0 - (korting/100.00));
            }
            return prijs;
        }
        public void VerwijderKlant() {
            Klant = null;
        }
        public void VerwijderProduct(Voetbaltruitje voetbaltruitje, int aantal) {
            if (aantal <= 0) throw new BestellingException("VerwijderVoetbaltruitje - aantal");
            if (!_producten.ContainsKey(voetbaltruitje)) {
                throw new BestellingException("VerwijderVoetbaltruitje - product niet beschikbaar");
            } else {
                //als de value van dat truitje te klein is
                if (_producten[voetbaltruitje] < aantal) {
                    throw new BestellingException("VerwijderVoetbaltruitje - beschikbare aantal te klein");
                } else {
                    _producten[voetbaltruitje] += aantal;
                }
            }
        }
        public void VoegProductToe(Voetbaltruitje voetbaltruitje, int aantal) {
            if (aantal <= 0) throw new BestellingException("VoegVoetbaltruitjeToe - aantal");
            if (_producten.ContainsKey(voetbaltruitje)) {
                _producten[voetbaltruitje] += aantal;
            } else {
                _producten.Add(voetbaltruitje, aantal);
            }
        }
        public void VoegProductenToe(Dictionary<Voetbaltruitje, int> producten) {
            _producten = producten;
        }
        public void ZetBestellingId(int id) {
            if (id <= 0) throw new BestellingException("Bestelling - invalid id");
            this.BestellingId = id;
        }
        public void ZetBetaald(bool betaald) {
            if (!betaald) {
                Betaald = false;
            } else {
                Betaald = true;
            }
            Prijs = Kostprijs();
        }
        public void ZetKlant(Klant newKlant) {
            if (newKlant == null) throw new BestellingException("Bestelling - invalid klant");
            if (newKlant == Klant) throw new BestellingException("Bestelling - ZetKlant - not new");
            if (Klant != null)
                if (Klant.HeeftBestelling(this))
                    Klant.VerwijderBestelling(this);
            if (!newKlant.HeeftBestelling(this))
                newKlant.VoegToeBestelling(this);
            Klant = newKlant;
        }
        public override string ToString() {
            string res = $"[Bestelling] {BestellingId},{Betaald},{Prijs},{Tijdstip},{Klant.KlantId},{Klant.Naam},{Klant.Adres},{_producten.Count}";
            foreach (var p in _producten) {
                res += $"\n {p}";
            }
            return res;
        }
    }
}
