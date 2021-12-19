using Exceptionless.Models.Collections;
using NSwag.Collections;
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
    /// Interaction logic for BestellingToevoegenScherm.xaml
    /// </summary>
    public partial class BestellingToevoegenScherm : Window {
        private BusinessLayer.Model.Klant _klant = (BusinessLayer.Model.Klant)Application.Current.Properties["Klant"];
        private BusinessLayer.Model.Klant _klantSave = (BusinessLayer.Model.Klant)Application.Current.Properties["SavedKlant"];
        public List<TruitjesData> _truitjes = (List<TruitjesData>)Application.Current.Properties["Truitjes"];
        public BestellingToevoegenScherm() {
            InitializeComponent();
        }
        private void SelectCustomerBtn_Click(object sender, RoutedEventArgs e) {
            BestellingKlantSelecterenScherm ks = new();
            ks.Show();
            this.Close();
        }

        private void SelectTruitjeBtn_Click(object sender, RoutedEventArgs e) {
            BestellingVoetbaltruitjeSelecterenScherm ks = new();
            ks.Show();
            this.Close();
        }

        private void CreateOrder_Click(object sender, RoutedEventArgs e) {
            try {
                List<TruitjesData> voetbaltruitjes = DataGridTruitjes.Items.OfType<TruitjesData>().ToList();
                bool betaald = false;
                double prijs = 0;
                if (IsPayed.IsChecked != false) {
                    betaald = true;
                }
                if (double.TryParse(Price.Text, out double prijs2)) {
                    prijs = prijs2;
                }
                if (Customer.Text != null && _truitjes != null && _truitjes.Count != 0 && _klantSave != null) {
                    Dictionary<BusinessLayer.Model.Voetbaltruitje, int> truitjes = new();
                    foreach (var item in voetbaltruitjes) {
                        truitjes.Add(item.Truitje, item.Aantal);
                    }
                    BusinessLayer.Model.Bestelling bestelling = new(_klantSave, DateTime.Now, betaald, truitjes, prijs);
                    MainWindow.bestellingBeheerder.VoegBestellingToe(bestelling);
                    Application.Current.Properties["Truitjes"] = null;
                    Customer.Text = null;
                    Price.Text = null;
                    IsPayed.IsChecked = false;
                    _truitjes.Clear();
                    DataGridTruitjes_Loaded(sender, e);
                    MessageBox.Show("Bestelling geplaatst", Title, MessageBoxButton.OK, MessageBoxImage.Information);
                } else {
                    throw new Exception();
                }
            } catch (Exception ex) {
                MessageBox.Show(ex.Message, Title, MessageBoxButton.OK, MessageBoxImage.Error);
            }
        }

        private void Page_Loaded(object sender, RoutedEventArgs e) {
            if (_klant != null) {
                Customer.Text = _klant.ToText(true);
                Application.Current.Properties["SavedKlant"] = _klant;
                _klantSave = _klant;
            }
        }

        private void Price_Loaded(object sender, RoutedEventArgs e) {
            PrijsLaden();
        }

        private void DeleteVoetbaltruitje_Click(object sender, RoutedEventArgs e) {
            try {
                var voetbaltruitje = (TruitjesData)DataGridTruitjes.CurrentItem;
                foreach (var item in _truitjes) {
                    if (item.Truitje.Equals(voetbaltruitje.Truitje) && item.Aantal.Equals(voetbaltruitje.Aantal)) {
                        _truitjes.Remove(item);
                        break;
                    }
                }
                Application.Current.Properties["Truitjes"] = _truitjes;
                MessageBox.Show("Truitje is verwijderd uit de bestelling", Title, MessageBoxButton.OK, MessageBoxImage.Information);
                DataGridTruitjes_Loaded(sender, e);
                Price_Loaded(sender, e);
            } catch (Exception ex) {
                MessageBox.Show(ex.Message, Title, MessageBoxButton.OK, MessageBoxImage.Error);
            }
        }

        private void DataGridTruitjes_Loaded(object sender, RoutedEventArgs e) {
            var collection = new ObservableCollection<TruitjesData>();

            if (_truitjes != null && _truitjes.Count != 0) {
                foreach (var truitje in _truitjes) {
                    collection.Add(truitje);
                }
                DataGridTruitjes.ItemsSource = collection;
                Price_Loaded(sender, e);
            } else {
                collection.Clear();
                DataGridTruitjes.ItemsSource = collection;
            }
        }

        private void DataGridTruitjes_CellEditEnding(object sender, DataGridCellEditEndingEventArgs e) {
            TruitjesData v = (TruitjesData)DataGridTruitjes.SelectedItem;
            var truitje = _truitjes.Where(y => y.Truitje == v.Truitje).ToList()[0];
            var element = (TextBox)e.EditingElement;
            truitje.Aantal = int.Parse(element.Text);
            PrijsLaden();
        }

        private void PrijsLaden() {
            if (_truitjes != null) {
                double price = 0;
                foreach (var item in _truitjes) {
                    price += item.Truitje.Prijs * item.Aantal;
                }
                Price.Text = price.ToString("F2");
            }
        }

    }
}

