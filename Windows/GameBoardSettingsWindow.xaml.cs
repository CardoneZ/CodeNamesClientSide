using CodeNamesClientSide.CodeNamesService;
using System;
using System.CodeDom;
using System.Collections.Generic;
using System.Linq;
using System.ServiceModel;
using System.ServiceModel.Channels;
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
using static System.Runtime.CompilerServices.RuntimeHelpers;

namespace CodeNamesClientSide.Windows
{
    /// <summary>
    /// Interaction logic for GameBoardSettings.xaml
    /// </summary>
    public partial class GameBoardSettings : Window,IGameManagerServiceCallback
    {
        private int idPlayer;
        private MusicManager musicManager;
        private CodeNamesService.Player[] playerList;
        private GameManagerServiceClient gameManagerServiceClient;
        private bool isNewRoom;
        private string roomId;
        private bool isConected = false;
        private bool isHost = false;
        public bool IsNewRoom { get { return isNewRoom; } set { isNewRoom = value; } }
        public string RoomId { get { return roomId; } set { roomId = value; } }

        private List<string> chatMessages = new List<string>();

        public GameBoardSettings(MusicManager manager, int idPlayer)
        {
            InitializeComponent();
            LibChat.ItemsSource = chatMessages;
            musicManager = manager;
            Goback.MouseLeftButtonDown += Goback_MouseLeftButtonDown;
            musicManager = new MusicManager("Media/Music/SolvingTheCrimeFaster.wav");
            musicManager.PlayMusic();
            this.idPlayer = idPlayer;
        }


        private void ListBox_SelectionChanged(object sender, SelectionChangedEventArgs e)
        {

        }

        private void TextBox_TextChanged(object sender, TextChangedEventArgs e)
        {

        }

        private void BtnSendMessage_Click(object sender, RoutedEventArgs e)
        {
            String message = TbMessage.Text;
            if (!string.IsNullOrEmpty(message))
            {
                chatMessages.Add(message);
                LibChat.Items.Refresh();
                TbMessage.Clear();
                gameManagerServiceClient = new GameManagerServiceClient(new InstanceContext(this));
                gameManagerServiceClient.SendMessage(message, Utilities.Player.PlayerClient.Username, RoomId);

            }
        }
        public bool CreateNewRoom(bool isNewRoom)
        {
            var status = true;

            this.isNewRoom = isNewRoom;
            if (isNewRoom)
            {
                //btnStartGame.Visibility = Visibility.Visible;
                //gridLobby.Visibility = Visibility.Visible;
            }
            else
            {
                //btnStartGame.Visibility = Visibility.Collapsed;
            }

            try
            {
                Start();
            }
            catch (EndpointNotFoundException ex)
            {
                status = false;
            }
            catch (CommunicationObjectFaultedException ex)
            {
                status = false;
            }
            catch (TimeoutException ex)
            {
                status = false;
            }
            return status;
        }

        public void MessageCallBack(string message)
        {
            chatMessages.Add(message);
            LibChat.Items.Refresh();

        }

        private void Goback_MouseLeftButtonDown(object sender, MouseButtonEventArgs e)
        {
            MainMenuWindow mainMenulWindow = new MainMenuWindow(idPlayer);

            mainMenulWindow.Show();

            this.Close();
            musicManager.StopMusic();
            base.OnClosed(e);
        }

        private void Start()
        {
            if (!isConected)
            {
                gameManagerServiceClient = new GameManagerServiceClient(new InstanceContext(this));
                if (isNewRoom)
                {
                    roomId = gameManagerServiceClient.GenerateRoomCode();
                    //txtCode.Text = roomId;
                    gameManagerServiceClient.CreateRoom(Utilities.Player.PlayerClient.Username, roomId);
                    isHost = true;
                }
                //txtCode.Text = roomId;
                gameManagerServiceClient.Connect(Utilities.Player.PlayerClient.Username, roomId, " Un nuevo usuario se ha unido a la sala");
                isConected = true;
            }
        }
    }
}
