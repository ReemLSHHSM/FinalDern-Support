using FinalDern_Support.Models;
using Microsoft.AspNetCore.Identity;
using Microsoft.IdentityModel.Tokens;
using System.IdentityModel.Tokens.Jwt;
using System.Text;

namespace FinalDern_Support.Repositories.Services
{
    public class Jwt_TokenServices
    {
        private readonly IConfiguration _configuration;
        private readonly SignInManager<ApplicationUser> _signInManager;

        public Jwt_TokenServices(IConfiguration configuration, SignInManager<ApplicationUser> signInManager)
        {
            _configuration = configuration;
            this._signInManager = signInManager;
        }

        //Generate Token & verify it 
        public static TokenValidationParameters ValidateToken(IConfiguration configuration)
        {
            return new TokenValidationParameters
            {
                ValidateIssuerSigningKey = true,
                IssuerSigningKey = GetSecurityKey(configuration),
                ValidateIssuer = false,
                ValidateAudience = false
            };
        }
        private static SecurityKey GetSecurityKey(IConfiguration configuration)
        {
            var secretKey = configuration["JWT:SecretKey"];
            if (secretKey == null)
            {
                throw new InvalidOperationException("JWT secret key not exsist");
            }

            //convert secret key to array of byte
            var secretBytes = Encoding.UTF8.GetBytes(secretKey);

            return new SymmetricSecurityKey(secretBytes);
        }
        public async Task<string> GenerateToken(ApplicationUser user, TimeSpan expiryDate)
        {
            var userPrincipal = await _signInManager.CreateUserPrincipalAsync(user);
            if (userPrincipal == null)
            {
                return null;
            }
            var singInKey = GetSecurityKey(_configuration);

            var token = new JwtSecurityToken(
                expires: DateTime.UtcNow + expiryDate,
                signingCredentials: new SigningCredentials(singInKey, SecurityAlgorithms.HmacSha256),
                claims: userPrincipal.Claims);

            return new JwtSecurityTokenHandler().WriteToken(token);
        }

    }
}
