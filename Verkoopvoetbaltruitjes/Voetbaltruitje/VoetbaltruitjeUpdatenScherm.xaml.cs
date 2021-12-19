using BusinessLayer.Model;
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

namespace Verkoopvoetbaltruitjes.Voetbaltruitje {
    /// <summary>
    /// Interaction logic for VoetbaltruitjeUpdatenScherm.xaml
    /// </summary>
    public partial class VoetbaltruitjeUpdatenScherm : Window {
        public static BusinessLayer.Model.Voetbaltruitje _voetbaltruitje = (BusinessLayer.Model.Voetbaltruitje)Application.Current.Properties["Voetbaltruitje"];

        public VoetbaltruitjeUpdatenScherm() {
            InitializeComponent();
            Seizoen.Text = _voetbaltruitje.Seizoen;
            Versie.Text = _voetbaltruitje.ClubSet.Versie.ToString();
            Thuis.IsChecked = _voetbaltruitje.ClubSet.Thuis;
            Prijs.Text = _voetbaltruitje.Prijs.ToString();
        }
        private void ComboBoxCompetitie_SelectionChanged(object sender, SelectionChangedEventArgs e) {
            if (ComboBoxCompetitie.SelectedIndex != 0) {
                ObservableCollection<string> ploegen = new(MainWindow.clubBeheerder.GeefClubs(ComboBoxCompetitie.SelectedItem.ToString()));
                ploegen.Insert(0, "<geen club>");
                ComboBoxPloeg.ItemsSource = ploegen;
                ComboBoxPloeg.SelectedIndex = 0;
            } else {
                ComboBoxPloeg.ItemsSource = null;
            }
        }

        private void ComboBoxCompetitie_Loaded(object sender, RoutedEventArgs e) {
            ObservableCollection<string> competities = new(MainWindow.clubBeheerder.GeefCompetities());
            competities.Insert(0, "<geen competitie>");
            ComboBoxCompetitie.SelectedIndex = 0;
            ComboBoxCompetitie.ItemsSource = competities;
            ComboBoxCompetitie.SelectedValue = _voetbaltruitje.Club.Competitie;
        }

        private void ComboBoxMaat_Loaded(object sender, RoutedEventArgs e) {
            ObservableCollection<string> maten = new();
            maten.Insert(0, "<geen maat>");
            maten.Insert(1, "S");
            maten.Insert(2, "M");
            maten.Insert(3, "L");
            maten.Insert(4, "XL");
            ComboBoxMaat.ItemsSource = maten;
            ComboBoxMaat.SelectedIndex = 0;
            ComboBoxMaat.SelectedValue = _voetbaltruitje.Kledingmaat.ToString();
        }

        private void ComboBoxPloeg_Loaded(object sender, RoutedEventArgs e) {
            ComboBoxPloeg.SelectedValue = _voetbaltruitje.Club.Ploeg;
        }

        private void UpdateBtn_Click(object sender, RoutedEventArgs e) {
            try {
                string competitie = ComboBoxCompetitie.SelectedItem.ToString();
                string ploeg = ComboBoxPloeg.SelectedItem.ToString();
                string seizoen = Seizoen.Text;
                bool thuis = _voetbaltruitje.ClubSet.Thuis;
                Kledingmaat kledingmaat;
                if (ComboBoxCompetitie.SelectedIndex == 0) {
                    competitie = _voetbaltruitje.Club.Competitie;
                }
                if (ComboBoxPloeg.SelectedIndex == 0) {
                    ploeg = _voetbaltruitje.Club.Ploeg;
                }
                if (!double.TryParse(Prijs.Text, out double prijs)) {
                    prijs = _voetbaltruitje.Prijs;
                }
                if (ComboBoxMaat.SelectedIndex != 0) {
                    kledingmaat = (Kledingmaat)Enum.Parse(typeof(Kledingmaat), ComboBoxMaat.SelectedItem.ToString());
                } else {
                    kledingmaat = _voetbaltruitje.Kledingmaat;
                }
                if (!int.TryParse(Versie.Text, out int versie)) {
                    versie = _voetbaltruitje.ClubSet.Versie;
                }
                if (string.IsNullOrWhiteSpace(Seizoen.Text)) {
                    seizoen = _voetbaltruitje.Seizoen;
                }
                if (Thuis.IsChecked == false) {
                    thuis = false;
                }
                Club club = new(competitie, ploeg);
                ClubSet clubSet = new(thuis, versie);
                BusinessLayer.Model.Voetbaltruitje voetbaltruitje = new(_voetbaltruitje.Id, club, seizoen, prijs, kledingmaat, clubSet);
                MainWindow.voetbaltruitjeBeheerder.UpdateVoetbaltruitje(voetbaltruitje);
                MessageBox.Show("Voetbaltruitje is bijgewerkt, refresh om de zaken opnieuw in te laden", Title, MessageBoxButton.OK, MessageBoxImage.Information); 
                this.Close();
            } catch (Exception ex) {
                MessageBox.Show(ex.Message, Title, MessageBoxButton.OK, MessageBoxImage.Error);
            }
        }

        private void GoBack_Update_Click(object sender, RoutedEventArgs e) {
            VoetbaltruitjeZoekenScherm Zs = new();
            Zs.Show();
            this.Close();
        }
    }

}
