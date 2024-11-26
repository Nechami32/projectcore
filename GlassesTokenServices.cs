
using System;
using System.IdentityModel.Tokens.Jwt;
using Microsoft.IdentityModel.Tokens;
using System.Security.Claims;
using System.Text;

namespace OurApi.Services
{
    public static class TasksTokenService
    {
        private static SymmetricSecurityKey key = new SymmetricSecurityKey(Encoding.UTF8.GetBytes("SXkSqsKyNUyvGbnHs7ke2NCq8zQzNLW7mPmHbnZZ"));
        private static string issuer = "https://localhost:5071";

        public static SecurityToken GetToken(List<Claim> claims) =>
            new JwtSecurityToken(
                issuer: issuer,
               // audience: null, // אם לא נדרש Audience
                claims: claims,
                expires: DateTime.UtcNow.AddDays(30),
                signingCredentials: new SigningCredentials(key, SecurityAlgorithms.HmacSha256)
            );

        public static bool IsTokenExpired(string token)
        {
            var tokenHandler = new JwtSecurityTokenHandler();
            try
            {
                tokenHandler.ValidateToken(token, GetTokenValidationParameters(), out var validatedToken);
                return false; // הטוקן תקין
            }
            catch (SecurityTokenExpiredException)
            {
                return true; // הטוקן פג תוקף
            }
            catch
            {
                return true; // כל שגיאה אחרת
            }
        }

        public static TokenValidationParameters GetTokenValidationParameters() =>
            new TokenValidationParameters
            {
                ValidIssuer = issuer,
                ValidateIssuer = true,
                ValidateAudience = false, // אם Audience לא נדרש
                IssuerSigningKey = key,
                ValidateLifetime = true,
                ClockSkew = TimeSpan.Zero
            };

        public static string WriteToken(SecurityToken token) =>
            new JwtSecurityTokenHandler().WriteToken(token);

        public static bool LifetimeValidator(DateTime? notBefore, DateTime? expires, SecurityToken token, TokenValidationParameters validationParameters)
        {
            if (!expires.HasValue)
                return false;

            return expires.Value > DateTime.UtcNow;
        }
    }
}






            