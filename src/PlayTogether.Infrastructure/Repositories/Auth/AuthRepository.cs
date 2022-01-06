using Microsoft.AspNetCore.Identity;
using Microsoft.Extensions.Configuration;
using Microsoft.IdentityModel.Tokens;
using PlayTogether.Core.Dtos.Incoming.Auth;
using PlayTogether.Core.Dtos.Outgoing.Auth;
using PlayTogether.Core.Interfaces.Repositories.Auth;
using System;
using System.Collections.Generic;
using System.IdentityModel.Tokens.Jwt;
using System.Linq;
using System.Security.Claims;
using System.Text;
using System.Threading.Tasks;

namespace PlayTogether.Infrastructure.Repositories.Auth
{
    public class AuthRepository : IAuthRepository
    {
        private UserManager<IdentityUser> _userManager;
        private RoleManager<IdentityRole> _roleManager;
        private readonly IConfiguration _configuration;
        private readonly IConfigurationSection _jwtSettings;

        public AuthRepository(UserManager<IdentityUser> userManager,
            RoleManager<IdentityRole> roleManager,
            IConfiguration configuration)
        {
            _userManager = userManager;
            _roleManager = roleManager;
            _configuration = configuration;
            _jwtSettings = _configuration.GetSection("Jwt");
        }

        public async Task<AuthResultDto> LoginUserAsync(LoginDto loginDto)
        {
            if (loginDto is null) {
                throw new NullReferenceException("Login info is null");
            }

            var user = await _userManager.FindByNameAsync(loginDto.Username);

            if (user is null) {
                return new AuthResultDto {
                    Errors = new List<string>() { "Username is not found" },
                    Success = false,
                };
            }

            if (!await IsAdmin(user)) {
                if (!user.EmailConfirmed) {
                    return new AuthResultDto {
                        Errors = new List<string>() { "User is not comfirm email" },
                        Success = false,
                    };
                }
            }

            var result = await _userManager.CheckPasswordAsync(user, loginDto.Password);

            if (!result) {
                return new AuthResultDto {
                    Errors = new List<string>() { "Invalid password" },
                    Success = false,
                };
            }

            var token = await GenerateToken(user);

            return new AuthResultDto {
                Message = token[0],
                Success = true,
                ExpireDate = DateTime.Parse(token[1])
            };
        }

        public async Task<AuthResultDto> RegisterAdminAsync(RegisterDto registerDto)
        {
            if (registerDto is null) {
                throw new NullReferenceException("Register info is null");
            }

            if (registerDto.Password != registerDto.ConfirmPassword) {
                return new AuthResultDto {
                    Errors = new List<string>() { "Confirm password doesn't match the password" },
                    Success = false
                };
            }
            var ExistUser = await IsExistUser(registerDto.Username, registerDto.Email);
            if (ExistUser is not null) {
                return new AuthResultDto {
                    Errors = new List<string>() { ExistUser },
                    Success = false
                };
            }

            var identityUser = new IdentityUser() {
                Email = registerDto.Email,
                UserName = registerDto.Username
            };

            var result = await _userManager.CreateAsync(identityUser, registerDto.Password);
            await SetRoleForUser(identityUser, AuthConstant.RoleAdmin);

            if (result.Succeeded) {
                //var token = await _userManager.GenerateEmailConfirmationTokenAsync(identityUser);
                //var confirmLink = Url
                return new AuthResultDto {
                    Message = "Admin create successfully!",
                    Success = true
                };
            }

            return new AuthResultDto {
                Message = "Admin create fail",
                Success = false,
                Errors = result.Errors.Select(e => e.Description).ToList()
            };
        }

        private async Task<bool> IsAdmin(IdentityUser user)
        {
            // return role
            var roles = await _userManager.GetRolesAsync(user);

            foreach (var item in roles) {
                if (item == AuthConstant.RoleAdmin) {
                    return true;
                }
            }

            return false;
        }

        private async Task SetRoleForUser(IdentityUser identityUser, string roleAdmin)
        {
            if (!await _roleManager.RoleExistsAsync(AuthConstant.RoleAdmin)) {
                await _roleManager.CreateAsync(new IdentityRole(AuthConstant.RoleAdmin));
            }

            if (await _roleManager.RoleExistsAsync(AuthConstant.RoleAdmin)) {
                await _userManager.AddToRoleAsync(identityUser, AuthConstant.RoleAdmin);
            }
        }

        private async Task<string> IsExistUser(string username, string email)
        {
            var existEmail = await _userManager.FindByEmailAsync(email);
            if (existEmail is not null) {
                return $"{email} is exist";
            }

            var existUsername = await _userManager.FindByNameAsync(username);
            if (existUsername is not null) {
                return $"{username} is exist";
            }

            return null;
        }

        private SigningCredentials GetSigningCredentials()
        {
            var key = Encoding.UTF8.GetBytes(_jwtSettings.GetSection("SecretKey").Value);
            var secret = new SymmetricSecurityKey(key);

            return new SigningCredentials(secret, SecurityAlgorithms.HmacSha256);
        }

        private async Task<List<Claim>> GetClaims(IdentityUser user)
        {
            var claims = new List<Claim>
            {
            new Claim("Id", user.Id),
            new Claim(ClaimTypes.NameIdentifier, user.Id),
            new Claim(ClaimTypes.Name, user.UserName),
            new Claim(ClaimTypes.Email, user.Email),
        };

            var roles = await _userManager.GetRolesAsync(user);
            foreach (var role in roles) {
                claims.Add(new Claim(ClaimTypes.Role, role));
            }

            return claims;
        }

        private JwtSecurityToken GenerateTokenOptions(SigningCredentials signingCredentials, List<Claim> claims)
        {
            var timeExpireInStr = _jwtSettings.GetSection("Expired").Value;
            var timeExpire = Int32.Parse(timeExpireInStr);

            var tokenOptions = new JwtSecurityToken(
                claims: claims,
                expires: DateTime.UtcNow.AddDays(timeExpire),
                signingCredentials: signingCredentials);

            return tokenOptions;
        }

        private async Task<List<String>> GenerateToken(IdentityUser user)
        {
            var signingCredentials = GetSigningCredentials();
            var claims = await GetClaims(user);
            var tokenOptions = GenerateTokenOptions(signingCredentials, claims);
            var token = new JwtSecurityTokenHandler().WriteToken(tokenOptions);

            List<String> result = new List<string>();
            result.Add(token);
            result.Add(tokenOptions.ValidTo.ToString());

            return result;
        }

        private JwtPayload PayloadInfo(string idToken)
        {
            var token = idToken;
            var handler = new JwtSecurityTokenHandler();
            var tokenData = handler.ReadJwtToken(token);
            return tokenData.Payload;
        }
    }
}