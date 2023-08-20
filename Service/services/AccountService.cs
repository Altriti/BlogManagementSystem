using Domain.DTOs;
using Domain.Model;
using Microsoft.AspNetCore.Identity;
using Microsoft.EntityFrameworkCore;
using Repository.Repositories;
using Service.Contract;
using System.Security.Claims;

namespace Service.Implementation
{
    public class AccountService : IAccountService
    {
        private readonly UserManager<AppUser> userManager;
        private readonly IAccountRepository accountRepository;
        private readonly UserAccessor userAccessor;
        private readonly RoleManager<IdentityRole> roleManager;
        private readonly TokenService tokenService;

        public AccountService(UserManager<AppUser> userManager,
            TokenService tokenService, IAccountRepository accountRepository,
            UserAccessor userAccessor, RoleManager<IdentityRole> roleManager)
        {
            this.tokenService = tokenService;
            this.userManager = userManager;
            this.accountRepository = accountRepository;
            this.userAccessor = userAccessor;
            this.roleManager = roleManager;
        }

        public TokenService TokenService { get; }

        public async Task<UserDto> Login(LoginDto loginDto)
        {
            var user = await this.accountRepository.FindUser(loginDto);

            if (user == null)
            {
                throw new System.Exception($"Unauthorized");
            }

            var result = await this.userManager.CheckPasswordAsync(user, loginDto.Password);

            if (result)
            {
                return CreateUserObject(user);
            }

            throw new System.Exception($"Unauthorized");
        }

        public async Task<UserDto> Register(RegisterDto registerDto)
        {
            if (await this.userManager.Users.AnyAsync(u => u.UserName == registerDto.Username))
            {
                throw new System.Exception($"Username is taken");
            };

            if (await this.userManager.Users.AnyAsync(u => u.Email == registerDto.Email))
            {
                throw new System.Exception($"Email is taken");
            };

            var user = new AppUser
            {
                DisplayName = registerDto.DisplayName,
                Email = registerDto.Email,
                UserName = registerDto.Username
            };

            var result = await this.userManager.CreateAsync(user, registerDto.Password);

            await this.userManager.AddToRoleAsync(user, "Creator");

            if (result.Succeeded)
            {
                await accountRepository.CreateUser(user);

                return CreateUserObject(user);
            }

            throw new System.Exception($"Problem creating user");
        }

        public async Task<UserDto> RegisterAdministrator(RegisterDto registerDto)
        {
            if (await this.userManager.Users.AnyAsync(u => u.UserName == registerDto.Username))
            {
                throw new System.Exception($"Username is taken");
            };

            if (await this.userManager.Users.AnyAsync(u => u.Email == registerDto.Email))
            {
                throw new System.Exception($"Email is taken");
            };

            var user = new AppUser
            {
                DisplayName = registerDto.DisplayName,
                Email = registerDto.Email,
                UserName = registerDto.Username
            };

            var result = await this.userManager.CreateAsync(user, registerDto.Password);

            await this.userManager.AddToRoleAsync(user, "Administrator");

            if (result.Succeeded)
            {
                await accountRepository.CreateUser(user);

                return CreateUserObject(user);
            }

            throw new System.Exception($"Problem creating user");
        }

        public string RemoveUser(string id)
        {
            try
            {
                var loggedinUserId = this.userAccessor.GetUserId();

                var loggedinUser = this.accountRepository.FindUserById(loggedinUserId);

                if (loggedinUser != null)
                {
                    var roleUser = this.accountRepository.RoleUser(loggedinUserId);

                    var role = this.roleManager.Roles.FirstOrDefault(r => r.Id == roleUser.RoleId);

                    if (role.Name == "Administrator")
                    {
                        var userToRemove = this.userManager.Users.FirstOrDefault(u => u.Id == id);

                        if (userToRemove != null)
                        {
                            this.accountRepository.RemoveUserFromDatabase(userToRemove);

                            this.accountRepository.SaveChanges();

                            return $"User removed successfully";
                        }

                        return $"Could not find user";
                    }

                    return $"You should be administrator in order to delete user";
                }

                return $"You should be logged in";
            }

            catch (Exception ex)
            {
                return ex.Message;
            }
        }

        public async Task<UserDto> GetCurrentUser(ClaimsPrincipal user)
        {
            var email = user.FindFirstValue(ClaimTypes.Email);

            var appUser = await this.userManager.FindByEmailAsync(email);

            if (appUser != null)
            {
                return CreateUserObject(appUser);
            }

            throw new System.Exception($"Couldn't find user");
        }

        private UserDto CreateUserObject(AppUser user)
        {
            return new UserDto
            {
                DisplayName = user.DisplayName,
                Token = this.tokenService.CreateToken(user),
                Username = user.UserName
            };
        }
    }
}
