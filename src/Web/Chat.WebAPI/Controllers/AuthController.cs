using Microsoft.AspNetCore.Mvc;
using Chat.Application.Services.Authentication;
using Chat.Domain.DTOs.Authentication;

namespace Chat.WebAPI.Controllers;

[ApiController]
[Route("api/[controller]")]
public sealed class AuthController : ControllerBase
{
    private readonly IAuthService _authService;

    public AuthController(IAuthService authService)
    {
        _authService = authService;
    }
    
    [HttpPost("register")]
    public async Task<ActionResult<TokenPairDto>> RegisterAsync([FromBody] RegistrationDto registerData)
    {
        return Ok(await _authService.RegisterAsync(registerData));
    }

    [HttpPost("login")]
    public async Task<ActionResult<TokenPairDto>> LoginAsync([FromBody] LoginDto loginData)
    {
        return Ok(await _authService.LoginAsync(loginData));
    }

    [HttpPost("change-password")]
    public async Task<IActionResult> ChangePasswordAsync([FromBody] ChangePasswordDto changePasswordData)
    {
        await _authService.ChangePasswordAsync(changePasswordData);
        return Ok();
    }
}