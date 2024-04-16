using Microsoft.IdentityModel.Tokens;
using System.IdentityModel.Tokens.Jwt;
using System.Security.Claims;
using System.Text;
using UserManagement.Entity;
using UserManagement.Interface;

namespace UserManagement.Service
{
    public class AuthServiceImpl : IAuthService
    {
        private readonly IConfiguration _configuration;
        public AuthServiceImpl(IConfiguration configuration )
        {
            _configuration= configuration;
        }
        public string GenerateJwtToken(UserEntity entity)
        {
            var key = new SymmetricSecurityKey(Encoding.UTF8.GetBytes(_configuration["Jwt:Key"]));
            var creds = new SigningCredentials(key, SecurityAlgorithms.HmacSha256);

            var claims = new[]
            {
                new Claim(ClaimTypes.Email, entity.Email),
                new Claim(ClaimTypes.Name,entity.FirstName),
                new Claim(ClaimTypes.Role,entity.Role+""),
               // new Claim("Password",entity.Password),
                new Claim("UserId",entity.UserID.ToString())
            };

            var token = new JwtSecurityToken(
                issuer: _configuration["Jwt:Issuer"],
                audience: _configuration["Jwt:Audience"],
                claims: claims,
                expires: DateTime.UtcNow.AddMinutes(Convert.ToDouble(_configuration["Jwt:Minutes"])),
                signingCredentials: creds);

            return new JwtSecurityTokenHandler().WriteToken(token);
        }
    }
}
