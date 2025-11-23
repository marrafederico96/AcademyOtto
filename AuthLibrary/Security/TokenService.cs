using AuthLibrary.Models;
using Microsoft.IdentityModel.Tokens;
using System.IdentityModel.Tokens.Jwt;
using System.Security.Claims;
using System.Text;

namespace AuthLibrary.Security
{
    internal class TokenService
    {
        /// <summary>
        /// Generates a JSON Web Token (JWT) for the specified user email and role using the provided token settings.
        /// </summary>
        /// <param name="email">The email address to include in the JWT claims. Cannot be null.</param>
        /// <param name="role">The user role to include in the JWT claims. Cannot be null.</param>
        /// <param name="tokenSettings">The settings used to configure the JWT, including secret key, issuer, audience, and expiration. Cannot be
        /// null, and must contain a valid secret key.</param>
        /// <returns>A string containing the generated JWT. The token includes the specified email and role claims, and is signed
        /// using the provided secret key.</returns>
        /// <exception cref="ArgumentException">Thrown if the secret key in <paramref name="tokenSettings"/> is null.</exception>
        internal static string GenerateJwtToken(string email, string role, TokenSettings tokenSettings)
        {
            var secretKey = tokenSettings.SecretKey ?? throw new ArgumentException("Secret key not found");
            var key = Encoding.ASCII.GetBytes(secretKey);

            var tokenHandler = new JwtSecurityTokenHandler();
            var tokenDescriptor = new SecurityTokenDescriptor
            {
                Subject = new ClaimsIdentity(
                [
                    new Claim(ClaimTypes.Email, email),
                    new Claim(ClaimTypes.Role, role)
                ]),
                Issuer = tokenSettings.Issuer,
                Audience = tokenSettings.Audience,
                Expires = DateTime.UtcNow.AddMinutes(tokenSettings.ExpirationMinutes),
                SigningCredentials = new SigningCredentials(new SymmetricSecurityKey(key), SecurityAlgorithms.HmacSha256Signature)
            };

            var token = tokenHandler.CreateToken(tokenDescriptor);
            var tokenString = tokenHandler.WriteToken(token);
            return tokenString;
        }
    }
}
