using BusinessLayer_VoetbaltruitjesWinkel.Exceptions;
using System;
using System.Collections.Generic;

namespace BusinessLayer.Model {
    public class Voetbaltruitje : IVoetbaltruitje, IEquatable<Voetbaltruitje> {
        public int Id { get; private set; }
        public Club Club { get; private set; }
        public string Seizoen { get; set; }
        public double Prijs { get; private set; }
        public Kledingmaat Kledingmaat { get; set; }
        public ClubSet ClubSet { get; private set; }

        public Voetbaltruitje(int id, Club club, string seizoen, double prijs, Kledingmaat kledingMaat, ClubSet clubSet) {
            ZetId(id);
            ZetClub(club);
            ZetSeizoen(seizoen);
            ZetPrijs(prijs);
            Kledingmaat = kledingMaat;
            ZetClubSet(clubSet);
        }
        public Voetbaltruitje(Club club, string seizoen, double prijs, Kledingmaat kledingMaat, ClubSet clubSet) {
            ZetClub(club);
            ZetSeizoen(seizoen);
            ZetPrijs(prijs);
            Kledingmaat = kledingMaat;
            ZetClubSet(clubSet);
        }

        public void ZetId(int shirtId) {
            if (shirtId <= 0) throw new VoetbaltruitjeException("Voetbaltruitje - invalid id");
            this.Id = shirtId;
        }
        public void ZetPrijs(double verkoopprijs) {
            if (verkoopprijs <= 0) throw new VoetbaltruitjeException("Voetbaltruitje - invalid prijs");
            this.Prijs = verkoopprijs;
        }
        public void ZetClub(Club club) {
            if(club == null) throw new VoetbaltruitjeException("voetbaltruitje - invalid club");
            this.Club = club; 
        }
        public void ZetClubSet(ClubSet clubset) {
            if (clubset == null) throw new VoetbaltruitjeException("Voebaltruitje - invalid ClubSet");
            this.ClubSet = clubset;
        }
        public void ZetSeizoen(string seizoen) {
            if (string.IsNullOrEmpty(seizoen)) throw new VoetbaltruitjeException("Voetbaltruitje - Seizoen is leeg");
            this.Seizoen = seizoen;
        }
        public override string ToString() {
            string thuis = "Uit";
            if (ClubSet.Thuis == true) {
                thuis = "Thuis";
            }
            return $"{Id} - {Club.Competitie} - {Club.Ploeg} - {Seizoen} - Prijs: €{Prijs} - {Kledingmaat} - {thuis} - {ClubSet.Versie}";
        }

        public override bool Equals(object obj) {
            return Equals(obj as Voetbaltruitje);
        }

        public bool Equals(Voetbaltruitje other) {
            return other != null &&
                   Id == other.Id &&
                   EqualityComparer<Club>.Default.Equals(Club, other.Club) &&
                   Seizoen == other.Seizoen &&
                   Prijs == other.Prijs &&
                   Kledingmaat == other.Kledingmaat &&
                   EqualityComparer<ClubSet>.Default.Equals(ClubSet, other.ClubSet);
        }

        public override int GetHashCode() {
            return HashCode.Combine(Id, Club, Seizoen, Prijs, Kledingmaat, ClubSet);
        }

        public static bool operator ==(Voetbaltruitje left, Voetbaltruitje right) {
            return EqualityComparer<Voetbaltruitje>.Default.Equals(left, right);
        }

        public static bool operator !=(Voetbaltruitje left, Voetbaltruitje right) {
            return !(left == right);
        }
    }
}