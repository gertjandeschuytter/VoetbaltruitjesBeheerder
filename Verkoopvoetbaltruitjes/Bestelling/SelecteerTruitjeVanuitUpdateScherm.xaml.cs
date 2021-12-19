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
    /// Interaction logic for SelecteerTruitjeVanuitUpdateScherm.xaml
    /// </summary>
    public partial class SelecteerTruitjeVanuitUpdateScherm : Window
    {
        public List<TruitjesData> _voetbaltruitjesAantals { get; set; } = new();
        private BusinessLayer.Model.Bestelling _geselecteerdeBestellingUpdate = (BusinessLayer.Model.Bestelling)Application.Current.Properties["GeselecteerdeBestellingenUpdate"];

        public SelecteerTruitjeVanuitUpdateScherm()
        {
            InitializeComponent();
        }
        private void GaTerugNaarUpdateScherm_Click(object sender, RoutedEventArgs e)
        {
            this.Close();
        }
        
        private void ComboBoxCompetitie_SelectionChanged(object sender, SelectionChangedEventArgs e)
        {
            if (ComboBoxCompetitie.SelectedIndex != 0)
            {
                ObservableCollection<string> ploegen = new(MainWindow.clubBeheerder.GeefClubs(ComboBoxCompetitie.SelectedItem.ToString()));
                ploegen.Insert(0, "<geen club>");
                ComboBoxPloeg.ItemsSource = ploegen;
                ComboBoxPloeg.SelectedIndex = 0;
            }
            else
            {
                ComboBoxPloeg.ItemsSource = null;
            }
        }

        private void ComboBoxCompetitie_Loaded(object sender, RoutedEventArgs e)
        {
            ObservableCollection<string> competities = new(MainWindow.clubBeheerder.GeefCompetities());
            competities.Insert(0, "<geen competitie>");
            ComboBoxCompetitie.SelectedIndex = 0;
            ComboBoxCompetitie.ItemsSource = competities;
        }

        private void ComboBoxMaat_Loaded(object sender, RoutedEventArgs e)
        {
            ObservableCollection<string> maten = new();
            maten.Insert(0, "<geen maat>");
            maten.Insert(1, "S");
            maten.Insert(2, "M");
            maten.Insert(3, "L");
            maten.Insert(4, "XL");
            ComboBoxMaat.ItemsSource = maten;
            ComboBoxMaat.SelectedIndex = 0;
        }

        private void SearchBtn_Click(object sender, RoutedEventArgs e)
        {
            try
            {
                string competitie = "";
                string ploeg = "";
                double? prijs = null;
                bool? thuis = null;
                string maat = "";
                int? valueId = null;
                int? valueVersie = null;

                if (int.TryParse(Id.Text, out int id))
                {
                    valueId = id;
                }
                if (ComboBoxCompetitie.SelectedIndex != 0 && ComboBoxCompetitie != null)
                {
                    competitie = ComboBoxCompetitie.SelectedItem.ToString();
                }
                if (ComboBoxPloeg.SelectedIndex != 0 && ComboBoxPloeg != null && ComboBoxPloeg.Items.Count != 0)
                {
                    ploeg = ComboBoxPloeg.SelectedItem.ToString();
                }
                if (double.TryParse(Prijs.Text, out double prijs2))
                {
                    prijs = prijs2;
                }
                if (int.TryParse(Versie.Text, out int versie))
                {
                    valueVersie = versie;
                }
                if (Thuis.IsChecked == true)
                {
                    thuis = true;
                }
                if (Uit.IsChecked == true)
                {
                    thuis = false;
                }
                if (Thuis.IsChecked == Uit.IsChecked)
                {
                    thuis = null;
                }
                if (ComboBoxMaat.SelectedIndex != 0 && ComboBoxMaat != null)
                {
                    maat = ComboBoxMaat.SelectedItem.ToString();
                }
                IReadOnlyList<BusinessLayer.Model.Voetbaltruitje> voetbaltruitjes = MainWindow.voetbaltruitjeBeheerder.ZoekTruitjes(valueId, competitie, ploeg, Seizoen.Text, maat, valueVersie, thuis, prijs);
                ObservableCollection<BusinessLayer.Model.Voetbaltruitje> ts = new();
                foreach (var voetbaltruitje in voetbaltruitjes)
                {
                    ts.Add(voetbaltruitje);
                }
                ListViewTruitjes.ItemsSource = ts;
            }
            catch (Exception ex)
            {
                MessageBox.Show(ex.Message, Title, MessageBoxButton.OK, MessageBoxImage.Error);
            }
        }

        private void SelectBtn_Click(object sender, RoutedEventArgs e)
        {
            try
            {
                if (_geselecteerdeBestellingUpdate == null)
                {
                    if (ListViewTruitjes.SelectedItem != null)
                    {
                        BusinessLayer.Model.Voetbaltruitje voetbaltruitje = (BusinessLayer.Model.Voetbaltruitje)ListViewTruitjes.SelectedItem;
                        bool zitItemAlInLijst = false;
                        foreach (var item in _voetbaltruitjesAantals)
                        {
                            if (item.Truitje.Equals(voetbaltruitje))
                            {
                                zitItemAlInLijst = true;
                            }
                        }
                        if (!zitItemAlInLijst)
                        {
                            _voetbaltruitjesAantals.Add(new TruitjesData(voetbaltruitje, 1));
                            Application.Current.Properties["Truitjes"] = _voetbaltruitjesAantals;
                            Application.Current.Properties["GeselecteerdeBestellingenUpdate"] = _geselecteerdeBestellingUpdate;
                            BestellingUpdatenScherm bt = new();
                            bt.Show();
                            this.Close();
                        }
                        else
                        {
                            foreach (var item in _voetbaltruitjesAantals)
                            {
                                if (item.Truitje.Equals(voetbaltruitje))
                                {
                                    item.Aantal++;
                                }
                            }
                            Application.Current.Properties["Truitjes"] = _voetbaltruitjesAantals;
                            Application.Current.Properties["GeselecteerdeBestellingenUpdate"] = _geselecteerdeBestellingUpdate;
                            BestellingUpdatenScherm bt = new();
                            bt.Show();
                            this.Close();
                        }
                    }
                    else
                    {
                        MessageBox.Show("Er is geen truitje geselecteerd", Title, MessageBoxButton.OK, MessageBoxImage.Warning);
                    }
                }
                else
                {
                    Dictionary<BusinessLayer.Model.Voetbaltruitje, int> truitjes = (Dictionary<BusinessLayer.Model.Voetbaltruitje, int>)_geselecteerdeBestellingUpdate.GeefProducten();
                    if (ListViewTruitjes.SelectedItem != null)
                    {
                        BusinessLayer.Model.Voetbaltruitje voetbaltruitje = (BusinessLayer.Model.Voetbaltruitje)ListViewTruitjes.SelectedItem;
                        if (!truitjes.ContainsKey(voetbaltruitje))
                        {
                            truitjes.Add(voetbaltruitje, 1);
                            _geselecteerdeBestellingUpdate.VoegProductenToe(truitjes);
                            Application.Current.Properties["GeselecteerdeBestellingenUpdate"] = _geselecteerdeBestellingUpdate;
                            BestellingUpdatenScherm bt = new();
                            bt.Show();
                            this.Close();
                        }
                        else
                        {
                            truitjes.TryGetValue(voetbaltruitje, out int value);
                            truitjes[voetbaltruitje] = value + 1;
                            _geselecteerdeBestellingUpdate.VoegProductenToe(truitjes);
                            Application.Current.Properties["GeselecteerdeBestellingenUpdate"] = _geselecteerdeBestellingUpdate;
                            BestellingUpdatenScherm bt = new();
                            bt.Show();
                            this.Close();
                        }
                    }
                    else
                    {
                        MessageBox.Show("Er is geen truitje geselecteerd", Title, MessageBoxButton.OK, MessageBoxImage.Warning);
                    }
                }

            }
            catch (Exception ex)
            {
                MessageBox.Show(ex.Message, Title, MessageBoxButton.OK, MessageBoxImage.Error);
            }
        }

        private void Page_Loaded(object sender, RoutedEventArgs e)
        {
            if (Application.Current.Properties["Truitjes"] != null)
            {
                _voetbaltruitjesAantals = (List<TruitjesData>)Application.Current.Properties["Truitjes"];
            }
        }
    }
}
