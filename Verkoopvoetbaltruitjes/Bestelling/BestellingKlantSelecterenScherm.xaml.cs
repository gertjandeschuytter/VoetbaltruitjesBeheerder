using System;
using System.Collections.Generic;
using System.Collections.ObjectModel;
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
    /// Interaction logic for BestellingKlantSelecterenScherm.xaml
    /// </summary>
    public partial class BestellingKlantSelecterenScherm : Window {
        private BusinessLayer.Model.Bestelling _geselecteerdeBestellingUpdate = (BusinessLayer.Model.Bestelling)Application.Current.Properties["GeselecteerdeBestellingenUpdate"];

        public BestellingKlantSelecterenScherm() {
            InitializeComponent();
        }
        private void SearchBtn_Click(object sender, RoutedEventArgs e) {
            try {
                int? idValue = null;
                string? naamValue = null;
                string? adresValue = null;

                if (int.TryParse(Id.Text, out int id)) {
                    idValue = id;
                }
                if (!string.IsNullOrEmpty(Name.Text)) {
                    naamValue = Name.Text;
                }
                if (!string.IsNullOrEmpty(Address.Text)) {
                    adresValue = Address.Text;
                }
                IReadOnlyList<BusinessLayer.Model.Klant> klanten = MainWindow.klantBeheerder.ZoekKlanten(idValue, naamValue, adresValue);
                ObservableCollection<BusinessLayer.Model.Klant> ts = new();
                foreach (BusinessLayer.Model.Klant klant in klanten) {
                    ts.Add(klant);
                }
                ListViewCustomers.ItemsSource = ts;
            } catch (Exception ex) {
                MessageBox.Show(ex.Message, Title, MessageBoxButton.OK, MessageBoxImage.Error);
            }
        }

        private void BackBtn_Click(object sender, RoutedEventArgs e) {
            BestellingToevoegenScherm bt = new();
            bt.Show();
            this.Close();
        }

        private void SelectBtn_Click(object sender, RoutedEventArgs e) {
            try {
                if (ListViewCustomers.SelectedItem != null) {
                    BusinessLayer.Model.Klant klant = (BusinessLayer.Model.Klant)ListViewCustomers.SelectedItem;
                    Application.Current.Properties["Klant"] = klant;
                    if (_geselecteerdeBestellingUpdate != null) {
                        _geselecteerdeBestellingUpdate.ZetKlant(klant);
                        Application.Current.Properties["GeselecteerdeBestellingenUpdate"] = _geselecteerdeBestellingUpdate;
                    }
                    BestellingToevoegenScherm bt = new();
                    bt.Show();
                    this.Close();
                } else {
                    MessageBox.Show("Er is geen klant geselecteerd", Title, MessageBoxButton.OK, MessageBoxImage.Warning);
                }
            } catch (Exception ex) {
                MessageBox.Show(ex.Message, Title, MessageBoxButton.OK, MessageBoxImage.Error);
            }
        }
    }
}
