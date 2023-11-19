using System;
using System.Collections.Generic;
using System.Linq;
using System.ServiceModel;
using System.ServiceModel.Configuration;
using System.Media;
using System.Text;
using System.Text.RegularExpressions;
using System.Threading.Tasks;
using System.Windows;
using System.Windows.Controls;
using System.Windows.Data;
using System.Windows.Documents;
using System.Windows.Input;
using System.Windows.Media;
using System.Windows.Media.Imaging;
using System.Windows.Shapes;
using System.Xml.Linq;
using CodeNamesClientSide.CodeNamesService;
using System.Net;

namespace CodeNamesClientSide.Windows
{
    /// <summary>
    /// Interaction logic for SignUpWindow.xaml
    /// </summary>
    public partial class SignUpWindow : Window
    {
        private MusicManager musicManager;

        String username = "";
        String email = "";
        String password = "";
        String confirmationPassword = "";

        public SignUpWindow()
        {
            InitializeComponent();
            //musicManager = MusicManager.MusicClient;
            //MusicManager.MusicClient.PlayMusic();

        }
        private void BtnSignUp_Click(object sender, RoutedEventArgs e)
        {
               SignUpRequest();

        }

        private void SignUpRequest()
        {
            username = TbNickname.Text;
            email = TbEmail.Text;
            string password = new NetworkCredential(string.Empty, PbPassword.SecurePassword).Password;
            string confirmationPassword = new NetworkCredential(string.Empty, PbConfirmPassword.SecurePassword).Password;


            CodeNamesService.PlayerManagerServiceClient client = new CodeNamesService.PlayerManagerServiceClient();


            if (CheckFields())
            {
                string hashedPassword = Utilities.PasswordEncryptor.HashPassword(password);
                client.AddUserAccountToDatabase(username, email, hashedPassword);
                MessageBox.Show("El registro del usuario se ha realizado con éxito", "Registro Exitoso");
                TbEmail.Clear();
                PbPassword.Clear();
                TbNickname.Clear();
                PbConfirmPassword.Clear();

            }
        }

        private void BtnCancelSignUp_Click(object sender, RoutedEventArgs e)
        {
            LogInWindow logInWindow = new LogInWindow();
            this.Close();
            logInWindow.Show();
            base.OnClosed(e);

        }

        private void TbPassword_TextChanged(object sender, TextChangedEventArgs e)
        {

        }

        #region SignUpValidations

        private bool CheckFields()
        {
            bool validFields = true;
            if (!CheckEmptyFields() || !CheckExcessLength() || !CheckCharactersValidity(username)|| !CheckPassword() || !CheckPasswordsMatch()
                || !CheckEmailValidity() )
            {
                validFields = false;
            }
            return validFields;
        }
        private bool CheckEmailValidity()
        {
            string pattern = @"^\w+([-+.']\w+)*@(gmail.com|hotmail.com|outlook.com|estudiantes.uv.mx)$";
            bool isValid = Regex.IsMatch(email, pattern, RegexOptions.IgnoreCase);

            if (!isValid)
            {
                MessageBox.Show("La dirección de correo electrónico no es válida", "Error en el correo electrónico", MessageBoxButton.OK);
            }

            return isValid;
        }


        private bool CheckCharactersValidity(string text)
        {
            string pattern = "^[A-Za-zÁÉÍÓÚáéíóúñÑ\\s]+$";
            bool isValid = Regex.IsMatch(text, pattern, RegexOptions.IgnoreCase);

            if (!isValid)
            {
                MessageBox.Show("La cadena contiene caracteres inválidos", "Error en la cadena", MessageBoxButton.OK);
            }

            return isValid;
        }


        private bool CheckPasswordsMatch()
        {
            if (password != confirmationPassword)
            {
                MessageBox.Show("Las contraseñas no coinciden", "Error en las contraseñas", MessageBoxButton.OK);
                return false;
            }

            return true;
        }

        private bool CheckEmptyFields()
        {
            bool completeInformation = !string.IsNullOrWhiteSpace(username) && !string.IsNullOrWhiteSpace(email) &&
                                     !string.IsNullOrWhiteSpace(password) && !string.IsNullOrWhiteSpace(confirmationPassword);

            if (!completeInformation)
            {
                MessageBox.Show("Ingrese la información de nuevo por favor", "Campos vacíos o con espacios", MessageBoxButton.OK);
            }

            return completeInformation;
        }

        private bool CheckExcessLength()
        {
            bool validLength = username.Length <= 20 && email.Length <= 50 && password.Length <= 30;

            if (!validLength)
            {
                MessageBox.Show("La información excede el número de caracteres permitido", "Máximo de caracteres", MessageBoxButton.OK);
            }

            return validLength;
        }


        private bool CheckPassword()
        {
            bool validPassword = Regex.IsMatch(password, "^(?=\\w*\\d)(?=\\w*[A-Z])(?=\\w*[a-z])\\S{8,30}$");

            if (!validPassword)
            {
                MessageBox.Show("Su contraseña cuenta con valores inválidos", "Ingrese la contraseña de nuevo", MessageBoxButton.OK);
            }

            return validPassword;
        }

        #endregion
    }
}
