using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows;
using System.Windows.Controls;
using System.Windows.Data;
using System.Windows.Documents;
using System.Windows.Input;
using System.Windows.Media;
using System.Windows.Media.Imaging;
using System.Windows.Shapes;

namespace Verkoopvoetbaltruitjes.Klant {
    /// <summary>
    /// Interaction logic for KlantHoofdScherm.xaml
    /// </summary>
    public partial class KlantHoofdScherm : Window {
        public KlantHoofdScherm() {
            InitializeComponent();
        }

        private void VoegNieuweKlantToe_Click(object sender, RoutedEventArgs e) {
            KlantToevoegenScherm zoekenScherm = new KlantToevoegenScherm();
            zoekenScherm.Show();
            this.Close();
        }
        private void ZoekKlant_Click(object sender, RoutedEventArgs e) {
            KlantZoekenScherm zoekenScherm = new KlantZoekenScherm();
            zoekenScherm.Show();
            this.Close();
        }
        private void GaTerugNaarHomeVanKlantHoofdscherm_Click(object sender, RoutedEventArgs e) {
            MainWindow mainWindow = new MainWindow();
            mainWindow.Show();
            this.Close();
        }
    }
}
