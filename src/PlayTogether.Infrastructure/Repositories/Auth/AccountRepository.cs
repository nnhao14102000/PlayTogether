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
using PlayTogether.Core.Entities;
using System;
using System.Collections.Generic;
using System.IdentityModel.Tokens.Jwt;
using System.Linq;
using System.Security.Claims;
using System.Text;
using System.Threading.Tasks;

namespace PlayTogether.Infrastructure.Repositories.Auth
{
    public class AccountRepository : BaseRepository, IAccountRepository
    {
        private UserManager<IdentityUser> _userManager;
        private RoleManager<IdentityRole> _roleManager;
        private readonly IConfiguration _configuration;
        private readonly IConfigurationSection _jwtSettings;

        public AccountRepository(
            IMapper mapper,
            AppDbContext context,
            UserManager<IdentityUser> userManager,
            RoleManager<IdentityRole> roleManager,
            IConfiguration configuration) : base(mapper, context)
        {
            _userManager = userManager;
            _roleManager = roleManager;
            _configuration = configuration;
            _jwtSettings = _configuration.GetSection("Jwt");
        }

        public async Task<bool> CheckExistEmailAsync(string email)
        {
            if (email is null) {
                throw new ArgumentNullException("Vui lòng nhập email!");
            }
            return await IsExistEmail(email);
        }

