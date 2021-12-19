using BusinessLayer.Model;
using BusinessLayer_VoetbaltruitjesWinkel.ExceptionsForManagers;
using BusinessLayer_VoetbaltruitjesWinkel.Repo_Interfaces;
using System;
using System.Collections.Generic;

namespace BusinessLayer_VoetbaltruitjesWinkel.Managers {
    public class VoetbaltruitjeManager {
        //fields
        private IVoetbaltruitjeRepository repo;
        //ctor
        public VoetbaltruitjeManager(IVoetbaltruitjeRepository repo) {
            this.repo = repo;

        }
        //methods (in orde)
        public IReadOnlyList<Voetbaltruitje> ZoekTruitjes(int? voetbaltruitjeId, string competitie, string club, string seizoen, string kledingmaat, int? versie, bool? thuis, double? prijs)  {
            List<Voetbaltruitje> voetbaltruitjes = new List<Voetbaltruitje>();
            try {
                if (voetbaltruitjeId.HasValue) {
                    if (repo.BestaatVoetbaltruitje((int)voetbaltruitjeId)) voetbaltruitjes.Add(repo.GeefVoetbaltruitje(voetbaltruitjeId.Value));
                } else {
                        voetbaltruitjes.AddRange(repo.GeefVoetbaltruitjes(competitie, club, seizoen, kledingmaat, versie, thuis, prijs));
                    }
                    return voetbaltruitjes;
            } catch (Exception ex) {
                throw new VoetbaltruitjeManagerException("ZoekTruitjes", ex);
            }
        }
        public void VoegTruitjeToe(Voetbaltruitje truitje) {
            try {
                if (repo.BestaatVoetbaltruitje(truitje)) {
                    throw new VoetbaltruitjeManagerException("VoegTruitjeToe - bestaat al");
                } else {
                    repo.VoegVoetbaltruitjeToe(truitje);
                }
            } catch (Exception ex) {
                throw new VoetbaltruitjeManagerException("VoegTruitjeToe", ex);
            }
        }
        public void VerwijderVoetbaltruitje(Voetbaltruitje truitje) {
            try {
                if (!repo.BestaatVoetbaltruitje(truitje)) {
                    throw new VoetbaltruitjeManagerException("VerwijderVoetbaltruitje");
                } else {
                    repo.VerwijderVoetbaltruitje(truitje);
                }
            } catch (Exception ex) {
                throw new VoetbaltruitjeManagerException("VerwijderVoetbaltruitje", ex);
            }
        }
        public void UpdateVoetbaltruitje(Voetbaltruitje truitje) {
            try {
                if (repo.BestaatVoetbaltruitje(truitje.Id)) {
                    Voetbaltruitje dbVoetbalTruitje = repo.GeefVoetbaltruitje(truitje.Id);
                    if (dbVoetbalTruitje == truitje) {
                        throw new VoetbaltruitjeManagerException("UpdateVoetbaltruitje - geen verschillen");
                    } else {
                        repo.UpdateVoetbaltruitje(truitje);
                    }
                } else {
                    throw new VoetbaltruitjeManagerException("UpdateVoetbaltruitje - bestaat niet");
                }
            } catch (Exception ex) {
                throw new VoetbaltruitjeManagerException("UpdateVoetbaltruitje", ex);
            }
        }
    }
}
