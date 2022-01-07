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
    /// Interaction logic for VoetbaltruitjeToevoegenScherm.xaml
    /// </summary>
    public partial class VoetbaltruitjeToevoegenScherm : Window {
        public VoetbaltruitjeToevoegenScherm() {
            InitializeComponent();
        }
        private void Goback_Click(object sender, RoutedEventArgs e) {
            VoetbaltruitjeHoofdScherm mw = new();
            mw.Show();
            this.Close();
        }
        private void UpdateBtn_Click(object sender, RoutedEventArgs e) {
            try {
                bool thuis = true;
                bool isOk = true;
                if (Thuis.IsChecked == null || Thuis.IsChecked == false) {
                    thuis = false;
                }
                if (ComboBoxCompetitie.SelectedIndex == 0 || ComboBoxMaat.SelectedIndex == 0 || ComboBoxPloeg.SelectedIndex == 0 || string.IsNullOrWhiteSpace(Seizoen.Text)
                    || string.IsNullOrWhiteSpace(Prijs.Text) || string.IsNullOrWhiteSpace(Versie.Text)) {
                    isOk = false;
                }
                if (isOk) {
                    Club club = new(ComboBoxCompetitie.SelectedItem.ToString(), ComboBoxPloeg.SelectedItem.ToString());
                    ClubSet clubSet = new(thuis, int.Parse(Versie.Text));
                    BusinessLayer.Model.Voetbaltruitje voetbaltruitje = new(club, Seizoen.Text, double.Parse(Prijs.Text), (Kledingmaat)Enum.Parse(typeof(Kledingmaat), ComboBoxMaat.SelectedItem.ToString()), clubSet);
                    MainWindow.voetbaltruitjeBeheerder.VoegTruitjeToe(voetbaltruitje);
                    MessageBox.Show("Voetbaltuitje is aangemaakt", Title, MessageBoxButton.OK, MessageBoxImage.Information);
                } else {
                    throw new Exception("Controleer of alles wel correct is ingevuld/aangeduid");
                }
            } catch (Exception ex) {
                MessageBox.Show(ex.Message, Title, MessageBoxButton.OK, MessageBoxImage.Error);
            }
        }
        private void ComboBoxCompetitie_Loaded(object sender, RoutedEventArgs e) {
            ObservableCollection<string> competities = new(MainWindow.clubBeheerder.GeefCompetities());
            competities.Insert(0, "<geen competitie>");
            ComboBoxCompetitie.SelectedIndex = 0;
            ComboBoxCompetitie.ItemsSource = competities;
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
        private void ComboBoxMaat_Loaded(object sender, RoutedEventArgs e) {
            ObservableCollection<string> maten = new();
            maten.Insert(0, "<geen maat>");
            maten.Insert(1, "S");
            maten.Insert(2, "M");
            maten.Insert(3, "L");
            maten.Insert(4, "XL");
            ComboBoxMaat.ItemsSource = maten;
            ComboBoxMaat.SelectedIndex = 0;
        }

    }
}
