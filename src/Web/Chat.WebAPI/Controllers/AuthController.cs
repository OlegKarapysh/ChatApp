using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Authorization;
using Chat.Application.Services.Authentication;
using Chat.Application.Services.JWT;
using Chat.Domain.DTOs.Authentication;

namespace Chat.WebAPI.Controllers;

[ApiController, Route("api/[controller]")]
public sealed class AuthController : ControllerBase
{
    private readonly IAuthService _authService;
    private readonly IJwtService _jwtService;

    public AuthController(IAuthService authService, IJwtService jwtService)
    {
        _authService = authService;
        _jwtService = jwtService;
    }

    [AllowAnonymous, HttpPost("register")]
    public async Task<ActionResult<TokenPairDto>> RegisterAsync([FromBody] RegistrationDto registerData)
    {
        return Ok(await _authService.RegisterAsync(registerData));
    }

    [AllowAnonymous, HttpPost("login")]
    public async Task<ActionResult<TokenPairDto>> LoginAsync([FromBody] LoginDto loginData)
    {
        return Ok(await _authService.LoginAsync(loginData));
    }

    [AllowAnonymous, HttpPost("refresh")]
    public async Task<ActionResult<TokenPairDto>> RefreshTokenPairAsync([FromBody] TokenPairDto tokens)
    {
        return Ok(await _authService.RefreshTokenPairAsync(tokens));
    }

    [Authorize, HttpPost("change-password")]
    public async Task<IActionResult> ChangePasswordAsync([FromBody] ChangePasswordDto changePasswordData)
    {
        await _authService.ChangePasswordAsync(changePasswordData, _jwtService.GetIdClaim(User));
        return Ok();
    }
}