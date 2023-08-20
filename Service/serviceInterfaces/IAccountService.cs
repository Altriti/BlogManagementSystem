using Domain.DTOs;
using System.Security.Claims;

namespace Service.Contract
{
    public interface IAccountService
    {
        Task<UserDto> Login(LoginDto loginDto);
        Task<UserDto> Register(RegisterDto registerDto);
        Task<UserDto> RegisterAdministrator(RegisterDto registerDto);
        Task<UserDto> GetCurrentUser(ClaimsPrincipal user);
        string RemoveUser(string id);
    }
}
