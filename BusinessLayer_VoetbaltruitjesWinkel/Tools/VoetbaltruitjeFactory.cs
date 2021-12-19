using BusinessLayer.Model;
using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Linq;
using System.Reflection;
using System.Text;
using System.Threading.Tasks;

namespace BusinessLayer_VoetbaltruitjesWinkel.Tools {
    //public static class VoetbaltruitjeFactory {
    //    public static Voetbaltruitje MaakVoetbalTrui(string competitie, string ploeg, string seizoen, double prijs, Kledingmaat kledingmaat, bool thuis, int versie) {
    //        try {

    //            return new Voetbaltruitje(new Club(competitie, ploeg), seizoen, prijs, (kledingmaat)Enum.Parse(typeof(Kledingmaat));
    //        } catch (VoetbalTruitjeFactoryException ex) {
    //            throw new VoetbalTruitjeFactoryException("VoetbaltruitjeFactory: MaakVoetbalTrui - methode is gefaald!", ex);
    //        }
    //    }

    //    public static Trui MaakVoetbalTrui(int id, string competitie, string ploeg, string seizoen, double prijs, Maat kledingmaat, bool thuis, int versie) {
    //        try {
    //            return new Voetbaltruitje(new Club(competitie, ploeg), seizoen, prijs, GetDescription(kledingmaat));
    //        } catch (VoetbalTruitjeFactoryException ex) {
    //            throw new VoetbalTruitjeFactoryException("VoetbaltruitjeFactoryMetId: MaakVoetbalTrui - methode is gefaald!", ex);
    //        }
    //    }


    //    //Method parse ENUM => String
    //    public static string GetDescription(this Enum e) {
    //        Type eType = e.GetType();
    //        string eName = Enum.GetName(eType, e);
    //        if (eName != null) {
    //            FieldInfo fieldInfo = eType.GetField(eName);
    //            if (fieldInfo != null) {
    //                DescriptionAttribute descriptionAttribute =
    //                       Attribute.GetCustomAttribute(fieldInfo,
    //                         typeof(DescriptionAttribute)) as DescriptionAttribute;
    //                if (descriptionAttribute != null) {
    //                    return descriptionAttribute.Description;
    //                }
    //            }
    //        }
    //        return null;
    //    }




    //}
}
