using BusinessLayer_VoetbaltruitjesWinkel.Exceptions;
using System;

namespace BusinessLayer.Model {
    public class ClubSet {
        public int Versie { get; set; }
        public bool Thuis { get; set; }

        public ClubSet(bool thuis, int versie) {
            Thuis = thuis;
            if (versie < 1) throw new VoetbaltruitjeException("Clubset - versie < 1");
            Versie = versie;
        }
        //vanaf dat je begint met vergelijken gebruik je equals/hashcode
        public override bool Equals(object obj) {
            return obj is ClubSet set &&
                   Versie == set.Versie &&
                   Thuis == set.Thuis;
        }
        public override int GetHashCode() {
            return HashCode.Combine(Versie, Thuis);
        }
    }
}