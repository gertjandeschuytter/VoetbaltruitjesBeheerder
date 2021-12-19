using BusinessLayer.Model;
using BusinessLayer_VoetbaltruitjesWinkel.ExceptionsForManagers;
using BusinessLayer_VoetbaltruitjesWinkel.Repo_Interfaces;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using BusinessLayer_VoetbaltruitjesWinkel.Exceptions;

namespace BusinessLayer_VoetbaltruitjesWinkel.Managers {
    public class BestellingManager {
        //fields
        private IBestellingRepository repo;
        //ctor
        public BestellingManager(IBestellingRepository repo) {
            this.repo = repo;
        }

        public IReadOnlyList<Bestelling> ZoekBestellingen(int? bestellingId, DateTime? startdatum, DateTime? einddatum, Klant? klant) {
            List<Bestelling> bestellingen = new List<Bestelling>();
            try {
                if (bestellingId.HasValue) {
                    if (repo.BestaatBestelling((int)bestellingId)) bestellingen.Add(repo.GeefBestelling(bestellingId.Value));
                } else {
                    bestellingen.AddRange(repo.GeefBestellingen(bestellingId,startdatum, einddatum, klant));
                }
                return bestellingen.AsReadOnly();
            } catch (Exception ex) {
                throw new VoetbaltruitjeManagerException("ZoekTruitjes", ex);
            }
        }
        public bool BestaatBestelling(Bestelling bestelling) {

            if (bestelling.BestellingId <= 0) throw new BestellingException("Bestelling - BestaatBestelling - Bestelling bestaat niet!");
            else
                return true;
        }
        public void UpdateBestelling(Bestelling bestelling)
        {
            try
            {
                if (repo.BestaatBestelling(bestelling.BestellingId))
                {
                    Bestelling dbVoetbalTruitje = repo.GeefBestelling(bestelling.BestellingId);
                    if (dbVoetbalTruitje == bestelling)
                    {
                        throw new VoetbaltruitjeManagerException("UpdateBestelling - geen verschillen");
                    }
                    else
                    {
                        repo.UpdateBestelling(bestelling);
                    }
                }
                else
                {
                    throw new BestellingManagerException("UpdateBestelling - bestaat niet");
                }
            }
            catch (Exception ex)
            {
                throw new BestellingManagerException("UpdateBestelling", ex);
            }
        }

        public void VerwijderBestelling(Bestelling bestelling) {
            try {
                if (!repo.BestaatBestelling(bestelling.BestellingId)) throw new BestellingException("Bestelling - BestellingVerwijderen - Bestelling bestaat niet!");
                else
                    repo.VerwijderBestelling(bestelling);
            } catch (Exception ex) {
                Console.WriteLine(ex.Message);
            }
        }
        public void VoegBestellingToe(Bestelling bestelling) {
            try {
                if (repo.BestaatBestelling(bestelling.BestellingId)) throw new BestellingException("Bestelling - VoegBestellingToe - Bestelling bestaat al");
                else
                    repo.VoegBestellingToe(bestelling);
            } catch (Exception ex) {
                Console.WriteLine(ex.Message);
            }
        }
    }
}

