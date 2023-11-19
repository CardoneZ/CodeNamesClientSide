using CodeNamesClientSide.CodeNamesService;
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
    /// Lógica de interacción para JoinRoomWindow.xaml
    /// </summary>
    public partial class JoinRoomWindow : Window
    {
        private MusicManager musicManager;
        private int idPlayer = Utilities.Player.PlayerClient.Idplayer;

        public JoinRoomWindow()
        {
            InitializeComponent();
            musicManager = new MusicManager("Media/Music/SolvingTheCrimeFaster.wav");
            musicManager.PlayMusic();
        }

        private void BtnJoinRoom_Click(object sender, RoutedEventArgs e)
        {
            if (!String.IsNullOrWhiteSpace(TbIdRoom.Text))
            {
                GameBoardSettings gameBoardSettings = new GameBoardSettings(idPlayer);
                {
                    WindowState = this.WindowState;
                    Left = this.Left;
                    gameBoardSettings.IsNewRoom = false;
                    gameBoardSettings.RoomId = TbIdRoom.Text;
                };
                if (gameBoardSettings.CheckQuota())
                {
                    if (gameBoardSettings.CreateNewRoom(false))
                    {
                        gameBoardSettings.Show();
                        this.Close();
                    }
                    else
                    {
                        gameBoardSettings.Close();
                    }
                }
                else
                {
                    MessageBox.Show(Properties.Resources.JoinRoom_CantJoin_Message, Properties.Resources.GeneralWarning, MessageBoxButton.OK, MessageBoxImage.Information);
                }
            }
            else
            {
                MessageBox.Show(Properties.Resources.JoinRoom_NoCode_Message, Properties.Resources.GeneralWarning, MessageBoxButton.OK, MessageBoxImage.Information);
            }
        }

        private void BtnGoBack_Click(object sender, RoutedEventArgs e)
        {
            MainMenuWindow mainMenuWindow = new MainMenuWindow(idPlayer);
            {
                WindowState = this.WindowState;
                Left = this.Left;
            };
            mainMenuWindow.Show();
            this.Close();
        }
    }
}

