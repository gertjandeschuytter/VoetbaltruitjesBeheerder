using BusinessLayer.Model;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace BusinessLayer_VoetbaltruitjesWinkel.Repo_Interfaces {
    public interface IVoetbaltruitjeRepository {
        bool BestaatVoetbaltruitje(Voetbaltruitje truitje);
        void VoegVoetbaltruitjeToe(Voetbaltruitje truitje);
        void VerwijderVoetbaltruitje(Voetbaltruitje voetbaltruitje);
        void UpdateVoetbaltruitje(Voetbaltruitje truitje);
        IEnumerable<Voetbaltruitje> GeefVoetbaltruitjes(string competitie, string club, string seizoen, string kledingmaat, int? versie, bool? thuis, double? prijs);
        bool BestaatVoetbaltruitje(int voetbaltruitjeId);
        Voetbaltruitje GeefVoetbaltruitje(int voetbaltruitjeId);
    }
}
