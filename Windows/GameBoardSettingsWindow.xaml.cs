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
        private List<string> chatMessages = new List<string>();

        public bool IsNewRoom { get { return isNewRoom; } set { isNewRoom = value; } }
        public string RoomId { get { return roomId; } set { roomId = value; } }

        public GameBoardSettings(MusicManager manager, int idPlayer)
        {
            InitializeComponent();
            LibChat.ItemsSource = chatMessages;
            musicManager = manager;
            musicManager = new MusicManager("Media/Music/SolvingTheCrimeFaster.wav");
            musicManager.PlayMusic();
            this.idPlayer = idPlayer;


        }

        private void ListBox_SelectionChanged(object sender, SelectionChangedEventArgs e)
        {

        }

        public bool CreateNewRoom(bool isNewRoom)
        {
            var status = true;

            this.isNewRoom = isNewRoom;
            if (isNewRoom)
            {
                BtnStartGame.Visibility = Visibility.Visible;

            }
            else
            {
                BtnStartGame.Visibility = Visibility.Collapsed;
            }

            try
            {
                Start();
            }
            catch (EndpointNotFoundException ex)
            {
                status = false;
                Console.WriteLine(ex.Message);

                MessageBox.Show(Properties.Resources.Exception_NoConnection_Message, Properties.Resources.Exception_NoConnection_Title, MessageBoxButton.OK, MessageBoxImage.Error);

            }
            catch (CommunicationObjectFaultedException ex)
            {
                status = false;
                Console.WriteLine(ex.Message);

                MessageBox.Show(Properties.Resources.Exception_NoConnection_Message, Properties.Resources.Exception_NoConnection_Title, MessageBoxButton.OK, MessageBoxImage.Error);
            }
            catch (TimeoutException ex)
            {
                status = false;
                Console.WriteLine(ex.Message);
                MessageBox.Show(Properties.Resources.Exception_NoConnection_Message, Properties.Resources.Exception_NoConnection_Title, MessageBoxButton.OK, MessageBoxImage.Error);

            }
            return status;
        }

        public bool CheckQuota()
        {
            gameManagerServiceClient = new GameManagerServiceClient(new InstanceContext(this));
            var available = false;

            try
            {
                available = gameManagerServiceClient.CheckQuota(RoomId);
            }
            catch (EndpointNotFoundException ex)
            {
                Console.WriteLine(ex.Message);
                MessageBox.Show(Properties.Resources.Exception_NoConnection_Message, Properties.Resources.Exception_NoConnection_Title, MessageBoxButton.OK, MessageBoxImage.Error);
            }
            catch (CommunicationObjectFaultedException ex)
            {
                Console.WriteLine(ex.Message);

                MessageBox.Show(Properties.Resources.Exception_NoConnection_Message, Properties.Resources.Exception_NoConnection_Title, MessageBoxButton.OK, MessageBoxImage.Error);
            }
            catch (TimeoutException ex)
            {
                Console.WriteLine(ex.Message);
                MessageBox.Show(Properties.Resources.Exception_NoConnection_Message, Properties.Resources.Exception_NoConnection_Title, MessageBoxButton.OK, MessageBoxImage.Error);

            }
            finally
            {
                gameManagerServiceClient.Abort();
            }

            return available;
        }

        public void MessageCallBack(string message)
        {
            chatMessages.Add(message);
            LibChat.Items.Refresh();

        }

        private void Start()
        {
            if (!isConected)
            {
                gameManagerServiceClient = new GameManagerServiceClient(new InstanceContext(this));
                if (isNewRoom)
                {
                    roomId = gameManagerServiceClient.GenerateRoomCode();
                    gameManagerServiceClient.CreateRoom(Utilities.Player.PlayerClient.Username, roomId);
                    isHost = true;
                    TbRoomCode.Text = roomId;
                }
                gameManagerServiceClient.Connect(Utilities.Player.PlayerClient.Username, roomId, " Un nuevo usuario se ha unido a la sala");
                isConected = true;
            }
        }

        private void End()
        {
            if (isConected)
            {
                gameManagerServiceClient.Disconnect(Utilities.Player.PlayerClient.Username, roomId, "Un usuario ha dejado la sala");
                gameManagerServiceClient.Abort();
                gameManagerServiceClient = null;
                isConected = false;
            }
        }

        private void BtnCopyRoomCode_Click(object sender, RoutedEventArgs e)
        {
            Clipboard.SetText(roomId);
            MessageBox.Show(Properties.Resources.Copy_RoomCode_Message, Properties.Resources.Copy_RoomCode_Title, MessageBoxButton.OK, MessageBoxImage.Information);

        }

        private void BtnStartGame_Click(object sender, RoutedEventArgs e)
        {
            GameBoard game = new GameBoard();
            game.Show();
            this.Close();
        }

        private void BtnLeaveRoom_Click(object sender, RoutedEventArgs e)
        {
            End();
            MainMenuWindow mainMenuWindow = new MainMenuWindow(idPlayer);
            mainMenuWindow.Show();
            this.Close();
        }

        private void BtnSendMessage_Click(object sender, RoutedEventArgs e)
        {
            String message = TbMessage.Text;
            if (!string.IsNullOrEmpty(message))
            {
                TbMessage.Clear();
                gameManagerServiceClient = new GameManagerServiceClient(new InstanceContext(this));
                gameManagerServiceClient.SendMessage(message, Utilities.Player.PlayerClient.Username, RoomId);

            }
        }

        private void ButtonSpyRed_Click(object sender, RoutedEventArgs e)
        {

        }

        private void ButtonOperatorRed_Click(object sender, RoutedEventArgs e)
        {

        }

        private void ButtonSpyBlue_Click(object sender, RoutedEventArgs e)
        {

        }

        private void ButtonOperatorBlue_Click(object sender, RoutedEventArgs e)
        {

        }
    }
}
