using BusinessLayer.Model;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace BusinessLayer_VoetbaltruitjesWinkel.Tools {
    public static class KlantFactory {
        public static Klant MaakKlant(string naam, string adres) {
            try {
                return new Klant(naam.Trim(), adres.Trim());
            } catch (Exception ex) {
                throw new KlantFactoryException("KlantFactory: MaakKlant - methode gefaald!", ex);
            }
        }
        public static Klant MaakKlant(int id, string naam, string adres) {
            try {
                return new Klant(id, naam.Trim(), adres.Trim());
            } catch (Exception ex) {
                throw new KlantFactoryException("KlantFactory: MaakKlantMetId - methode gefaald!", ex);
            }
        }
        public static Klant MaakKlantUpper(string naam,string adres) {
            try {
                return new Klant( naam.Trim(), adres.Trim());
            } catch (Exception ex) {
                throw new KlantFactoryException("KlantFactory: MaakKlantUpper - methode gefaald!", ex);
            }
        }
    }
}
