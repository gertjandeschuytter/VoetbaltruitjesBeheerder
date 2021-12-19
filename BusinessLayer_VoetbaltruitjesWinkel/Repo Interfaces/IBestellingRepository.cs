using BusinessLayer.Model;
using BusinessLayer_VoetbaltruitjesWinkel.ExceptionsForManagers;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace BusinessLayer_VoetbaltruitjesWinkel.Repo_Interfaces {
    public interface IBestellingRepository {
        void VoegBestellingToe(Bestelling bestelling);
        bool BestaatBestelling(int bestellingId);
        IEnumerable<Bestelling> GeefBestellingen(int? BestellingId, DateTime? startdatum, DateTime? einddatum, Klant? klant);
        void VerwijderBestelling(Bestelling bestelling);
        void UpdateBestelling(Bestelling bestelling);
        Bestelling GeefBestelling(int BestellingId);
    }
}
