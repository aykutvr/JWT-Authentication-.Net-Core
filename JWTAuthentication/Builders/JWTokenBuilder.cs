using Microsoft.IdentityModel.Tokens;
using System.IdentityModel.Tokens.Jwt;
using System.Reflection;
using System.Security.Claims;
using System.Text;

namespace JWTAuthentication.Builders
{
    public class JWTokenBuilder
    {
        public static string Build(Dto.UserDto userData)
        {
            Dictionary<string, object> claims = new Dictionary<string, object>();

            claims.Add(ClaimTypes.Name, userData.UserName);
            claims.Add("id", userData.Id);
            claims.Add(ClaimTypes.Email, userData.EmailAddress);

            var key = Encoding.ASCII.GetBytes(Settings.SecretKey);
                var tokenHandler = new JwtSecurityTokenHandler();
                var tokenDescriptor = new SecurityTokenDescriptor
                {
                    Claims = claims,
                    Audience = Settings.JWTIssuer,
                    Issuer = Settings.JWTIssuer,
                    NotBefore = new DateTimeOffset(DateTime.Now).DateTime.AddMinutes(-1),
                    Subject = new ClaimsIdentity(claims.Select(s=> new Claim(s.Key.ToString(),s.Value.ToString()))),
                    Expires = DateTime.UtcNow.AddDays(1),
                    SigningCredentials = new SigningCredentials(new SymmetricSecurityKey(key), SecurityAlgorithms.HmacSha256Signature)
                };
                var token = tokenHandler.CreateToken(tokenDescriptor);
                return tokenHandler.WriteToken(token);

        }
    }
}
