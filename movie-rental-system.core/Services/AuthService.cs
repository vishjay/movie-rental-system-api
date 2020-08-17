using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.Configuration;
using Microsoft.IdentityModel.Tokens;
using movie_rental_system.core.Data;
using movie_rental_system.core.DTOs;
using movie_rental_system.core.Models;
using System;
using System.Collections.Generic;
using System.IdentityModel.Tokens.Jwt;
using System.Linq;
using System.Runtime.InteropServices.ComTypes;
using System.Security.Claims;
using System.Security.Cryptography;
using System.Text;
using System.Threading.Tasks;

namespace movie_rental_system.core.Services
{
    public class AuthService
    {
        protected readonly MovieRentalContext context;
        protected readonly IConfiguration configuration;
        public AuthService(MovieRentalContext context, IConfiguration configuration)
        {
            this.context = context;
            this.configuration = configuration;
        }
        public async Task AddUserAsync(UserDTO_In userDTO)
        {
            //create user
            User user = new User()
            {
                FirstName = userDTO.FirstName,
                LastName = userDTO.LastName,
                Email = userDTO.Email,
                CreatedOn = DateTime.UtcNow,
                UpdatedOn = DateTime.UtcNow
            };
            //create password hash and the salt
            byte[] passwordHash, passwordSalt;
            CreatePasswordHash(userDTO.Password, out passwordHash, out passwordSalt);
            user.PasswordHash = passwordHash;
            user.PasswordSalt = passwordSalt;

            //add user to the db
            this.context.Users.Add(user);
            await this.context.SaveChangesAsync();
        }


        public async Task<AuthDTO_Out> AuthenticateAsync(AuthDTO_In authDTO, string ipAddress)
        {
            //check if the user exist
             var user = await this.context.Users
                                          .Include(u => u.Refresh_Tokens)
                                          .SingleOrDefaultAsync(u => u.Email == authDTO.Email);

            if(user == null)
                return null;

            //check if for the password match
            if (!VerifyPasswordHash(authDTO.Password, user.PasswordHash, user.PasswordSalt))
                return null;
            //create the JWT token and return 
            var JWTToken = this.GenerateJWTToken(user);
            var refreshToken = this.generateRefreshToken(ipAddress);

            // save refresh token
                user.Refresh_Tokens.Add(refreshToken);
                context.Update(user);
                await context.SaveChangesAsync();

            return new AuthDTO_Out
            {
                Id = user.Id,
                FirstName = user.FirstName,
                LastName = user.LastName,
                Email = user.Email,
                Token = JWTToken,
                RefreshToken = refreshToken.Token
            };
        }


        public async Task<AuthDTO_Out> RefreshTokenAsync(string token, string ipAddress)
        {
            var user = await context.Users
                              .Include(u => u.Refresh_Tokens)
                              .SingleOrDefaultAsync(u => u.Refresh_Tokens.Any(t => t.Token == token));

            // return null if no user found with token
            if (user == null) return null;

            var refreshToken = user.Refresh_Tokens.Single(t => t.Token == token);

            // return null if token is no longer active
            if (!refreshToken.IsActive) return null;

            // replace old refresh token with a new one and save
            var newRefreshToken = generateRefreshToken(ipAddress);
            refreshToken.Revoked = DateTime.UtcNow;
            refreshToken.RevokedByIp = ipAddress;
            refreshToken.ReplacedByToken = newRefreshToken.Token;
            user.Refresh_Tokens.Add(newRefreshToken);
            context.Update(user);
            await context.SaveChangesAsync();

            // generate new jwt
            var JWTToken = this.GenerateJWTToken(user);

            return new AuthDTO_Out
            {
                Id = user.Id,
                FirstName = user.FirstName,
                LastName = user.LastName,
                Email = user.Email,
                Token = JWTToken,
                RefreshToken = newRefreshToken.Token
            };
        }

        public async Task<bool> RevokeTokenAsync(string token, string ipAddress)
        {
            var user = await context.Users
                  .Include(u => u.Refresh_Tokens)
                  .SingleOrDefaultAsync(u => u.Refresh_Tokens.Any(t => t.Token == token));

            // return null if no user found with token
            if (user == null) return false;

            var refreshToken = user.Refresh_Tokens.Single(t => t.Token == token);

            // return null if token is no longer active
            if (!refreshToken.IsActive) return false;

            //revoke token
            refreshToken.Revoked = DateTime.UtcNow;
            refreshToken.RevokedByIp = ipAddress;
            context.Update(user);
            await context.SaveChangesAsync();
            return true;

        }

        public async Task<User> GetUserAsync(Guid id)
        {
            return await this.context.Users.SingleOrDefaultAsync(u => u.Id == id);
        }

        public User GetUser(Guid id)
        {
            return this.context.Users.SingleOrDefault(u => u.Id == id);
        }


        //private methods

        private string GenerateJWTToken(User user)
        {
            var tokenHandler = new JwtSecurityTokenHandler();
            var key = Encoding.ASCII.GetBytes(configuration["Authentication:Secret"]);
            var tokenDescriptor = new SecurityTokenDescriptor
            {
                Subject = new ClaimsIdentity(new Claim[]
                {
                    new Claim(ClaimTypes.Name, user.Id.ToString())
                }),
                Expires = DateTime.UtcNow.AddMinutes(15),
                SigningCredentials = new SigningCredentials(new SymmetricSecurityKey(key), SecurityAlgorithms.HmacSha256Signature)
            };
            var token = tokenHandler.CreateToken(tokenDescriptor);
            var tokenString = tokenHandler.WriteToken(token);
            return tokenString;
        }


        private RefreshToken generateRefreshToken(string ipAddress)
        {
            using (var rngCryptoServiceProvider = new RNGCryptoServiceProvider())
            {
                var randomBytes = new byte[64];
                rngCryptoServiceProvider.GetBytes(randomBytes);
                return new RefreshToken
                {
                    Token = Convert.ToBase64String(randomBytes),
                    Expires = DateTime.UtcNow.AddDays(7),
                    Created = DateTime.UtcNow,
                    CreatedByIp = ipAddress
                };
            }
        }

        private static void CreatePasswordHash(string password, out byte[] passwordHash, out byte[] passwordSalt)
        {
            using (var hmac = new System.Security.Cryptography.HMACSHA512())
            {
                passwordSalt = hmac.Key;
                passwordHash = hmac.ComputeHash(System.Text.Encoding.UTF8.GetBytes(password));
            }
        }


        private static bool VerifyPasswordHash(string password, byte[] storedHash, byte[] storedSalt)
        {
            if (password == null) throw new ArgumentNullException("password");
            if (string.IsNullOrWhiteSpace(password)) throw new ArgumentException("Value cannot be empty or whitespace only string.", "password");
            if (storedHash.Length != 64) throw new ArgumentException("Invalid length of password hash (64 bytes expected).", "passwordHash");
            if (storedSalt.Length != 128) throw new ArgumentException("Invalid length of password salt (128 bytes expected).", "passwordHash");

            using (var hmac = new System.Security.Cryptography.HMACSHA512(storedSalt))
            {
                var computedHash = hmac.ComputeHash(System.Text.Encoding.UTF8.GetBytes(password));
                for (int i = 0; i < computedHash.Length; i++)
                {
                    if (computedHash[i] != storedHash[i]) return false;
                }
            }

            return true;
        }
    }
}
