using AutoMapper;
using Microsoft.AspNetCore.Identity;
using Microsoft.Extensions.Configuration;
using Microsoft.IdentityModel.Tokens;
using PlayTogether.Core.Dtos.Incoming.Auth;
using PlayTogether.Core.Dtos.Incoming.Generic;
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
    public class AccountRepository : IAccountRepository
    {
        private UserManager<IdentityUser> _userManager;
        private RoleManager<IdentityRole> _roleManager;
        private readonly IConfiguration _configuration;
        private readonly IConfigurationSection _jwtSettings;
        private readonly IMapper _mapper;
        private readonly AppDbContext _context;

        public AccountRepository(
            UserManager<IdentityUser> userManager,
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

        public async Task<bool> CheckExistEmailAsync(string email)
        {
            if (email is null) {
                throw new ArgumentNullException("Email is null");
            }
            return await IsExistEmail(email);
        }

        public async Task<AuthResult> LoginUserAsync(LoginRequest loginDto)
        {
            var user = await _userManager.FindByEmailAsync(loginDto.Email);

            if (user is null) {
                return new AuthResult {
                    Errors = new List<string>() { "Email is not found" }
                };
            }

            if (!await IsAdminOrCharity(user)) {
                if (!user.EmailConfirmed) {
                    return new AuthResult {
                        Errors = new List<string>() { "User is not comfirm email" }
                    };
                }
            }

            var hirer = _context.Hirers.FirstOrDefault(x => x.IdentityId == user.Id);
            var player = _context.Players.FirstOrDefault(x => x.IdentityId == user.Id);
            var charity = _context.Charities.FirstOrDefault(x => x.IdentityId == user.Id);
            var admin = _context.Admins.FirstOrDefault(x => x.IdentityId == user.Id);
            if (hirer is not null) {
                if (hirer.IsActive == false) {
                    return new AuthResult {
                        Errors = new List<string>() { "Account is disable" }
                    };
                }
            }

            if (player is not null) {
                if (player.IsActive == false) {
                    return new AuthResult {
                        Errors = new List<string>() { "Account is disable" }
                    };
                }
            }

            if (charity is not null) {
                if (charity.IsActive == false) {
                    return new AuthResult {
                        Errors = new List<string>() { "Account is disable" }
                    };
                }
            }

            if (admin is not null) {
                if (admin.IsActive == false) {
                    return new AuthResult {
                        Errors = new List<string>() { "Account is disable" }
                    };
                }
            }

            var result = await _userManager.CheckPasswordAsync(user, loginDto.Password);

            if (!result) {
                return new AuthResult {
                    Errors = new List<string>() { "Invalid password" }
                };
            }

            var token = await GenerateToken(user);

            return new AuthResult {
                Message = token[0],
                ExpireDate = DateTime.Parse(token[1])
            };
        }

        public async Task<AuthResult> LoginHirerByGoogleAsync(GoogleLoginRequest loginEmailDto)
        {
            var payload = PayloadInfo(loginEmailDto.IdToken);

            if (payload is null) {
                return new AuthResult {
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
                        UserName = payload["email"].ToString(),
                        EmailConfirmed = Convert.ToBoolean(payload["email_verified"].ToString())
                    };

                    var result = await _userManager.CreateAsync(user);
                    await SetRoleForUser(user, AuthConstant.RoleHirer);
                    await _userManager.AddLoginAsync(user, info);

                    if (result.Succeeded) {
                        var userEntityModel = new Hirer();
                        userEntityModel.IdentityId = user.Id;
                        userEntityModel.Firstname = payload["given_name"].ToString();
                        userEntityModel.Lastname = payload["family_name"].ToString();
                        userEntityModel.Email = payload["email"].ToString();
                        userEntityModel.Avatar = payload["picture"].ToString();
                        userEntityModel.CreatedDate = DateTime.Now;

                        await _context.Hirers.AddAsync(userEntityModel);

                        if (await _context.SaveChangesAsync() >= 0) {
                            var token = await GenerateToken(user);
                            return new AuthResult {
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
                return new AuthResult {
                    Message = token[0],
                    ExpireDate = DateTime.Parse(token[1])
                };
            }

            return new AuthResult {
                Errors = new List<string>() { "Invalid google authentication" }
            };
        }

        public async Task<AuthResult> LoginPlayerByGoogleAsync(GoogleLoginRequest loginEmailDto)
        {
            var payload = PayloadInfo(loginEmailDto.IdToken);

            if (payload is null) {
                return new AuthResult {
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
                        UserName = payload["email"].ToString(),
                        EmailConfirmed = Convert.ToBoolean(payload["email_verified"].ToString())
                    };

                    var result = await _userManager.CreateAsync(user);
                    await SetRoleForUser(user, AuthConstant.RolePlayer);
                    await _userManager.AddLoginAsync(user, info);

                    if (result.Succeeded) {
                        var userEntityModel = new Player();
                        userEntityModel.IdentityId = user.Id;
                        userEntityModel.Firstname = payload["given_name"].ToString();
                        userEntityModel.Lastname = payload["family_name"].ToString();
                        userEntityModel.Email = payload["email"].ToString();
                        userEntityModel.Avatar = payload["picture"].ToString();
                        userEntityModel.CreatedDate = DateTime.Now;

                        await _context.Players.AddAsync(userEntityModel);

                        if (await _context.SaveChangesAsync() >= 0) {
                            var token = await GenerateToken(user);
                            return new AuthResult {
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
                return new AuthResult {
                    Message = token[0],
                    ExpireDate = DateTime.Parse(token[1])
                };
            }

            return new AuthResult {
                Errors = new List<string>() { "Invalid google authentication" }
            };
        }

        public async Task<AuthResult> RegisterAdminAsync(RegisterAdminInfoRequest registerDto)
        {
            if (await IsExistEmail(registerDto.Email)) {
                return new AuthResult {
                    Errors = new List<string>() { "This user is exist" }
                };
            }

            var identityUser = new IdentityUser() {
                Email = registerDto.Email,
                UserName = registerDto.Email
            };

            var result = await _userManager.CreateAsync(identityUser, registerDto.Password);
            await SetRoleForUser(identityUser, AuthConstant.RoleAdmin);

            if (result.Succeeded) {
                var userEntityModel = _mapper.Map<Admin>(registerDto);
                userEntityModel.IdentityId = identityUser.Id;
                userEntityModel.CreatedDate = DateTime.Now;
                userEntityModel.Firstname = registerDto.Firstname;
                userEntityModel.Lastname = registerDto.Lastname;

                await _context.Admins.AddAsync(userEntityModel);
                var saveSuccess = await _context.SaveChangesAsync() >= 0;
                if (saveSuccess) {
                    //var token = await _userManager.GenerateEmailConfirmationTokenAsync(identityUser);
                    //var confirmLink = Url
                    return new AuthResult {
                        Message = "Admin create successfully!"
                    };
                }
            }

            return new AuthResult {
                Errors = result.Errors.Select(e => e.Description).ToList()
            };
        }

        public async Task<AuthResult> RegisterCharityAsync(RegisterCharityInfoRequest registerDto)
        {
            if (await IsExistEmail(registerDto.Email)) {
                return new AuthResult {
                    Errors = new List<string>() { "This user is exist" }
                };
            }

            var identityUser = new IdentityUser() {
                Email = registerDto.Email,
                UserName = registerDto.Email
            };

            var result = await _userManager.CreateAsync(identityUser, registerDto.Password);
            await SetRoleForUser(identityUser, AuthConstant.RoleCharity);

            if (result.Succeeded) {
                var userEntityModel = _mapper.Map<Charity>(registerDto);
                userEntityModel.IdentityId = identityUser.Id;
                userEntityModel.CreatedDate = DateTime.Now;
                userEntityModel.OrganizationName = registerDto.OrganizationName;

                await _context.Charities.AddAsync(userEntityModel);

                if (await _context.SaveChangesAsync() >= 0) {
                    //var token = await _userManager.GenerateEmailConfirmationTokenAsync(identityUser);
                    //var confirmLink = Url
                    return new AuthResult {
                        Message = "Charity create successfully!"
                    };
                }
            }

            return new AuthResult {
                Errors = result.Errors.Select(e => e.Description).ToList()
            };
        }

        public async Task<AuthResult> RegisterPlayerAsync(RegisterUserInfoRequest registerDto)
        {
            if (await IsExistEmail(registerDto.Email)) {
                return new AuthResult {
                    Errors = new List<string>() { "This user is exist" }
                };
            }

            var identityUser = new IdentityUser() {
                Email = registerDto.Email,
                UserName = registerDto.Email,
                EmailConfirmed = registerDto.ConfirmEmail
            };

            var result = await _userManager.CreateAsync(identityUser, registerDto.Password);
            await SetRoleForUser(identityUser, AuthConstant.RolePlayer);

            if (result.Succeeded) {
                // Create basic user
                var userEntityModel = _mapper.Map<Player>(registerDto);
                userEntityModel.IdentityId = identityUser.Id;
                userEntityModel.CreatedDate = DateTime.Now;

                userEntityModel.Firstname = registerDto.Firstname;
                userEntityModel.Lastname = registerDto.Lastname;
                userEntityModel.City = registerDto.City;
                userEntityModel.DateOfBirth = registerDto.DateOfBirth;
                userEntityModel.Gender = registerDto.Gender;
                userEntityModel.Status = PlayerStatusConstants.NotAcceptPolicy;

                await _context.Players.AddAsync(userEntityModel);

                if (await _context.SaveChangesAsync() >= 0) {
                    //var token = await _userManager.GenerateEmailConfirmationTokenAsync(identityUser);
                    //var confirmLink = Url
                    return new AuthResult {
                        Message = "Player create successfully!"
                    };
                }
            }

            return new AuthResult {
                Errors = result.Errors.Select(e => e.Description).ToList()
            };
        }

        public async Task<bool> RegisterMultiPlayerAsync(List<RegisterUserInfoRequest> registerDtos)
        {
            foreach (var registerDto in registerDtos) {
                if (await IsExistEmail(registerDto.Email)) {
                    return false;
                }

                var identityUser = new IdentityUser() {
                    Email = registerDto.Email,
                    UserName = registerDto.Email,
                    EmailConfirmed = registerDto.ConfirmEmail
                };

                var result = await _userManager.CreateAsync(identityUser, registerDto.Password);
                await SetRoleForUser(identityUser, AuthConstant.RolePlayer);

                if (result.Succeeded) {
                    // Create basic user
                    var userEntityModel = _mapper.Map<Player>(registerDto);
                    userEntityModel.IdentityId = identityUser.Id;
                    userEntityModel.CreatedDate = DateTime.Now;

                    userEntityModel.Firstname = registerDto.Firstname;
                    userEntityModel.Lastname = registerDto.Lastname;
                    userEntityModel.City = registerDto.City;
                    userEntityModel.DateOfBirth = registerDto.DateOfBirth;
                    userEntityModel.Gender = registerDto.Gender;
                    userEntityModel.Status = PlayerStatusConstants.Online;

                    await _context.Players.AddAsync(userEntityModel);

                    if (await _context.SaveChangesAsync() < 0) {
                        //var token = await _userManager.GenerateEmailConfirmationTokenAsync(identityUser);
                        //var confirmLink = Url
                        return false;
                    }
                }
            }
            return true;
        }

        public async Task<AuthResult> RegisterHirerAsync(RegisterUserInfoRequest registerDto)
        {
            if (await IsExistEmail(registerDto.Email)) {
                return new AuthResult {
                    Errors = new List<string>() { "This user is exist" }
                };
            }

            var identityUser = new IdentityUser() {
                Email = registerDto.Email,
                UserName = registerDto.Email,
                EmailConfirmed = registerDto.ConfirmEmail
            };

            var result = await _userManager.CreateAsync(identityUser, registerDto.Password);
            await SetRoleForUser(identityUser, AuthConstant.RoleHirer);

            if (result.Succeeded) {
                // Create basic user
                var userEntityModel = _mapper.Map<Hirer>(registerDto);
                userEntityModel.IdentityId = identityUser.Id;
                userEntityModel.CreatedDate = DateTime.Now;

                userEntityModel.Firstname = registerDto.Firstname;
                userEntityModel.Lastname = registerDto.Lastname;
                userEntityModel.City = registerDto.City;
                userEntityModel.DateOfBirth = registerDto.DateOfBirth;
                userEntityModel.Gender = registerDto.Gender;

                await _context.Hirers.AddAsync(userEntityModel);

                if (await _context.SaveChangesAsync() >= 0) {
                    //var token = await _userManager.GenerateEmailConfirmationTokenAsync(identityUser);
                    //var confirmLink = Url
                    return new AuthResult {
                        Message = "Hirer create successfully!"
                    };
                }
            }

            return new AuthResult {
                Errors = result.Errors.Select(e => e.Description).ToList()
            };
        }

        public async Task<bool> RegisterMultiHirerAsync(List<RegisterUserInfoRequest> registerDtos)
        {
            foreach (var registerDto in registerDtos) {
                if (await IsExistEmail(registerDto.Email)) {
                    return false;
                }

                var identityUser = new IdentityUser() {
                    Email = registerDto.Email,
                    UserName = registerDto.Email,
                    EmailConfirmed = registerDto.ConfirmEmail
                };

                var result = await _userManager.CreateAsync(identityUser, registerDto.Password);
                await SetRoleForUser(identityUser, AuthConstant.RoleHirer);

                if (result.Succeeded) {
                    // Create basic user
                    var userEntityModel = _mapper.Map<Hirer>(registerDto);
                    userEntityModel.IdentityId = identityUser.Id;
                    userEntityModel.CreatedDate = DateTime.Now;

                    userEntityModel.Firstname = registerDto.Firstname;
                    userEntityModel.Lastname = registerDto.Lastname;
                    userEntityModel.City = registerDto.City;
                    userEntityModel.DateOfBirth = registerDto.DateOfBirth;
                    userEntityModel.Gender = registerDto.Gender;

                    await _context.Hirers.AddAsync(userEntityModel);

                    if ((await _context.SaveChangesAsync() < 0)) {
                        return false;
                    }
                }
            }
            return true;
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

        private async Task SetRoleForUser(IdentityUser identityUser, string role)
        {
            if (!await _roleManager.RoleExistsAsync(role)) {
                await _roleManager.CreateAsync(new IdentityRole(role));
            }

            if (await _roleManager.RoleExistsAsync(role)) {
                await _userManager.AddToRoleAsync(identityUser, role);
            }
        }

        private async Task<bool> IsExistEmail(string email)
        {
            var existEmail = await _userManager.FindByEmailAsync(email);
            if (existEmail is not null) {
                return true;
            }
            return false;
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
                expires: DateTime.Now.AddDays(timeExpire),
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