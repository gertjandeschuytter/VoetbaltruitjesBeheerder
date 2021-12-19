using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows;
using Verkoopvoetbaltruitjes;
using System.Windows.Controls;
using System.Windows.Data;
using System.Windows.Documents;
using System.Windows.Input;
using System.Windows.Media;
using System.Windows.Media.Imaging;
using System.Windows.Shapes;
using BusinessLayer_VoetbaltruitjesWinkel.Managers;
using System.Configuration;
using BusinessLayer_VoetbaltruitjesWinkel.DATALAYER;

namespace Verkoopvoetbaltruitjes.Klant {
    /// <summary>
    /// Interaction logic for KlantToevoegenScherm.xaml
    /// </summary>
    public partial class KlantToevoegenScherm : Window {
        private string _connectionString;
        private KlantManager _klantManager;
        public KlantToevoegenScherm() {
            InitializeComponent();
            _connectionString = ConfigurationManager.ConnectionStrings["connectionString"].ConnectionString;
        }
        private void AddBtn_Click(object sender, RoutedEventArgs e) {
            try {
                _klantManager = new(new KlantRepositoryADO(_connectionString));
                BusinessLayer.Model.Klant klant = new(Name.Text, Address.Text);
                _klantManager.voegKlantToe(klant);
                MessageBox.Show("Klant is aangemaakt", Title, MessageBoxButton.OK, MessageBoxImage.Information);
            } catch (Exception ex) {
                MessageBox.Show(ex.Message, Title, MessageBoxButton.OK, MessageBoxImage.Error);
            }
        }

        private void GaTerugNaarKlantHoofdscherm_Click(object sender, RoutedEventArgs e) {
            KlantHoofdScherm kh = new();
            this.Close();
            kh.Show();
        }
    }
}
