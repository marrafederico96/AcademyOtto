using System.Security.Cryptography;
using System.Text;

namespace AuthLibrary
{
    internal class PasswordService
    {
        internal static bool CheckPassword(string password, string storedHashBase64, string storedSaltBase64)
        {
            byte[] saltBytes = Convert.FromBase64String(storedSaltBase64);

            byte[] hashBytes = Rfc2898DeriveBytes.Pbkdf2(
                Encoding.UTF8.GetBytes(password),
                saltBytes,
                100_000,
                HashAlgorithmName.SHA256,
                32
            );

            byte[] storedHashBytes = Convert.FromBase64String(storedHashBase64);
            return CryptographicOperations.FixedTimeEquals(hashBytes, storedHashBytes);
        }

        internal static (string Hash, string Salt) HashPassword(string password)
        {
            byte[] saltBytes = new byte[32];
            RandomNumberGenerator.Fill(saltBytes);
            string saltBase64 = Convert.ToBase64String(saltBytes);

            byte[] hashBytes = Rfc2898DeriveBytes.Pbkdf2(
               Encoding.UTF8.GetBytes(password),
               saltBytes,
               100_000,
               HashAlgorithmName.SHA256,
               32
           );
            string hashBase64 = Convert.ToBase64String(hashBytes);
            return (hashBase64, saltBase64);
        }
    }
}
