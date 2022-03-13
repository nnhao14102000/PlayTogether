using AutoMapper;
using Microsoft.AspNetCore.Identity;
using Microsoft.EntityFrameworkCore;
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
                throw new ArgumentNullException("Vui lòng nhập email!");
            }
            return await IsExistEmail(email);
        }

        public async Task<AuthResult> LoginUserAsync(LoginRequest loginDto)
        {
            var user = await _userManager.FindByEmailAsync(loginDto.Email);

            if (user is null) {
                return new AuthResult {
                    Errors = new List<string>() { "Không tìm thấy email!" }
                };
            }

            if (!await IsAdminOrCharity(user)) {
                if (!user.EmailConfirmed) {
                    return new AuthResult {
                        Errors = new List<string>() { "Vui lòng xác nhận email!" }
                    };
                }
            }

            var hirer = await _context.Hirers.FirstOrDefaultAsync(x => x.IdentityId == user.Id);
            var player = await _context.Players.FirstOrDefaultAsync(x => x.IdentityId == user.Id);
            var charity = await _context.Charities.FirstOrDefaultAsync(x => x.IdentityId == user.Id);
            var admin = await _context.Admins.FirstOrDefaultAsync(x => x.IdentityId == user.Id);

            if (hirer is not null) {
                if (hirer.IsActive == false && hirer.UpdateDate > DateTime.Now.AddDays(-1)) {
                    return new AuthResult {
                        Errors = new List<string>() { $"Tài khoản đã bị khóa, bạn có thể đăng nhập lại sau {hirer.UpdateDate - DateTime.Now.AddDays(-1)}" }
                    };
                }
                else if (hirer.IsActive == false && hirer.UpdateDate <= DateTime.Now.AddDays(-1)) {
                    hirer.Status = HirerStatusConstants.Online;
                    hirer.IsActive = true;
                    await _context.SaveChangesAsync();
                }
                else {
                    hirer.Status = HirerStatusConstants.Online;
                    await _context.SaveChangesAsync();
                }
            }

            if (player is not null) {
                if (player.IsActive == false && player.UpdateDate > DateTime.Now.AddDays(-1)) {
                    return new AuthResult {
                        Errors = new List<string>() { $"Tài khoản đã bị khóa, bạn có thể đăng nhập lại sau {player.UpdateDate - DateTime.Now.AddDays(-1)}" }
                    };
                }
                else if (player.IsActive == false && player.UpdateDate <= DateTime.Now.AddDays(-1)) {
                    player.Status = PlayerStatusConstants.Online;
                    player.IsActive = true;
                    await _context.SaveChangesAsync();
                }
                else {
                    player.Status = PlayerStatusConstants.Online;
                    await _context.SaveChangesAsync();
                }
            }

            if (charity is not null) {
                if (charity.IsActive == false) {
                    return new AuthResult {
                        Errors = new List<string>() { "Tài khoản đã bị khóa." }
                    };
                }
            }

            if (admin is not null) {
                if (admin.IsActive == false) {
                    return new AuthResult {
                        Errors = new List<string>() { "Tài khoản đã bị khóa." }
                    };
                }
            }

            var result = await _userManager.CheckPasswordAsync(user, loginDto.Password);

            if (!result) {
                return new AuthResult {
                    Errors = new List<string>() { "Sai Password. Vui lòng kiểm tra và thử lại!" }
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
                    Errors = new List<string>() { "Lỗi xác thực với Google!" }
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

                var hirer = await _context.Hirers.FirstOrDefaultAsync(x => x.IdentityId == user.Id);
                hirer.Status = HirerStatusConstants.Online;
                await _context.SaveChangesAsync();

                var token = await GenerateToken(user);
                return new AuthResult {
                    Message = token[0],
                    ExpireDate = DateTime.Parse(token[1])
                };
            }

            return new AuthResult {
                Errors = new List<string>() { "Lỗi xác thực với Google!" }
            };
        }

        public async Task<AuthResult> LoginPlayerByGoogleAsync(GoogleLoginRequest loginEmailDto)
        {
            var payload = PayloadInfo(loginEmailDto.IdToken);

            if (payload is null) {
                return new AuthResult {
                    Errors = new List<string>() { "Lỗi xác thực với Google!" }
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

                var player = await _context.Players.FirstOrDefaultAsync(x => x.IdentityId == user.Id);
                player.Status = PlayerStatusConstants.Online;
                await _context.SaveChangesAsync();

                var token = await GenerateToken(user);
                return new AuthResult {
                    Message = token[0],
                    ExpireDate = DateTime.Parse(token[1])
                };
            }

            return new AuthResult {
                Errors = new List<string>() { "Lỗi xác thực với Google!" }
            };
        }

        public async Task<AuthResult> RegisterAdminAsync(RegisterAdminInfoRequest registerDto)
        {
            if (await IsExistEmail(registerDto.Email)) {
                return new AuthResult {
                    Errors = new List<string>() { "Tài khoản đã tồn tại." }
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
                        Message = "Tạo tài khoản Administrator thành công!"
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
                    Errors = new List<string>() { "Tài khoản đã tồn tại." }
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
                        Message = "Tạo tài khoản tổ chức từ thiện thành công!"
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
                    Errors = new List<string>() { "Tài khoản đã tồn tại." }
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
                        Message = "Tạo tài khoản Player thành công!"
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
                    userEntityModel.Balance = 1000000;

                    await _context.Players.AddAsync(userEntityModel);

                    if (await _context.SaveChangesAsync() < 0) {
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
                    Errors = new List<string>() { "Tài khoản đã tồn tại." }
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
                userEntityModel.Status = HirerStatusConstants.Offline;

                await _context.Hirers.AddAsync(userEntityModel);

                if (await _context.SaveChangesAsync() >= 0) {
                    //var token = await _userManager.GenerateEmailConfirmationTokenAsync(identityUser);
                    //var confirmLink = Url
                    return new AuthResult {
                        Message = "Tài khoản Hirer đã tạo thành công!"
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
                    userEntityModel.Balance = 1000000;
                    userEntityModel.Status = HirerStatusConstants.Online;

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

        public async Task<AuthResult> LogoutAsync(ClaimsPrincipal principal)
        {
            var loggedInUser = await _userManager.GetUserAsync(principal);
            if (loggedInUser is null) {
                return new AuthResult {
                    Errors = new List<string>() { "Bạn chưa đăng nhập." }
                };
            }
            var identityId = loggedInUser.Id;

            var hirer = await _context.Hirers.FirstOrDefaultAsync(x => x.IdentityId == identityId);
            var player = await _context.Players.FirstOrDefaultAsync(x => x.IdentityId == identityId);

            if (hirer is not null) {
                hirer.Status = HirerStatusConstants.Offline;
                await _context.SaveChangesAsync();
                return new AuthResult {
                    Message = "Đăng xuất thành cồng!"
                };
            }

            if (player is not null) {
                player.Status = PlayerStatusConstants.Offline;
                await _context.SaveChangesAsync();
                return new AuthResult {
                    Message = "Đăng xuất thành cồng!"
                };
            }
            return new AuthResult {
                Errors = new List<string>() { "Lỗi đăng xuất." }
            };
        }

        public async Task<AuthResult> ChangePasswordAsync(ChangePasswordRequest request)
        {
            var user = await _userManager.FindByEmailAsync(request.Email);
            if (user is null) {
                return new AuthResult {
                    Errors = new List<string>() { "Tài khoản không tồn tại." }
                };
            };

            var result = await _userManager.ChangePasswordAsync(user, request.CurrentPassword, request.NewPassword);
            if (!result.Succeeded) {
                return new AuthResult {
                    Errors = new List<string>() { "Có lỗi xảy ra, đổi mật khẩu không thành công!" }
                };
            }
            return new AuthResult {
                Message = "Đổi mật khẩu thành công!"
            };
        }

        public async Task<AuthResult> ResetPasswordAdminAsync(ResetPasswordAdminRequest request)
        {
            var user = await _userManager.FindByEmailAsync(request.Email);
            if (user is null) {
                return new AuthResult {
                    Errors = new List<string>() { "Tài khoản không tồn tại." }
                };
            };

            var token = await _userManager.GeneratePasswordResetTokenAsync(user);
            var result = await _userManager.ResetPasswordAsync(user, token, request.NewPassword);
            if (!result.Succeeded) {
                return new AuthResult {
                    Errors = new List<string>() { "Có lỗi xảy ra, reset mật khẩu không thành công!" }
                };
            }
            return new AuthResult {
                Message = "Reset mật khẩu thành công!"
            };
        }

        public async Task<AuthResult> ResetPasswordTokenAsync(ResetPasswordTokenRequest request)
        {
            var user = await _userManager.FindByEmailAsync(request.Email);
            if (user is null) {
                return new AuthResult {
                    Errors = new List<string>() { "Tài khoản không tồn tại." }
                };
            };
            var token = await _userManager.GeneratePasswordResetTokenAsync(user);

            return new AuthResult {
                Message = token
            };
        }

        public async Task<AuthResult> ResetPasswordAsync(ResetPasswordRequest request)
        {
            var user = await _userManager.FindByEmailAsync(request.Email);
            if (user is null) {
                return new AuthResult {
                    Errors = new List<string>() { "Tài khoản không tồn tại." }
                };
            };

            var result = await _userManager.ResetPasswordAsync(user, request.Token, request.NewPassword);
            if (!result.Succeeded) {
                return new AuthResult {
                    Errors = new List<string>() { "Có lỗi xảy ra, reset mật khẩu không thành công!" }
                };
            }
            return new AuthResult {
                Message = "Reset mật khẩu thành công!"
            };
        }
    }
}