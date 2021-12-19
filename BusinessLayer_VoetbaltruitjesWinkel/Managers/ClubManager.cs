using BusinessLayer_VoetbaltruitjesWinkel.ExceptionsForManagers;
using BusinessLayer_VoetbaltruitjesWinkel.Repo_Interfaces;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace BusinessLayer_VoetbaltruitjesWinkel.Managers {
    public class ClubManager {
        private IClubRepository repo;
        public ClubManager(IClubRepository repo) {
            this.repo = repo;
        }
        public bool BestaatCompetitie(string competitie) {
            if (!repo.GeefCompetities().Contains(competitie)) return false;
            else return true;
        }
        public IReadOnlyList<string> GeefCompetities() {
            try {
                return repo.GeefCompetities();
            } catch (Exception ex) {
                throw new ClubManagerException("GeefCompetities", ex);
            }
        }
        public IReadOnlyList<string> GeefClubs(string competitie) {
            try {
                if (!string.IsNullOrWhiteSpace(competitie)) {
                    if (repo.BestaatCompetitie(competitie)) {
                        return repo.GeefClubs(competitie);
                    } else throw new ClubManagerException("GeefClubs - competitie bestaat niet");
                } else throw new ClubManagerException("GeefClubs - competitienaam is leeg");
            } catch (Exception ex) {
                throw new ClubManagerException("GeefClubs", ex);
            }
        }
    }
}

