using System;
using System.Collections.Generic;

namespace BusinessLayer.Model
{
    public interface IBestelling
    {
        int BestellingId { get; }
        bool Betaald { get; }
        Klant Klant { get; }
        double Prijs { get; }
        DateTime Tijdstip { get; }

        IReadOnlyDictionary<Voetbaltruitje, int> GeefProducten();
        double Kostprijs();

        void VerwijderKlant();
        void VerwijderProduct(Voetbaltruitje voetbaltruitje, int aantal);
        void VoegProductToe(Voetbaltruitje voetbaltruitje, int aantal);
        void ZetBestellingId(int id);
        void ZetBetaald(bool betaald = true);
        void ZetKlant(Klant newKlant);
        void ZetTijdstip(DateTime tijdstip);
    }
}