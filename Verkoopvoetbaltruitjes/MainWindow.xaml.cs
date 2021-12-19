using BusinessLayer.Model;
using BusinessLayer_VoetbaltruitjesWinkel.DATALAYER;
using BusinessLayer_VoetbaltruitjesWinkel.Managers;
using System;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.ComponentModel;
using System.Configuration;
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
using System.Windows.Navigation;
using System.Windows.Shapes;
using System.Windows.Threading;
using Verkoopvoetbaltruitjes.Bestelling;
using Verkoopvoetbaltruitjes.Klant;
using Verkoopvoetbaltruitjes.Voetbaltruitje;

namespace Verkoopvoetbaltruitjes {
    /// <summary>
    /// Interaction logic for MainWindow.xaml
    /// </summary>
    public partial class MainWindow : Window {
        public string connectionString;
        public static KlantManager klantBeheerder;
        public static VoetbaltruitjeManager voetbaltruitjeBeheerder;
        public static BestellingManager bestellingBeheerder;
        public static ClubManager clubBeheerder;

        public MainWindow() {
            this.ResizeMode = ResizeMode.NoResize;
            InitializeComponent();
            connectionString = ConfigurationManager.ConnectionStrings["connectionString"].ConnectionString;
            klantBeheerder = new(new KlantRepositoryADO(connectionString));
            voetbaltruitjeBeheerder = new(new VoetbaltruitjeRepositoryADO(connectionString));
            bestellingBeheerder = new(new BestellingRepositoryADO(connectionString));
            clubBeheerder = new(new ClubRepositoryADO(connectionString));
        }

            void timer_Tick(object sender, EventArgs e) {
                lbltime.Content = DateTime.Now.ToString("HH:mm:ss");
        }

        private void GoToVoetbaltruitjeWindow_Click(object sender, RoutedEventArgs e) {
            VoetbaltruitjeHoofdScherm nw = new VoetbaltruitjeHoofdScherm();
            nw.Show();
            this.Close();
        }
        private void GoToKlantWindow_Click(object sender, RoutedEventArgs e) {
            KlantHoofdScherm kh = new KlantHoofdScherm();
            kh.Show();
            this.Close();
        }
        private void GoToBestellingWindow_Click(object sender, RoutedEventArgs e) {
            BestellingenHoofdscherm kh = new BestellingenHoofdscherm();
            kh.Show();
            this.Close();
        }
    }
}
