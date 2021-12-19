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

namespace Verkoopvoetbaltruitjes.Bestelling
{
    /// <summary>
    /// Interaction logic for BestellingUpdatenScherm.xaml
    /// </summary>
    public partial class BestellingUpdatenScherm : Window
    {
        private BusinessLayer.Model.Bestelling _geselecteerdeBestellingUpdate = (BusinessLayer.Model.Bestelling)Application.Current.Properties["GeselecteerdeBestellingenUpdate"];

        public BestellingUpdatenScherm()
        {
            InitializeComponent();
        }
        private void Page_Loaded(object sender, RoutedEventArgs e)
        {
            if (_geselecteerdeBestellingUpdate != null)
            {
                Customer.Text = _geselecteerdeBestellingUpdate.Klant.ToText(true);
                IsPayed.IsChecked = _geselecteerdeBestellingUpdate.Betaald;
                Price.Text = _geselecteerdeBestellingUpdate.Prijs.ToString();
            }
        }

        private void SelectCustomerBtn_Click(object sender, RoutedEventArgs e)
        {
            SelecteerKlantVanuitUpdateScherm skv = new();
            skv.Show();
            this.Close();
        }

        private void SelectTruitjeBtn_Click(object sender, RoutedEventArgs e)
        {
            SelecteerTruitjeVanuitUpdateScherm stv = new();
            stv.Show();
            this.Close();
        }

        private void DataGridTruitjes_Loaded(object sender, RoutedEventArgs e)
        {
            if (_geselecteerdeBestellingUpdate != null)
            {
                ObservableCollection<TruitjesData> oc = new();
                if (_geselecteerdeBestellingUpdate.GeefProducten() != null && _geselecteerdeBestellingUpdate.GeefProducten().Count != 0)
                {
                    foreach (var truitje in _geselecteerdeBestellingUpdate.GeefProducten())
                    {
                        oc.Add(new TruitjesData(truitje.Key, truitje.Value));
                    }
                    DataGridTruitjes.ItemsSource = oc;
                    Price_Loaded(sender, e);
                }
                else
                {
                    oc.Clear();
                    DataGridTruitjes.ItemsSource = oc;
                }
            }
        }

        private void DeleteVoetbaltruitje_Click(object sender, RoutedEventArgs e)
        {
            try
            {
                List<TruitjesData> truitjes = new();
                foreach (var item in _geselecteerdeBestellingUpdate.GeefProducten())
                {
                    truitjes.Add(new TruitjesData(item.Key, item.Value));
                }
                TruitjesData x = (TruitjesData)DataGridTruitjes.CurrentItem;
                foreach (var item in truitjes)
                {
                    if (item.Truitje.Equals(x.Truitje) && item.Aantal.Equals(x.Aantal))
                    {
                        truitjes.Remove(item);
                        _geselecteerdeBestellingUpdate.VerwijderProduct(item.Truitje, item.Aantal);
                        break;
                    }
                }
                Application.Current.Properties["GeselecteerdeBestellingenUpdate"] = _geselecteerdeBestellingUpdate;
                MessageBox.Show("Truitje is verwijderd uit de bestelling", Title, MessageBoxButton.OK, MessageBoxImage.Information);
                DataGridTruitjes_Loaded(sender, e);
                Price_Loaded(sender, e);
            }
            catch (Exception ex)
            {
                MessageBox.Show(ex.Message, Title, MessageBoxButton.OK, MessageBoxImage.Error);
            }
        }

        private void CreateOrder_Click(object sender, RoutedEventArgs e)
        {
            try
            {
                MainWindow.bestellingBeheerder.UpdateBestelling(_geselecteerdeBestellingUpdate);
                Application.Current.Properties["GeselecteerdeBestellingenUpdate"] = null;
                Application.Current.Properties["Klant"] = null;
                MessageBox.Show("Bestelling is geüpdate.", Title, MessageBoxButton.OK, MessageBoxImage.Information);
                this.Close();
            }
            catch (Exception ex)
            {
                MessageBox.Show(ex.Message, Title, MessageBoxButton.OK, MessageBoxImage.Error);
            }
        }

        private void Price_Loaded(object sender, RoutedEventArgs e)
        {
            PrijsLaden(DictionaryNaarListTruitjes());
        }

        private List<TruitjesData> DictionaryNaarListTruitjes()
        {
            Dictionary<BusinessLayer.Model.Voetbaltruitje, int> truitjes = new();
            List<TruitjesData> voetbaltruitjesAantals = new();
            if (_geselecteerdeBestellingUpdate != null)
            {
                truitjes = (Dictionary<BusinessLayer.Model.Voetbaltruitje, int>)_geselecteerdeBestellingUpdate.GeefProducten();
                foreach (var truitje in truitjes)
                {
                    voetbaltruitjesAantals.Add(new TruitjesData(truitje.Key, truitje.Value));
                }
            }
            return voetbaltruitjesAantals;
        }

        private void BackBtn_Click(object sender, RoutedEventArgs e)
        {
            Application.Current.Properties["GeselecteerdeBestellingenUpdate"] = null;
            Application.Current.Properties["Klant"] = null;
            this.Close();
        }

        private void DataGridTruitjes_CellEditEnding(object sender, DataGridCellEditEndingEventArgs e)
        {
            TruitjesData v = (TruitjesData)DataGridTruitjes.SelectedItem;
            List<TruitjesData> truitjes = DictionaryNaarListTruitjes();
            var truitje = truitjes.Where(y => y.Truitje == v.Truitje).ToList()[0];
            var element = (TextBox)e.EditingElement;
            truitje.Aantal = int.Parse(element.Text);
            UpdateBestellingTruitjes(truitjes);
            PrijsLaden(DictionaryNaarListTruitjes());
        }

        private void UpdateBestellingTruitjes(List<TruitjesData> truitjes)
        {
            Dictionary<BusinessLayer.Model.Voetbaltruitje, int> keyValuePairs = new();
            foreach (var item in truitjes)
            {
                keyValuePairs.Add(item.Truitje, item.Aantal);
            }
            _geselecteerdeBestellingUpdate.VoegProductenToe(keyValuePairs);
            Application.Current.Properties["GeselecteerdeBestellingenUpdate"] = _geselecteerdeBestellingUpdate;
        }

        private void PrijsLaden(List<TruitjesData> truitjes)
        {
            if (truitjes != null)
            {
                double price = 0;
                foreach (var item in truitjes)
                {
                    price += item.Truitje.Prijs * item.Aantal;
                }
                Price.Text = price.ToString("F2");
            }
        }
    }
}
