using BusinessLayer.Model;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace BusinessLayer_VoetbaltruitjesWinkel.Repo_Interfaces {
    public interface IKlantRepository {
        Klant GeefKlant(int klantId);
        bool bestaatKlant(Klant klant);
        bool bestaatKlant(int id);
        IReadOnlyList<Klant> GeefKlanten(string naam, string adres);
        void updateKlant(Klant klant);
        void verwijderKlant(Klant klant);
        void voegKlantToe(Klant klant);
        bool heeftKlantBestellingen(Klant klant);
    }
}