        public async Task<AuthResult> LoginUserAsync(LoginRequest request)
        {
            var user = await _userManager.FindByEmailAsync(request.Email);

            if (user is null) {
                return new AuthResult {
                    Errors = new List<string>() { "Tài khoản không tồn tại." }
                };
            }

            var appUser = await _context.AppUsers.FirstOrDefaultAsync(x => x.IdentityId == user.Id);

            if (appUser is not null) {
                if (appUser.IsActive == true) {
                    if (appUser.Status == UserStatusConstants.Offline) {
                        appUser.Status = UserStatusConstants.Online;
                        await _context.SaveChangesAsync();
                    }
                }
                else {
                    var disable = await _context.DisableUsers.FirstOrDefaultAsync(x => x.UserId == appUser.Id && x.IsActive == true);
                    if (disable is null) {
                        return new AuthResult {
                            Errors = new List<string>() { "Có lỗi xảy ra trong quá trình active tài khoản. Vui lòng liên hệ hỗ trợ qua email : PtoAdmin@contact.com" }
                        };
                    }
                    if (DateTime.UtcNow.AddHours(7) < disable.DateActive) {
                        // return new AuthResult {
                        //     Errors = new List<string>() { $"Tài khoản sẽ được active lúc: {disable.DateActive}" }
                        // };
                    }
                    else {
                        disable.IsActive = false;
                        appUser.IsActive = true;
                        await _context.SaveChangesAsync();
                    }
                }
            }
            else {
                return new AuthResult {
                    Errors = new List<string>() { "Tài khoản không tồn tại." }
                };
            }


            var result = await _userManager.CheckPasswordAsync(user, request.Password);

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

        public async Task<AuthResult> LoginCharityAsync(LoginRequest request)
        {
            var user = await _userManager.FindByEmailAsync(request.Email);

            if (user is null) {
                return new AuthResult {
                    Errors = new List<string>() { "Tài khoản không tồn tại." }
                };
            }

            var charity = await _context.Charities.FirstOrDefaultAsync(x => x.IdentityId == user.Id);

            if (charity is null) {
                return new AuthResult {
                    Errors = new List<string>() { "Tài khoản không tồn tại." }
                };
            }

            var result = await _userManager.CheckPasswordAsync(user, request.Password);

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

        public async Task<AuthResult> LoginAdminAsync(LoginRequest request)
        {
            var user = await _userManager.FindByEmailAsync(request.Email);

            if (user is null) {
                return new AuthResult {
                    Errors = new List<string>() { "Tài khoản không tồn tại." }
                };
            }

            if (!await IsAdmin(user)) {
                return new AuthResult {
                    Errors = new List<string>() { "Tài khoản không tồn tại." }
                };
            }

            var result = await _userManager.CheckPasswordAsync(user, request.Password);

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



        public async Task<AuthResult> LoginUserByGoogleAsync(GoogleLoginRequest request)
        {
            var payload = PayloadInfo(request.IdToken);

            if (payload is null) {
                return new AuthResult {
                    Errors = new List<string>() { "IdToken không hợp lệ." }
                };
            }

            var info = new UserLoginInfo(request.ProviderName, payload.Sub, request.ProviderName);

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
                    await SetRoleForUser(user, AuthConstant.RoleUser);
                    await _userManager.AddLoginAsync(user, info);

                    if (result.Succeeded) {
                        var userEntityModel = new AppUser();
                        userEntityModel.IdentityId = user.Id;
                        userEntityModel.Name = payload["family_name"].ToString() + " " + payload["given_name"].ToString();
                        userEntityModel.Email = payload["email"].ToString();
                        userEntityModel.Avatar = payload["picture"].ToString();
                        userEntityModel.CreatedDate = DateTime.UtcNow.AddHours(7);
                        userEntityModel.IsPlayer = false;
                        userEntityModel.Status = UserStatusConstants.Online;
                        userEntityModel.Description = "";
                        userEntityModel.PricePerHour = 10000;

                        await _context.AppUsers.AddAsync(userEntityModel);

                        if (await _context.SaveChangesAsync() >= 0) {

                            var userBalance = new UserBalance();
                            userBalance.UserId = userEntityModel.Id;
                            userBalance.CreatedDate = DateTime.UtcNow.AddHours(7);
                            userBalance.UpdateDate = null;
                            userBalance.Balance = 0;
                            userBalance.ActiveBalance = 0;
                            await _context.UserBalances.AddAsync(userBalance);

                            var userPoint = new BehaviorPoint();
                            userPoint.UserId = userEntityModel.Id;
                            userPoint.CreatedDate = DateTime.UtcNow.AddHours(7);
                            userPoint.UpdateDate = null;
                            userPoint.Point = 100;
                            userPoint.SatisfiedPoint = 100;
                            await _context.BehaviorPoints.AddAsync(userPoint);


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
            }
            else {
                await _userManager.AddLoginAsync(user, info);

                var appUser = await _context.AppUsers.FirstOrDefaultAsync(x => x.IdentityId == user.Id);

                if (appUser is not null) {
                    if (appUser.IsActive == true) {
                        if (appUser.Status == UserStatusConstants.Offline) {
                            appUser.Status = UserStatusConstants.Online;
                            await _context.SaveChangesAsync();
                        }
                    }
                    else {
                        var disable = await _context.DisableUsers.FirstOrDefaultAsync(x => x.UserId == appUser.Id && x.IsActive == true);
                        if (disable is null) {
                            return new AuthResult {
                                Errors = new List<string>() { "Có lỗi xảy ra trong quá trình active tài khoản. Vui lòng liên hệ hỗ trợ qua email : PtoAdmin@contact.com" }
                            };
                        }
                        if (DateTime.UtcNow.AddHours(7) < disable.DateActive) {
                            // return new AuthResult {
                            //     Errors = new List<string>() { $"Tài khoản sẽ được active lúc: {disable.DateActive}" }
                            // };
                        }
                        else {
                            disable.IsActive = false;
                            appUser.IsActive = true;
                            await _context.SaveChangesAsync();
                        }
                    }
                }
                else {
                    return new AuthResult {
                        Errors = new List<string>() { "Tài khoản không tồn tại." }
                    };
                }

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

        public async Task<AuthResult> RegisterAdminAsync(RegisterAdminInfoRequest request)
        {
            if (await IsExistEmail(request.Email)) {
                return new AuthResult {
                    Errors = new List<string>() { "Tài khoản đã tồn tại." }
                };
            }

            var identityUser = new IdentityUser() {
                Email = request.Email,
                UserName = request.Email
            };

            var result = await _userManager.CreateAsync(identityUser, request.Password);
            await SetRoleForUser(identityUser, AuthConstant.RoleAdmin);

            if (result.Succeeded) {
                return new AuthResult {
                    Message = "Tạo tài khoản Administrator thành công!"
                };
            }

            return new AuthResult {
                Errors = result.Errors.Select(e => e.Description).ToList()
            };
        }

        public async Task<AuthResult> RegisterCharityAsync(RegisterCharityInfoRequest request)
        {
            if (await IsExistEmail(request.Email)) {
                return new AuthResult {
                    Errors = new List<string>() { "Tài khoản đã tồn tại." }
                };
            }

            var identityUser = new IdentityUser() {
                Email = request.Email,
                UserName = request.Email
            };

            var result = await _userManager.CreateAsync(identityUser, request.Password);
            await SetRoleForUser(identityUser, AuthConstant.RoleCharity);

            if (result.Succeeded) {
                var userEntityModel = _mapper.Map<Charity>(request);
                userEntityModel.IdentityId = identityUser.Id;
                userEntityModel.CreatedDate = DateTime.UtcNow.AddHours(7);
                userEntityModel.OrganizationName = request.OrganizationName;
                userEntityModel.Avatar = ValueConstants.DefaultAvatar;

                await _context.Charities.AddAsync(userEntityModel);

                if (await _context.SaveChangesAsync() >= 0) {
                    return new AuthResult {
                        Message = "Tạo tài khoản tổ chức từ thiện thành công!"
                    };
                }
            }

            return new AuthResult {
                Errors = result.Errors.Select(e => e.Description).ToList()
            };
        }

        public async Task<AuthResult> RegisterUserAsync(RegisterUserInfoRequest request)
        {
            if (await IsExistEmail(request.Email)) {
                return new AuthResult {
                    Errors = new List<string>() { "Tài khoản đã tồn tại." }
                };
            }

            var identityUser = new IdentityUser() {
                Email = request.Email,
                UserName = request.Email,
                EmailConfirmed = request.ConfirmEmail
            };

            var result = await _userManager.CreateAsync(identityUser, request.Password);
            await SetRoleForUser(identityUser, AuthConstant.RoleUser);

            if (result.Succeeded) {
                var userEntityModel = _mapper.Map<AppUser>(request);
                userEntityModel.IdentityId = identityUser.Id;
                userEntityModel.CreatedDate = DateTime.UtcNow.AddHours(7);

                userEntityModel.Name = request.Name;
                userEntityModel.Email = request.Email;
                userEntityModel.City = request.City;
                userEntityModel.DateOfBirth = request.DateOfBirth;
                userEntityModel.Gender = request.Gender;
                userEntityModel.Status = UserStatusConstants.Online;
                userEntityModel.Avatar = ValueConstants.DefaultAvatar;
                userEntityModel.IsPlayer = false;
                userEntityModel.Description = "";
                userEntityModel.PricePerHour = 10000;

                await _context.AppUsers.AddAsync(userEntityModel);

                if (await _context.SaveChangesAsync() >= 0) {

                    var userBalance = new UserBalance();
                    userBalance.UserId = userEntityModel.Id;
                    userBalance.CreatedDate = DateTime.UtcNow.AddHours(7);
                    userBalance.UpdateDate = null;
                    userBalance.Balance = 0;
                    userBalance.ActiveBalance = 0;
                    await _context.UserBalances.AddAsync(userBalance);

                    var userPoint = new BehaviorPoint();
                    userPoint.UserId = userEntityModel.Id;
                    userPoint.CreatedDate = DateTime.UtcNow.AddHours(7);
                    userPoint.UpdateDate = null;
                    userPoint.Point = 100;
                    userPoint.SatisfiedPoint = 100;
                    await _context.BehaviorPoints.AddAsync(userPoint);

                    if (await _context.SaveChangesAsync() >= 0) {
                        return new AuthResult {
                            Message = "Tạo tài khoản User thành công!"
                        };
                    }
                }
            }

            return new AuthResult {
                Errors = result.Errors.Select(e => e.Description).ToList()
            };
        }

        public async Task<bool> RegisterMultiUserAsync(List<RegisterUserInfoRequest> request)
        {
            foreach (var registerDto in request) {
                if (await IsExistEmail(registerDto.Email)) {
                    return false;
                }

                var identityUser = new IdentityUser() {
                    Email = registerDto.Email,
                    UserName = registerDto.Email,
                    EmailConfirmed = registerDto.ConfirmEmail
                };

                var result = await _userManager.CreateAsync(identityUser, registerDto.Password);
                await SetRoleForUser(identityUser, AuthConstant.RoleUser);

                if (result.Succeeded) {
                    // Create basic user
                    var userEntityModel = _mapper.Map<AppUser>(registerDto);
                    // await _context.Entry(userEntityModel).Reference(x => x.UserBalance).LoadAsync();

                    userEntityModel.IdentityId = identityUser.Id;
                    userEntityModel.CreatedDate = DateTime.UtcNow.AddHours(7);

                    userEntityModel.Name = registerDto.Name;
                    userEntityModel.Email = registerDto.Email;
                    userEntityModel.City = registerDto.City;
                    userEntityModel.DateOfBirth = registerDto.DateOfBirth;
                    userEntityModel.Gender = registerDto.Gender;
                    userEntityModel.Status = UserStatusConstants.Online;
                    userEntityModel.Avatar = ValueConstants.DefaultAvatar;

                    await _context.AppUsers.AddAsync(userEntityModel);

                    if (await _context.SaveChangesAsync() >= 0) {
                        var userBalance = new UserBalance();
                        userBalance.UserId = userEntityModel.Id;
                        userBalance.CreatedDate = DateTime.UtcNow.AddHours(7);
                        userBalance.UpdateDate = null;
                        userBalance.Balance = 1000000;
                        userBalance.ActiveBalance = 1000000;
                        await _context.UserBalances.AddAsync(userBalance);

                        if (await _context.SaveChangesAsync() < 0) {
                            return false;
                        }
                    }
                }
            }
            return true;
        }

        public async Task<bool> RegisterMultiUserIsPlayerAsync(List<RegisterUserInfoRequest> request)
        {
            foreach (var registerDto in request) {
                if (await IsExistEmail(registerDto.Email)) {
                    return false;
                }

                var identityUser = new IdentityUser() {
                    Email = registerDto.Email,
                    UserName = registerDto.Email,
                    EmailConfirmed = registerDto.ConfirmEmail
                };

                var result = await _userManager.CreateAsync(identityUser, registerDto.Password);
                await SetRoleForUser(identityUser, AuthConstant.RoleUser);

                if (result.Succeeded) {
                    // Create basic user
                    var userEntityModel = _mapper.Map<AppUser>(registerDto);
                    // await _context.Entry(userEntityModel).Reference(x => x.UserBalance).LoadAsync();

                    userEntityModel.IdentityId = identityUser.Id;
                    userEntityModel.CreatedDate = DateTime.UtcNow.AddHours(7);

                    userEntityModel.Name = registerDto.Name;
                    userEntityModel.Email = registerDto.Email;
                    userEntityModel.City = registerDto.City;
                    userEntityModel.DateOfBirth = registerDto.DateOfBirth;
                    userEntityModel.Gender = registerDto.Gender;
                    userEntityModel.Status = UserStatusConstants.Online;
                    userEntityModel.Avatar = ValueConstants.DefaultAvatar;
                    userEntityModel.IsPlayer = true;
                    userEntityModel.PricePerHour = 10000;
                    userEntityModel.MaxHourHire = 5;
                    userEntityModel.Description = "Default";

                    await _context.AppUsers.AddAsync(userEntityModel);

                    if (await _context.SaveChangesAsync() >= 0) {
                        var userBalance = new UserBalance();
                        userBalance.UserId = userEntityModel.Id;
                        userBalance.CreatedDate = DateTime.UtcNow.AddHours(7);
                        userBalance.UpdateDate = null;
                        userBalance.Balance = 1000000;
                        userBalance.ActiveBalance = 1000000;

                        await _context.UserBalances.AddAsync(userBalance);
                        if (await _context.SaveChangesAsync() < 0) {
                            return false;
                        }
                    }
                }
            }
            return true;
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
                expires: DateTime.UtcNow.AddHours(7).AddDays(timeExpire),
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

            var appUser = await _context.AppUsers.FirstOrDefaultAsync(x => x.IdentityId == identityId);

            if (appUser is not null) {
                appUser.Status = UserStatusConstants.Offline;
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