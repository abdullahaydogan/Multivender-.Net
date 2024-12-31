using System;
using System.Collections.Generic;
using System.IdentityModel.Tokens.Jwt;
using System.Linq;
using System.Security.Claims;
using System.Text;
using System.Threading.Tasks;
using Entities.DTO_s;
using Entities.Models;
using Microsoft.AspNetCore.Identity;
using Microsoft.IdentityModel.Tokens;
using Repositories;
using Services.Interfaces;

namespace Services
{
    public class AuthenticationService : IAuthenticationService
    {
        private readonly RepositoryContext _repositoryContext;
        private readonly UserManager<User> _userManager;

        private User? _user;

        // Secret key, issuer, audience ve expiration süresi gibi değerler config'den alınabilir
        private readonly string _secretKey = "kocaeliunivercitymuhtas3project2024";
        private readonly string _validIssuer = "MuhTas3";
        private readonly string _validAudience = "http://localhost:300";
        private readonly int _expiresInMinutes = 60;

        public AuthenticationService(RepositoryContext repositoryContext, UserManager<User> userManager)
        {
            _repositoryContext = repositoryContext;
            _userManager = userManager;
        }

        public async Task<string> CreateToken()
        {
            var signinCredentials = GetSigninCredentials();
            var claims = await GetClaims();
            var tokenOptions = GenerateTokenOptions(signinCredentials, claims);
            return new JwtSecurityTokenHandler().WriteToken(tokenOptions);
        }

        public async Task<IdentityResult> RegisterUser(UserForRegistrationDto userForRegistrationDto)
        {
            var user = new User
            {
                FirstName = userForRegistrationDto.FirstName,
                LastName = userForRegistrationDto.LastName,
                UserName = userForRegistrationDto.UserName,
                Email = userForRegistrationDto.Email
            };

            var result = await _userManager.CreateAsync(user, userForRegistrationDto.Password);

            if (result.Succeeded)
            {
                await _userManager.AddToRolesAsync(user, userForRegistrationDto.Roles);
            }

            return result;
        }

        public async Task<bool> ValidateUser(UserForAuthenticationDto userForAuthenticationDto)
        {
            try
            {
                _user = await _userManager.FindByNameAsync(userForAuthenticationDto.Username);

                if (_user == null)
                {
                    throw new InvalidOperationException("Kullanıcı bulunamadı.");
                }

                var isPasswordValid = await _userManager.CheckPasswordAsync(_user, userForAuthenticationDto.Password);

                if (!isPasswordValid)
                {
                    throw new UnauthorizedAccessException("Yanlış şifre.");
                }

                return true;
            }
            catch (Exception ex)
            {
                throw new Exception("Kullanıcı doğrulama sırasında hata oluştu.", ex);
            }
        }

        private SigningCredentials GetSigninCredentials()
        {
            var key = Encoding.UTF8.GetBytes(_secretKey);
            var signinCredentials = new SigningCredentials(new SymmetricSecurityKey(key), SecurityAlgorithms.HmacSha256);
            return signinCredentials;
        }

        private async Task<List<Claim>> GetClaims()
        {
            var claims = new List<Claim>
            {
                new Claim(ClaimTypes.Name, _user.UserName),
            };

            var roles = await _userManager.GetRolesAsync(_user);
            foreach (var role in roles)
            {
                claims.Add(new Claim(ClaimTypes.Role, role));
            }

            return claims;
        }

        private JwtSecurityToken GenerateTokenOptions(SigningCredentials signinCredentials, List<Claim> claims)
        {
            var tokenOptions = new JwtSecurityToken(
                issuer: _validIssuer,
                audience: _validAudience,
                claims: claims,
                expires: DateTime.Now.AddMinutes(_expiresInMinutes),
                signingCredentials: signinCredentials);

            return tokenOptions;
        }
    }
}
