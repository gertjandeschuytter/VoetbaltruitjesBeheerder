using BusinessLayer.Model;
using System;
using System.ComponentModel;

namespace Verkoopvoetbaltruitjes {
    public class TruitjesData {
        public TruitjesData(BusinessLayer.Model.Voetbaltruitje truitje, int aantal) {
            Truitje = truitje;
            Aantal = aantal;
        }
        public BusinessLayer.Model.Voetbaltruitje Truitje { get; private set; }
        public int Aantal { get; set; }
    }
}