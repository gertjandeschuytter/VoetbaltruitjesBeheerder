using System.Collections.Generic;

namespace BusinessLayer.Model
{
    public interface IKlant
    {
        string Adres { get; }
        int KlantId { get; }
        string Naam { get; }

        IReadOnlyList<Bestelling> GetBestellingen();
        bool HeeftBestelling(Bestelling bestelling);
        void VerwijderBestelling(Bestelling bestelling);
        void VoegToeBestelling(Bestelling bestelling);
        void ZetAdres(string adres);
        void ZetKlantId(int id);
        void ZetNaam(string naam);
    }
}