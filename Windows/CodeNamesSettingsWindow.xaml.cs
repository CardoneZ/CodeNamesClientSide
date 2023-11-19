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

namespace CodeNamesClientSide.Windows
{
    /// <summary>
    /// Interaction logic for CodeNamesSettingsWindow.xaml
    /// </summary>
    public partial class CodeNamesSettingsWindow : Window
    {
        private MusicManager musicManager;
        private int idPlayer;
        public CodeNamesSettingsWindow(int idPlayer)
        {
            InitializeComponent();
            Goback.MouseLeftButtonDown += Goback_MouseLeftButtonDown;
            musicManager.PlayMusic();
            this.idPlayer = idPlayer;
        }

        private void ToggleMusicButton_Click(object sender, RoutedEventArgs e)
        {
            musicManager.ToggleMusic();
        }

        private void Goback_MouseLeftButtonDown(object sender, MouseButtonEventArgs e)
        {
            MainMenuWindow mainMenulWindow = new MainMenuWindow(idPlayer);

            mainMenulWindow.Show();

            this.Close();
            musicManager.StopMusic();
            base.OnClosed(e);
        }

        private void VolumeSlider_ValueChanged(object sender, RoutedPropertyChangedEventArgs<double> e)
        {
            if (musicManager != null)
            {
                musicManager.Volume = (float)volumeSlider.Value;
            }
        }
    }
}
