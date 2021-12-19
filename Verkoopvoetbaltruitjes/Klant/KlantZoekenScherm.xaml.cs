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

namespace Verkoopvoetbaltruitjes.Klant {
    /// <summary>
    /// Interaction logic for KlantZoekenScherm.xaml
    /// </summary>
    public partial class KlantZoekenScherm : Window {
        public KlantZoekenScherm() {
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
                if (!string.IsNullOrEmpty(Adres.Text)) {
                    adresValue = Adres.Text;
                }
                IReadOnlyList<BusinessLayer.Model.Klant> klanten = MainWindow.klantBeheerder.ZoekKlanten(idValue,naamValue,adresValue);
                ObservableCollection<BusinessLayer.Model.Klant> ts = new();
                foreach (BusinessLayer.Model.Klant klant in klanten) {
                    ts.Add(klant);
                }
                DataGridCustomers.ItemsSource = ts;
            } catch (Exception ex) {
                MessageBox.Show(ex.Message, Title, MessageBoxButton.OK, MessageBoxImage.Error);
            }
        }

        private void UpdateCustomer_Click(object sender, RoutedEventArgs e) {
            try {
                Application.Current.Properties["SelectedKlant"] = (BusinessLayer.Model.Klant)DataGridCustomers.CurrentItem;
                KlantUpdatenScherm ku = new();
                ku.ShowDialog();
                SearchBtn_Click(sender, e);
            } catch (Exception ex) {
                MessageBox.Show(ex.Message, Title, MessageBoxButton.OK, MessageBoxImage.Error);
            }
        }

        private void DeleteCustomer_Click(object sender, RoutedEventArgs e) {
            try {
                BusinessLayer.Model.Klant klant = (BusinessLayer.Model.Klant)DataGridCustomers.CurrentItem;
                MainWindow.klantBeheerder.verwijderKlant(klant);
                MessageBox.Show("Klant is verwijderd", Title, MessageBoxButton.OK, MessageBoxImage.Information);
                SearchBtn_Click(sender, e);
            } catch (Exception ex) {
                MessageBox.Show(ex.Message, Title, MessageBoxButton.OK, MessageBoxImage.Error);
            }
        }
        private void Ga_terug_home(object sender, RoutedEventArgs e) {
            KlantHoofdScherm kh = new();
            kh.Show();
            this.Close();
        }
    }
}
