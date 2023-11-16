using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace CodeNamesClientSide.Utilities
{
    public class Player
    {
        private int idPlayer;
        private string username;
        private string password;
        private string email;
        private bool isGuest;

        #region Singletone

        private static Player playerClient;

        public static Player PlayerClient { get { return playerClient; } set { playerClient = value; } }

        #endregion

        public int Idplayer { get { return idPlayer; } set { idPlayer = value; } }
        public string Username { get { return username; } set { username = value; } }
        public string Password { get { return password; } set { password = value; } }
        public string Email { get { return email; } set { email = value; } }
        public bool IsGuest { get { return isGuest; } set { isGuest = value; } }

    }
}
