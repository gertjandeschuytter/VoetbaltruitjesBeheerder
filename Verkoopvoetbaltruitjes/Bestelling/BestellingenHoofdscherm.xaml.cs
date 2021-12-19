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

namespace Verkoopvoetbaltruitjes.Bestelling {
    /// <summary>
    /// Interaction logic for BestellingenHoofdscherm.xaml
    /// </summary>
    public partial class BestellingenHoofdscherm : Window {
        public BestellingenHoofdscherm() {
            InitializeComponent();
        }
        private void VoegNieuweBestellingToe_Click(object sender, RoutedEventArgs e) {
            BestellingToevoegenScherm toevoegScherm = new BestellingToevoegenScherm();
            toevoegScherm.Show();
            this.Close();
        }

        private void ZoekBestelling_Click(object sender, RoutedEventArgs e) {
            BestellingZoekenScherm zoekscherm = new BestellingZoekenScherm();
            zoekscherm.Show();
            this.Close();
        }
        private void GaTerugNaarHome_Click(object sender, RoutedEventArgs e) {
            MainWindow mainWindow = new MainWindow();
            mainWindow.Show();
            this.Close();
        }
    }
}
