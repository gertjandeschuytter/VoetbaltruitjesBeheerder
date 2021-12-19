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
    /// Interaction logic for KlantUpdatenScherm.xaml
    /// </summary>
    public partial class KlantUpdatenScherm : Window {
        private BusinessLayer.Model.Klant _klant = (BusinessLayer.Model.Klant)Application.Current.Properties["SelectedKlant"];
        public KlantUpdatenScherm() {
            InitializeComponent();
            Naam.Text = _klant.Naam;
            Adres.Text = _klant.Adres;
        }
        private void Ga_terug_home(object sender, RoutedEventArgs e) {
            this.Close();
        }

        private void EditBtn_Click(object sender, RoutedEventArgs e) {
            try {
                BusinessLayer.Model.Klant klant = _klant;
                klant.ZetNaam(Naam.Text);
                klant.ZetAdres(Adres.Text);
                MainWindow.klantBeheerder.updateKlant(klant);
                MessageBox.Show("Klant is aangepast", Title, MessageBoxButton.OK, MessageBoxImage.Information);
                this.Close();
            } catch (Exception ex) {
                MessageBox.Show(ex.Message, Title, MessageBoxButton.OK, MessageBoxImage.Information);
            }
        }
    }
}
