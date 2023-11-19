using CodeNamesClientSide.CodeNamesService;
using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Linq;
using System.Runtime.Remoting.Contexts;
using System.ServiceModel;
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
    /// Interaction logic for MainMenuWindow.xaml
    /// </summary>
    public partial class MainMenuWindow : Window, IFriendListServiceCallback
    {
        private int idPlayer;
        CodeNamesService.FriendListServiceClient client;
        private MusicManager musicManager;

        

        public MainMenuWindow(int idPlayer)
        {
            InitializeComponent();
            InstanceContext context = new InstanceContext(this); 
            client = new CodeNamesService.FriendListServiceClient(context);
            client.UpdatePlayerSession(idPlayer);
            Settings.MouseLeftButtonDown += Settings_MouseLeftButtonDown;
            Profile.MouseLeftButtonDown += Profile_MouseLeftButtonDown;
            this.idPlayer = idPlayer;
            Closing += MainWindow_Closing;
        }
        private void MainWindow_Closing(object sender, System.ComponentModel.CancelEventArgs e)
        {
            InstanceContext context = new InstanceContext(this);
            CodeNamesService.FriendListServiceClient clientFriend = new CodeNamesService.FriendListServiceClient(context);
            clientFriend.RemovePlayerSession(idPlayer);
        }

        private void Settings_MouseLeftButtonDown(object sender, MouseButtonEventArgs e)
        {
            CodeNamesSettingsWindow codeNamesSettingsWindow = new CodeNamesSettingsWindow(idPlayer);
            codeNamesSettingsWindow.Show();
            this.Close();
            base.OnClosed(e);
        }

        private void BtnSocial_Click(object sender, RoutedEventArgs e)
        {
            SocialWindow socialWindow = new SocialWindow(idPlayer);
            socialWindow.Show();
            this.Close();
            base.OnClosed(e);
        }

        private void BtnJoinGame_Click(object sender, RoutedEventArgs e)
        {
            JoinRoomWindow joinRoomWindow = new JoinRoomWindow();
            joinRoomWindow.Show();
            this.Close();
            base.OnClosed(e);
        }

        private void BtnCreateGame_Click(object sender, RoutedEventArgs e)
        {
            GameBoardSettings gameBoardSettingsWindow = new GameBoardSettings(idPlayer);
            if (gameBoardSettingsWindow.CreateNewRoom(true))
            {
                gameBoardSettingsWindow.Show();
                this.Close();
            }
            MusicManager.MusicClient.StopMusic();
            base.OnClosed(e);
        }

        private void BtnExitGame_Click(object sender, RoutedEventArgs e)
        {
            InstanceContext context = new InstanceContext(this);
            CodeNamesService.FriendListServiceClient clientFriend = new CodeNamesService.FriendListServiceClient(context);
            clientFriend.RemovePlayerSession(idPlayer);
            Environment.Exit(0);
        }

        private void Profile_MouseLeftButtonDown(object sender, MouseButtonEventArgs e)
        {
            UserProfileWindow userProfileWindow = new UserProfileWindow();

            userProfileWindow.Show();

            this.Close();

            MusicManager.MusicClient.StopMusic();
            base.OnClosed(e);
        }

        public void NotifyFriendRequest(int idPlayer)
        {
            throw new NotImplementedException();
        }

        public void NotifyFriendOnline(int idPlayer)
        {
            throw new NotImplementedException();
        }

        public void UpdateFriendRequest(int idPlayer)
        {
            throw new NotImplementedException();
        }

        public void UpdateFriendDisplay()
        {
            throw new NotImplementedException();
        }
    }
}
