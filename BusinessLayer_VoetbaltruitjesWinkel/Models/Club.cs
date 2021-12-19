using System;

namespace BusinessLayer.Model {
    public class Club {
        public string Competitie { get; set; }
        public string Ploeg { get; set; }

        public Club(string competitie, string ploeg) {
            Competitie = competitie;
            Ploeg = ploeg;
        }

        public override bool Equals(object obj) {
            return obj is Club club &&
                   Competitie == club.Competitie &&
                   Ploeg == club.Ploeg;
        }

        public override int GetHashCode() {
            return HashCode.Combine(Competitie, Ploeg);
        }
    }
}