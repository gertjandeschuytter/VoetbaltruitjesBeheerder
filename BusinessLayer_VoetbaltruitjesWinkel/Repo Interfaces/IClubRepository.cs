using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace BusinessLayer_VoetbaltruitjesWinkel.Repo_Interfaces {
    public interface IClubRepository {
        IReadOnlyList<string> GeefClubs(string competitie);
        IReadOnlyList<string> GeefCompetities();
        bool BestaatCompetitie(string competitie);
    }
}
