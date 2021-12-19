using System;
using System.Collections.Generic;
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
    /// Interaction logic for VoetbaltruitjeHoofdScherm.xaml
    /// </summary>
    public partial class VoetbaltruitjeHoofdScherm : Window {
        public VoetbaltruitjeHoofdScherm() {
            this.ResizeMode = ResizeMode.NoResize;
            InitializeComponent();
        }

        private void VoegNieuwVoetbaltruitjeToe_Click(object sender, RoutedEventArgs e) {
            VoetbaltruitjeToevoegenScherm mainWindow = new VoetbaltruitjeToevoegenScherm();
            mainWindow.Show();
            this.Close();
        }

        private void ZoekVoetbaltruitje_Click(object sender, RoutedEventArgs e) {
            VoetbaltruitjeZoekenScherm zoekenScherm = new VoetbaltruitjeZoekenScherm();
            zoekenScherm.Show();
            this.Close();
        }
        private void GaTerugNaarHome_Click(object sender, RoutedEventArgs e) {
            MainWindow mainWindow = new MainWindow();
            mainWindow.Show();
            this.Close();
        }
    }
}
