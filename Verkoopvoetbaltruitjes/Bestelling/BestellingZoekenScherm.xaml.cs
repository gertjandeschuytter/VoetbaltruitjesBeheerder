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
    /// Interaction logic for BestellingZoekenScherm.xaml
    /// </summary>
    public partial class BestellingZoekenScherm : Window {
        private BusinessLayer.Model.Klant _klant = (BusinessLayer.Model.Klant)Application.Current.Properties["Klant"];
        private BusinessLayer.Model.Klant _klantSave;
        public BestellingZoekenScherm() {
            InitializeComponent();
        }
        private void SearchBtn_Click(object sender, RoutedEventArgs e) {
            try {
                DateTime? start = null;
                DateTime? end = null;
                int? valueId = null;
                if (int.TryParse(Id.Text, out int id))
                {
                    valueId = id;
                }
                if (StartDate.SelectedDate != null) {
                    start = StartDate.SelectedDate;
                }
                if (EndDate.SelectedDate != null) {
                    end = EndDate.SelectedDate;
                }
                IReadOnlyList<BusinessLayer.Model.Bestelling> bestellingen = MainWindow.bestellingBeheerder.ZoekBestellingen(valueId, start, end, _klantSave);
                ObservableCollection<BusinessLayer.Model.Bestelling> ts = new();
                foreach (BusinessLayer.Model.Bestelling bestelling in bestellingen) {
                    ts.Add(bestelling);
                }
                ListViewOrders.ItemsSource = ts;
            } catch (Exception ex) {
                MessageBox.Show(ex.Message, Title, MessageBoxButton.OK, MessageBoxImage.Error);
            }
        }

        private void SelectTruitjeBtn_Click(object sender, RoutedEventArgs e) {
            BestellingKlantSelecterenVanuitUpdateScherm ks = new();
            ks.Show();
            this.Close();
        }

        private void UpdateVoetbaltruitje_Click(object sender, RoutedEventArgs e) {
            Application.Current.Properties["GeselecteerdeBestellingenUpdate"] = (BusinessLayer.Model.Bestelling)ListViewOrders.SelectedItem;
            BestellingUpdatenScherm bu = new();
            bu.ShowDialog();
            SearchBtn_Click(sender, e);
        }

        private void DeleteVoetbaltruitje_Click(object sender, RoutedEventArgs e) {
            try {
                BusinessLayer.Model.Bestelling bestelling = (BusinessLayer.Model.Bestelling)ListViewOrders.SelectedItem;
                MainWindow.bestellingBeheerder.VerwijderBestelling(bestelling);
                MessageBox.Show("Bestelling is verwijderd", Title, MessageBoxButton.OK, MessageBoxImage.Information);
                SearchBtn_Click(sender, e);
            } catch (Exception ex) {
                MessageBox.Show(ex.Message, Title, MessageBoxButton.OK, MessageBoxImage.Error);
            }
        }

        private void Customer_Loaded(object sender, RoutedEventArgs e) {
            if (_klant != null) {
                if (_klantSave != _klant && _klantSave != null) {
                    _klantSave = _klant;
                    Customer.Text = _klantSave.ToString();
                } else {
                    _klantSave = _klant;
                    Customer.Text = _klant.ToString();
                }
            }
        }
        private void Ga_terug_home(object sender, RoutedEventArgs e)
        {
            BestellingenHoofdscherm bh = new();
            bh.Show();
            this.Close();
        }
    }
}
