using System.Security.Claims;
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

    public AuthController(IAuthService authService)
    {
        _authService = authService;
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

    [AllowAnonymous, HttpPost("change-password")]
    public async Task<IActionResult> ChangePasswordAsync([FromBody] ChangePasswordDto changePasswordData)
    {
        var user = User.Claims.ToList();
        var b = HttpContext.Request;
        
        var a = HttpContext.Request.Headers.Authorization;
        var isParsed = int.TryParse(User.FindFirstValue(IJwtService.IdClaimName), out var id);
        if (!isParsed)
        {
            throw new Exception("Cannot parse 'id' claim from current user!");
        }
        
        await _authService.ChangePasswordAsync(changePasswordData, id);
        return Ok();
    }
}