using System.Security.Cryptography;
using System.Text;

namespace WebApp.MemesMVC.Security
{
    public static class Encryptor
    {
        public static bool IsPasswordCorrect(string userPassword, string passwordFromDatabase)
        {
            return EncryptPassword(userPassword).Equals(passwordFromDatabase);
        }

        public static string EncryptPassword(string userPassword)
        {
            using MD5 md5 = MD5.Create();

            byte[] passwordBytes = System.Text.Encoding.ASCII.GetBytes(userPassword);
            byte[] hashBytes = md5.ComputeHash(passwordBytes);

            StringBuilder sb = new StringBuilder();

            for (int i = 0; i < hashBytes.Length; i++)
            {
                sb.Append(hashBytes[i].ToString("X2"));
            }

            return sb.ToString();
        }
    }
}
