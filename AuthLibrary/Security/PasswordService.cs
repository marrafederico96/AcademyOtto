using System.Security.Cryptography;
using System.Text;

namespace AuthLibrary.Security
{
    internal class PasswordService
    {
        /// <summary>
        /// Verifies whether the specified password matches the stored hash using the provided salt.
        /// </summary>
        /// <remarks>This method uses PBKDF2 with SHA-256 and 100,000 iterations to derive the hash for
        /// comparison. The comparison is performed in constant time to help prevent timing attacks.</remarks>
        /// <param name="password">The plaintext password to verify.</param>
        /// <param name="storedHashBase64">The password hash, encoded as a Base64 string, to compare against.</param>
        /// <param name="storedSaltBase64">The salt used to generate the stored hash, encoded as a Base64 string.</param>
        /// <returns>true if the password is valid and matches the stored hash; otherwise, false.</returns>
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

        /// <summary>
        /// Generates a cryptographically secure hash and salt for the specified password using PBKDF2 with SHA-256.
        /// </summary>
        /// <remarks>The generated hash and salt can be stored for later password verification. The method
        /// uses 100,000 iterations of PBKDF2 with SHA-256 for key derivation, which provides strong resistance against
        /// brute-force attacks.</remarks>
        /// <param name="password">The plain text password to hash. Cannot be null.</param>
        /// <returns>A tuple containing the Base64-encoded password hash and the Base64-encoded salt.</returns>
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
