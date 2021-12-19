using BusinessLayer.Model;
using BusinessLayer_VoetbaltruitjesWinkel.Exceptions;
using BusinessLayer_VoetbaltruitjesWinkel.ExceptionsForManagers;
using BusinessLayer_VoetbaltruitjesWinkel.Repo_Interfaces;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace BusinessLayer_VoetbaltruitjesWinkel.Managers {
    public class KlantManager {
        private IKlantRepository repo;

        public KlantManager(IKlantRepository repo) {
            this.repo = repo;
        }

        public IReadOnlyList<Klant> ZoekKlanten(int? klantId, string? naam, string? adres) {
            List<Klant> klanten = new List<Klant>();
            try {
                if (klantId.HasValue) {
                    if (repo.bestaatKlant((int)klantId)) klanten.Add(repo.GeefKlant((int)klantId));
                } else {
                    klanten.AddRange(repo.GeefKlanten(naam, adres));
                }
                return klanten;
            } catch (Exception ex) {
                throw new VoetbaltruitjeManagerException("zoekKlanten - er iets foutgelopen", ex);
            }
        }
        public Klant GeefKlant(int id) {
            Klant klant = null;
            try {
                if (repo.bestaatKlant(id)) {
                    klant = repo.GeefKlant(id);
                } else {
                    throw new KlantManagerException("klant bestaat nog niet dus kan ook niet opgevraagd worden");
                }
                return klant;
            } catch (Exception ex) {
                throw new KlantManagerException("KlantManager - " + ex.Message);
            }
        }
        public void updateKlant(Klant klant) {
            try {
                if (repo.bestaatKlant(klant.KlantId)) {
                    Klant DBKlant = repo.GeefKlant(klant.KlantId);
                    if (DBKlant == klant) {
                        throw new KlantManagerException("geen verschillen tussen nieuwe en oude klant");
                    } else {
                        repo.updateKlant(klant);
                    }
                } else {
                    throw new KlantManagerException("klant bestaat niet");
                }
            } catch (Exception ex) {
                throw new KlantManagerException("updateKlant - " + ex.Message);
            }
        }
        public void verwijderKlant(Klant klant) {
            try {
                if (klant == null) throw new KlantException("Klant is null!");
                if (!repo.bestaatKlant(klant)) throw new KlantException("bestaat niet!");
                else
                    repo.verwijderKlant(klant);
            } catch (Exception ex) {
                throw new KlantManagerException("KlantManager - VerwijderKlant - " + ex.Message);
            }
        }
        public void voegKlantToe(Klant klant) {
            try {
                if (klant == null) {
                    throw new KlantManagerException("Klant is null.");
                }
                if (repo.bestaatKlant(klant.KlantId)) {
                    throw new KlantManagerException("Klant bestaat al.");
                }
                repo.voegKlantToe(klant);
            } catch (Exception ex) {
                throw new KlantManagerException("KlantManager - VoegKlantToe - " + ex.Message);
            }
        }
    }
}

