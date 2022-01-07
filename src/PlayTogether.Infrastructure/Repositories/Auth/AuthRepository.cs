using AutoMapper;
using Microsoft.AspNetCore.Identity;
using Microsoft.Extensions.Configuration;
using Microsoft.IdentityModel.Tokens;
using PlayTogether.Core.Dtos.Incoming.Auth;
using PlayTogether.Core.Dtos.Outgoing.Auth;
using PlayTogether.Core.Interfaces.Repositories.Auth;
using PlayTogether.Infrastructure.Data;
using PlayTogether.Infrastructure.Entities;
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
        private readonly IMapper _mapper;
        private readonly AppDbContext _context;

        public AuthRepository(UserManager<IdentityUser> userManager,
            RoleManager<IdentityRole> roleManager,
            IConfiguration configuration,
            IMapper mapper,
            AppDbContext context)
        {
            _userManager = userManager;
            _roleManager = roleManager;
            _configuration = configuration;
            _jwtSettings = _configuration.GetSection("Jwt");
            _mapper = mapper;
            _context = context;
        }

        public async Task<AuthResultDto> LoginUserAsync(LoginDto loginDto)
        {
            if (loginDto is null) {
                throw new NullReferenceException("Login info is null");
            }

            var user = await _userManager.FindByNameAsync(loginDto.Username);

            if (user is null) {
                return new AuthResultDto {
                    Errors = new List<string>() { "Username is not found" }
                };
            }

            //if (!await IsAdminOrCharity(user)) {
            //    if (!user.EmailConfirmed) {
            //        return new AuthResultDto {
            //            Errors = new List<string>() { "User is not comfirm email" }
            //        };
            //    }
            //}

            var result = await _userManager.CheckPasswordAsync(user, loginDto.Password);

            if (!result) {
                return new AuthResultDto {
                    Errors = new List<string>() { "Invalid password" }
                };
            }

            var token = await GenerateToken(user);

            return new AuthResultDto {
                Message = token[0],
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
                    Errors = new List<string>() { "Confirm password doesn't match the password" }
                };
            }
            var ExistUser = await IsExistUser(registerDto.Username, registerDto.Email);
            if (ExistUser is not null) {
                return new AuthResultDto {
                    Errors = new List<string>() { ExistUser }
                };
            }

            var identityUser = new IdentityUser() {
                Email = registerDto.Email,
                UserName = registerDto.Username
            };

            var result = await _userManager.CreateAsync(identityUser, registerDto.Password);
            await SetRoleForUser(identityUser, AuthConstant.RoleAdmin);

            if (result.Succeeded) {
                var basicUserInfoDto = _mapper.Map<RegisterBasicInfoDto>(registerDto);
                var userEntityModel = _mapper.Map<Admin>(basicUserInfoDto);
                userEntityModel.IdentityId = identityUser.Id;
                await _context.Admins.AddAsync(userEntityModel);

                if (await _context.SaveChangesAsync() >= 0) {
                    //var token = await _userManager.GenerateEmailConfirmationTokenAsync(identityUser);
                    //var confirmLink = Url
                    return new AuthResultDto {
                        Message = "Admin create successfully!"
                    };
                }
            }

            return new AuthResultDto {
                Message = "Admin create fail",
                Errors = result.Errors.Select(e => e.Description).ToList()
            };
        }

        public async Task<AuthResultDto> LoginHirerByGoogle(GoogleLoginDto loginEmailDto)
        {
            var payload = PayloadInfo(loginEmailDto.IdToken);

            if (payload is null) {
                return new AuthResultDto {
                    Errors = new List<string>() { "Invalid google authentication" }
                };
            }

            var info = new UserLoginInfo(loginEmailDto.ProviderName, payload.Sub, loginEmailDto.ProviderName);

            var user = await _userManager.FindByLoginAsync(info.LoginProvider, info.ProviderKey);

            if (user is null) {
                user = await _userManager.FindByEmailAsync(payload["email"].ToString());

                if (user is null) {
                    user = new IdentityUser() {
                        Email = payload["email"].ToString(),
                        UserName = loginEmailDto.Username
                    };

                    var result = await _userManager.CreateAsync(user);
                    await SetRoleForUser(user, AuthConstant.RoleHirer);
                    await _userManager.AddLoginAsync(user, info);

                    if (result.Succeeded) {
                        var basicUserInfoDto = _mapper.Map<RegisterBasicInfoDto>(loginEmailDto);
                        var userEntityModel = _mapper.Map<Hirer>(basicUserInfoDto);
                        userEntityModel.IdentityId = user.Id;
                        await _context.Hirers.AddAsync(userEntityModel);

                        if (await _context.SaveChangesAsync() >= 0) {
                            var token = await GenerateToken(user);
                            return new AuthResultDto {
                                Message = token[0],
                                ExpireDate = DateTime.Parse(token[1])
                            };
                        }
                    }
                }
            }
            else {
                await _userManager.AddLoginAsync(user, info);
                var token = await GenerateToken(user);
                return new AuthResultDto {
                    Message = token[0],
                    ExpireDate = DateTime.Parse(token[1])
                };
            }

            return new AuthResultDto {
                Errors = new List<string>() { "Invalid google authentication" }
            };
        }

        public async Task<AuthResultDto> LoginPlayerByGoogle(GoogleLoginDto loginEmailDto)
        {
            var payload = PayloadInfo(loginEmailDto.IdToken);

            if (payload is null) {
                return new AuthResultDto {
                    Errors = new List<string>() { "Invalid google authentication" }
                };
            }

            var info = new UserLoginInfo(loginEmailDto.ProviderName, payload.Sub, loginEmailDto.ProviderName);

            var user = await _userManager.FindByLoginAsync(info.LoginProvider, info.ProviderKey);

            if (user is null) {
                user = await _userManager.FindByEmailAsync(payload["email"].ToString());

                if (user is null) {
                    user = new IdentityUser() {
                        Email = payload["email"].ToString(),
                        UserName = loginEmailDto.Username
                    };

                    var result = await _userManager.CreateAsync(user);
                    await SetRoleForUser(user, AuthConstant.RolePlayer);
                    await _userManager.AddLoginAsync(user, info);

                    if (result.Succeeded) {
                        var basicUserInfoDto = _mapper.Map<RegisterBasicInfoDto>(loginEmailDto);
                        var userEntityModel = _mapper.Map<Player>(basicUserInfoDto);
                        userEntityModel.IdentityId = user.Id;
                        await _context.Players.AddAsync(userEntityModel);

                        if (await _context.SaveChangesAsync() >= 0) {
                            var token = await GenerateToken(user);
                            return new AuthResultDto {
                                Message = token[0],
                                ExpireDate = DateTime.Parse(token[1])
                            };
                        }
                    }
                }
            }
            else {
                await _userManager.AddLoginAsync(user, info);
                var token = await GenerateToken(user);
                return new AuthResultDto {
                    Message = token[0],
                    ExpireDate = DateTime.Parse(token[1])
                };
            }

            return new AuthResultDto {
                Errors = new List<string>() { "Invalid google authentication" }
            };
        }

        public async Task<AuthResultDto> RegisterCharityAsync(RegisterDto registerDto)
        {
            if (registerDto is null) {
                throw new NullReferenceException("Register info is null");
            }

            if (registerDto.Password != registerDto.ConfirmPassword) {
                return new AuthResultDto {
                    Errors = new List<string>() { "Confirm password doesn't match the password" }
                };
            }
            var ExistUser = await IsExistUser(registerDto.Username, registerDto.Email);
            if (ExistUser is not null) {
                return new AuthResultDto {
                    Errors = new List<string>() { ExistUser }
                };
            }

            var identityUser = new IdentityUser() {
                Email = registerDto.Email,
                UserName = registerDto.Username
            };

            var result = await _userManager.CreateAsync(identityUser, registerDto.Password);
            await SetRoleForUser(identityUser, AuthConstant.RoleCharity);

            if (result.Succeeded) {
                // Create basic user
                var basicUserInfoDto = _mapper.Map<RegisterBasicInfoDto>(registerDto);
                var userEntityModel = _mapper.Map<Charity>(basicUserInfoDto);
                userEntityModel.IdentityId = identityUser.Id;
                await _context.Charities.AddAsync(userEntityModel);

                if (await _context.SaveChangesAsync() >= 0) {
                    //var token = await _userManager.GenerateEmailConfirmationTokenAsync(identityUser);
                    //var confirmLink = Url
                    return new AuthResultDto {
                        Message = "Charity create successfully!"
                    };
                }
            }

            return new AuthResultDto {
                Message = "Charity create fail",
                Errors = result.Errors.Select(e => e.Description).ToList()
            };
        }

        public async Task<AuthResultDto> RegisterPlayerAsync(RegisterDto registerDto)
        {
            if (registerDto is null) {
                throw new NullReferenceException("Register info is null");
            }

            if (registerDto.Password != registerDto.ConfirmPassword) {
                return new AuthResultDto {
                    Errors = new List<string>() { "Confirm password doesn't match the password" }
                };
            }
            var ExistUser = await IsExistUser(registerDto.Username, registerDto.Email);
            if (ExistUser is not null) {
                return new AuthResultDto {
                    Errors = new List<string>() { ExistUser }
                };
            }

            var identityUser = new IdentityUser() {
                Email = registerDto.Email,
                UserName = registerDto.Username
            };

            var result = await _userManager.CreateAsync(identityUser, registerDto.Password);
            await SetRoleForUser(identityUser, AuthConstant.RolePlayer);

            if (result.Succeeded) {
                // Create basic user
                var basicUserInfoDto = _mapper.Map<RegisterBasicInfoDto>(registerDto);
                var userEntityModel = _mapper.Map<Player>(basicUserInfoDto);
                userEntityModel.IdentityId = identityUser.Id;
                await _context.Players.AddAsync(userEntityModel);

                if (await _context.SaveChangesAsync() >= 0) {
                    //var token = await _userManager.GenerateEmailConfirmationTokenAsync(identityUser);
                    //var confirmLink = Url
                    return new AuthResultDto {
                        Message = "Player create successfully!"
                    };
                }
            }

            return new AuthResultDto {
                Message = "Player create fail",
                Errors = result.Errors.Select(e => e.Description).ToList()
            };
        }

        public async Task<AuthResultDto> RegisterHirerAsync(RegisterDto registerDto)
        {
            if (registerDto is null) {
                throw new NullReferenceException("Register info is null");
            }

            if (registerDto.Password != registerDto.ConfirmPassword) {
                return new AuthResultDto {
                    Errors = new List<string>() { "Confirm password doesn't match the password" }
                };
            }
            var ExistUser = await IsExistUser(registerDto.Username, registerDto.Email);
            if (ExistUser is not null) {
                return new AuthResultDto {
                    Errors = new List<string>() { ExistUser }
                };
            }

            var identityUser = new IdentityUser() {
                Email = registerDto.Email,
                UserName = registerDto.Username
            };

            var result = await _userManager.CreateAsync(identityUser, registerDto.Password);
            await SetRoleForUser(identityUser, AuthConstant.RoleHirer);

            if (result.Succeeded) {
                // Create basic user
                var basicUserInfoDto = _mapper.Map<RegisterBasicInfoDto>(registerDto);
                var userEntityModel = _mapper.Map<Hirer>(basicUserInfoDto);
                userEntityModel.IdentityId = identityUser.Id;
                await _context.Hirers.AddAsync(userEntityModel);

                if (await _context.SaveChangesAsync() >= 0) {
                    //var token = await _userManager.GenerateEmailConfirmationTokenAsync(identityUser);
                    //var confirmLink = Url
                    return new AuthResultDto {
                        Message = "Hirer create successfully!"
                    };
                }
            }

            return new AuthResultDto {
                Message = "Hirer create fail",
                Errors = result.Errors.Select(e => e.Description).ToList()
            };
        }

        private async Task<bool> IsAdminOrCharity(IdentityUser user)
        {
            // return role
            var roles = await _userManager.GetRolesAsync(user);

            foreach (var item in roles) {
                if (item == AuthConstant.RoleAdmin || item == AuthConstant.RoleCharity) {
                    return true;
                }
            }

            return false;
        }

        private async Task<bool> IsCharity(IdentityUser user)
        {
            // return role
            var roles = await _userManager.GetRolesAsync(user);

            foreach (var item in roles) {
                if (item == AuthConstant.RoleCharity) {
                    return true;
                }
            }

            return false;
        }

        private async Task SetRoleForUser(IdentityUser identityUser, string role)
        {
            if (!await _roleManager.RoleExistsAsync(role)) {
                await _roleManager.CreateAsync(new IdentityRole(role));
            }

            if (await _roleManager.RoleExistsAsync(role)) {
                await _userManager.AddToRoleAsync(identityUser, role);
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