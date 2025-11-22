using api_dotnet_core.DTOs;
using Microsoft.Extensions.Configuration;
using System;
using System.IdentityModel.Tokens.Jwt;
using System.Text;
using Microsoft.IdentityModel.Tokens;
using System.Security.Claims;

namespace api_dotnet_core.Services
{
    public class JwtTokenService : ITokenService
    {
        private readonly IConfiguration _cfg;
        private readonly string _key;
        private readonly string _issuer;
        private readonly string _audience;
        private readonly int _expiresIn;


        public JwtTokenService(IConfiguration cfg)
        {
            _cfg = cfg;
            _key = _cfg["Jwt:Key"];
            _issuer = _cfg["Jwt:Issuer"];
            _audience = _cfg["Jwt:Audience"];
            _expiresIn = int.Parse(_cfg["Jwt:ExpiresInMinutes"] ?? "60");
        }


        public string GenerateToken(UserDto user)
        {
            var securityKey = new SymmetricSecurityKey(Encoding.UTF8.GetBytes(_key));
            var creds = new SigningCredentials(securityKey, SecurityAlgorithms.HmacSha256);


            var claims = new[] {
                    new Claim(JwtRegisteredClaimNames.Sub, user.Email),
                    new Claim("uid", user.Id.ToString()),
                    new Claim("name", user.FullName ?? "")
            };


            var token = new JwtSecurityToken(
            issuer: _issuer,
            audience: _audience,
            claims: claims,
            expires: DateTime.UtcNow.AddMinutes(_expiresIn),
            signingCredentials: creds);


            return new JwtSecurityTokenHandler().WriteToken(token);
        }
    }
}
