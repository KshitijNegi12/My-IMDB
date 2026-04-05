using IMDB.Models.Request;
using IMDB.Services.Interfaces;
using System.IdentityModel.Tokens.Jwt;
using System.Security.Claims;
using System.Collections.Generic;
using System;
using Microsoft.IdentityModel.Tokens;
using System.Text;
using Microsoft.Extensions.Configuration;
using IMDB.Repositories.Interfaces;
using IMDB.Models.Db;

namespace IMDB.Services
{
    public class AuthService : IAuthService
    {
        private readonly IConfiguration _configuration;
        private readonly IAuthRepository _authRepository;

        public AuthService(IConfiguration configuration, IAuthRepository authRepository)
        {
            _configuration = configuration;
            _authRepository = authRepository;
        }

        public bool SignIn(SignInRequest user)
        {
            var userDb = _authRepository.GetbyEmail(user.Email);
            if (userDb != null)
            {
                return false;
            }

            Validate(user);

            _authRepository.Add(new User
            {
                Email = user.Email,
                PasswordHash = BCrypt.Net.BCrypt.HashPassword(user.Password)
            });

            return true;
        }

        public string LogIn(LogInRequest user)
        {
            var userDb = _authRepository.GetbyEmail(user.Email);
            if (userDb == null)
            {
                return null;
            }

            var isPasswordValid = BCrypt.Net.BCrypt.Verify(user.Password, userDb.PasswordHash);
            if (!isPasswordValid)
            {
                return null;
            }

            return GenerateToken(user.Email);
        }

        public string GenerateToken(string email)
        {
            var authClaims = new List<Claim>
            {
                new Claim(ClaimTypes.Name, email),
                new Claim(JwtRegisteredClaimNames.Jti, Guid.NewGuid().ToString())

            };
            var authSigninKey = new SymmetricSecurityKey(Encoding.ASCII.GetBytes(_configuration["JWT_SECRET_KEY"]));

            var token = new JwtSecurityToken(
                issuer: _configuration["JWT_VALID_ISSUER"],
                audience: _configuration["JWT_VALID_AUDIENCE"],
                expires: DateTime.Now.AddHours(1),
                claims: authClaims,
                signingCredentials: new SigningCredentials(authSigninKey, SecurityAlgorithms.HmacSha256Signature)
            );
            return new JwtSecurityTokenHandler().WriteToken(token);
        }

        public void Validate(SignInRequest user)
        {
            if (string.IsNullOrEmpty(user.Email))
            {
                throw new ArgumentException("Email is required");
            }
            if (string.IsNullOrEmpty(user.Password))
            {
                throw new ArgumentException("Password is required");
            }
            if (user.Password.Length < 8)
            {
                throw new ArgumentException("Password must be at least 8 characters long");
            }

        }

    }
}
