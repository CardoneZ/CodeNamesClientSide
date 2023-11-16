using System;
using System.Collections.Generic;
using System.Linq;
using System.Security.Cryptography;
using System.Text;
using System.Threading.Tasks;

namespace CodeNamesClientSide.Utilities
{
    public class PasswordEncryptor
    {
        public static string HashPassword(string password)
        {
            using (SHA256 sha256 = SHA256.Create())
            {
                byte[] bytes = Encoding.UTF8.GetBytes(password);
                byte[] hash = sha256.ComputeHash(bytes);
                string hashedPassword = BitConverter.ToString(hash).Replace("-", string.Empty).ToLower();
                return hashedPassword;
            }
        }
    }
}
