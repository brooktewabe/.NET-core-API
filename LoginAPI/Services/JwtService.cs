using Microsoft.IdentityModel.Tokens;
using System.IdentityModel.Tokens.Jwt;
using System.Security.Claims;
using System.Text;

namespace LoginAPI.Services
{
    public class JwtService
    {
        public string Key { get; set; }
        public int Duration { get; set; }

        public JwtService(IConfiguration configuration)
        {
            
            Key = configuration.GetValue<string>("Jwt:Key");
            Duration = configuration.GetValue<int>("Jwt:ExpiryDuration");
        }

        public string GenerateToken(string email)
        {            
            var Key = Convert.FromBase64String(this.Key);
            var expDuration = int.Parse(this.Duration.ToString());
            // var Signature = new SigningCredentials(Key, SecurityAlgorithms.HmacSha256);
            var PayLoad = new[]
            {
                new Claim("id", email)
            };
            var claims = new List<Claim> {
             new Claim("id", email)
            };

            /*var JwtToken = new JwtSecurityToken(
                issuer: null,
                audience: null,
                claims: PayLoad,
                notBefore: DateTime.UtcNow,
                expires: DateTime.UtcNow.AddMinutes(Duration),
                signingCredentials: new SigningCredentials(new SymmetricSecurityKey(Key), SecurityAlgorithms.HmacSha256Signature)
                );*/
            var tokenDiscriptor = new SecurityTokenDescriptor
            {
                Issuer = null,
                Audience= null,
                //Claims= claims,
                NotBefore= DateTime.UtcNow,
                Subject  = new ClaimsIdentity(claims),
                Expires= DateTime.UtcNow.AddMinutes(expDuration),
                SigningCredentials= new SigningCredentials(new SymmetricSecurityKey(Key), SecurityAlgorithms.HmacSha256Signature)
            };

            var tokenHandler = new JwtSecurityTokenHandler();
            var jwttoken = tokenHandler.CreateJwtSecurityToken(tokenDiscriptor);
            var token = tokenHandler.WriteToken(jwttoken);

           // return new JwtSecurityTokenHandler().WriteToken(JwtToken);

            return token;
        }
    }
}
