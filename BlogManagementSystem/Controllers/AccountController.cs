using Domain.DTOs;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using Service.Contract;

namespace BlogManagementSystem.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    public class AccountController : ControllerBase
    {
        private readonly IAccountService accountService;

        public AccountController(IAccountService accountService)
        {
            this.accountService = accountService;
        }

        [HttpPost("login")]
        public async Task<ActionResult<UserDto>> Login(LoginDto loginDto)
        {
            return Ok(await this.accountService.Login(loginDto));
        }

        [HttpPost("register")]
        public async Task<ActionResult<UserDto>> Register(RegisterDto registerDto)
        {
            return Ok(await this.accountService.Register(registerDto));
        }

        [HttpPost("registerAdministrator")]
        public async Task<ActionResult<UserDto>> RegisterAdministrator(RegisterDto registerDto)
        {
            return Ok(await this.accountService.RegisterAdministrator(registerDto));
        }

        [HttpDelete]
        public IActionResult RemoveUser(string id)
        {
            return Ok(this.accountService.RemoveUser(id));
        }

        [HttpGet]
        public async Task<ActionResult<UserDto>> GetCurrentLoggedInUser()
        {
            return Ok(await this.accountService.GetCurrentUser(User));
        }
    }
}
