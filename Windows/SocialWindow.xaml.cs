using CodeNamesClientSide.CodeNamesService;
using CodeNamesClientSide.Utilities;
using ControlzEx.Controls;
using System;
using System.Collections.Generic;
using System.Linq;
using System.ServiceModel;
using System.Text;
using System.Threading.Tasks;
using System.Web.UI;
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
    /// Interaction logic for SocialWindow.xaml
    /// </summary>
    public partial class SocialWindow : Window, CodeNamesService.IFriendListServiceCallback
    {
        private int idPlayer;
        private MusicManager musicManager;
        private CodeNamesService.PlayerManagerServiceClient playerService;
        private CodeNamesService.FriendListServiceClient sessionService;

        public SocialWindow(int idPlayer)
        {
            InitializeComponent();
            musicManager = new MusicManager("Media/Music/BackgroundCheck.wav");
            musicManager.PlayMusic();
            GoBack.MouseLeftButtonDown += GoBack_MouseLeftButtonDown;

            // Initialize PlayerClient and PlayerManagerClient
            InstanceContext context = new InstanceContext(this);
            playerService = new CodeNamesService.PlayerManagerServiceClient(context);
            sessionService = new CodeNamesService.FriendListServiceClient(context);
            playerService.UpdatePlayerSession(idPlayer);

            ShowFriends(idPlayer);
        }
        private void GoBack_MouseLeftButtonDown(object sender, MouseButtonEventArgs e)
        {
            MainMenuWindow mainMenulWindow = new MainMenuWindow(idPlayer);

            mainMenulWindow.Show();

            this.Close();
            musicManager.StopMusic();
            base.OnClosed(e);
        }

        public void NotifyFriendOnline(int idPlayer)
        {
            throw new NotImplementedException();
        }

        public void NotifyFriendRequest(int idPlayer)
        {
            throw new NotImplementedException();
        }

        public void ShowFriends(int idPlayer)
        {
            FriendListDisplay.Children.Clear();

            Dictionary<int,object> onlineUsers = playerService.GetCurrentUsers();

            foreach (var friend in sessionService.GetFriends(idPlayer))
            {
                Border border = new Border
                {
                    Width = 600,
                    Height = 60,
                    Background = Brushes.LightBlue,
                    Margin = new Thickness(2),
                };

                Grid grid = new Grid();

                Ellipse ellipse = new Ellipse
                {
                    Width = 40,
                    Height = 40,
                    Margin = new Thickness(10, 10, 0, 0),
                };

                if (friend.IsOnline)
                {
                    ellipse.Fill = Brushes.Green;
                }
                else
                {
                    ellipse.Fill = Brushes.Red;
                }

                Label label = new Label
                {
                    Content = friend.Nickname,
                    FontSize = 28,
                    VerticalAlignment = VerticalAlignment.Center,
                    Margin = new Thickness(60, 0, 0, 0),
                };

                grid.Children.Add(ellipse);
                grid.Children.Add(label);
                border.Child = grid;
                FriendListDisplay.Children.Add(border);
            }
        }

        public void UpdateFriendDisplay()
        {
            ShowFriends(idPlayer);
        }

        public void UpdateFriendRequest(int idPlayer)
        {
            AddRequest(idPlayer);
        }

        private void AddRequest(int IdPlayer)
        {
            RequestList.Children.Clear();
            foreach (var request in sessionService.GetFriendshipRequests(IdPlayer))
            {
                Border border = new Border
                {
                    Width = 650,
                    Height = 60,
                    Background = Brushes.LightBlue,
                    Margin = new Thickness(2),
                };

                Grid grid = new Grid();

                Label label = new Label
                {
                    Content = request.SenderPlayer,
                    FontSize = 28,
                    Margin = new Thickness(10, 6, 0, 0),
                };

                Button acceptButton = new Button
                {
                    Content = "Aceptar",
                    Width = 80,
                    Height = 20,
                    Margin = new Thickness(100, 10, 0, 0),
                };

                Button rejectButton = new Button
                {
                    Content = "Rechazar",
                    Width = 80,
                    Height = 20,
                    Margin = new Thickness(300, 10, 0, 0),
                };

                acceptButton.Click += (sender, e) => {
                    sessionService.AcceptFriendRequest(request.IdFriendshipRequest);
                    AddRequest(IdPlayer);
                    ShowFriends(IdPlayer);
                };

                rejectButton.Click += (sender, e) => {
                    sessionService.RejectFriendRequest(request.IdFriendshipRequest);
                    AddRequest(IdPlayer);
                    ShowFriends(IdPlayer);
                };

                grid.Children.Add(label);
                grid.Children.Add(acceptButton);
                grid.Children.Add(rejectButton);
                border.Child = grid;
                RequestList.Children.Add(border);
            }
        }

        private void BtnGoBack_Click(object sender, RoutedEventArgs e)
        {
            MainMenuWindow mainMenuWindow = new MainMenuWindow(idPlayer);

            mainMenuWindow.Show();
            this.Close();
            musicManager.StopMusic();
            base.OnClosed(e);
        }

    }
}
