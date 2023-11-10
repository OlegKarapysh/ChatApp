﻿using Microsoft.AspNetCore.Identity;
using Chat.Application.RequestExceptions;
using Chat.Application.Services.JWT;
using Chat.Domain.DTOs.Authentication;
using Chat.Domain.Entities;

namespace Chat.Application.Services.Authentication;

public sealed class AuthService : IAuthService
{
    private readonly IJwtService _jwtService;
    private readonly UserManager<User> _userManager;
    private readonly SignInManager<User> _signInManager;
    
    public AuthService(IJwtService jwtService, UserManager<User> userManager, SignInManager<User> signInManager)
    {
        _jwtService = jwtService;
        _userManager = userManager;
        _signInManager = signInManager;
    }
    
    public async Task<TokenPairDto> LoginAsync(LoginDto loginData)
    {
        var user = await _userManager.FindByEmailAsync(loginData.Email) ?? throw new InvalidEmailException();
        var signInResult = await _signInManager.CheckPasswordSignInAsync(user, loginData.Password, false);
        if (!signInResult.Succeeded)
        {
            throw new InvalidPasswordException();
        }

        var refreshToken = _jwtService.CreateRefreshToken();
        await UpdateUserRefreshToken(user, refreshToken);

        return CreateTokenPair(user);
    }

    public async Task<TokenPairDto> RegisterAsync(RegistrationDto registerData)
    {
        if (await _userManager.FindByEmailAsync(registerData.Email) is not null)
        {
            throw new EmailAlreadyRegisteredException();
        }

        if (await _userManager.FindByNameAsync(registerData.UserName) is not null)
        {
            throw new UsernameAlreadyRegistered();
        }

        var refreshToken = _jwtService.CreateRefreshToken();
        var user = new User
        {
            UserName = registerData.UserName,
            Email = registerData.Email,
            RefreshToken = refreshToken.Token,
            TokenExpiresAt = refreshToken.ExpirationTime
        };
        var identityResult = await _userManager.CreateAsync(user, registerData.Password);
        if (!identityResult.Succeeded)
        {
            throw new BadRegistrationException();
        }

        return CreateTokenPair(await _userManager.FindByEmailAsync(user.Email), refreshToken.Token);
    }

    public async Task ChangePasswordAsync(ChangePasswordDto changePasswordData, int id)
    {
        var user = await _userManager.FindByIdAsync(id.ToString())
                   ?? throw new EntityNotFoundException(nameof(User), nameof(id));
        var changePasswordResult = await _userManager.ChangePasswordAsync(
            user, changePasswordData.CurrentPassword, changePasswordData.NewPassword);

        if (!changePasswordResult.Succeeded)
        {
            throw new ChangePasswordFailedException();
        }
    }

    public async Task<TokenPairDto> RefreshTokenPair(TokenPairDto tokens)
    {
        var userId = _jwtService.GetIdClaim(tokens.AccessToken);
        var user = _userManager.Users.FirstOrDefault(x => x.Id == userId);
        if (user is null)
        {
            throw new EntityNotFoundException("User", "Id");
        }

        if (user.RefreshToken is null)
        {
            throw new EntityNotFoundException("Refresh token");
        }
        
        CheckIfRefreshTokenExpired(user.TokenExpiresAt);
        if (tokens.RefreshToken != user.RefreshToken)
        {
            throw new InvalidTokenException("Refresh");
        }

        var newRefreshToken = _jwtService.CreateRefreshToken();
        await UpdateUserRefreshToken(user, newRefreshToken);
        var newTokens = CreateTokenPair(user, newRefreshToken.Token);

        return newTokens;
    }

    private void CheckIfRefreshTokenExpired(DateTime expiration)
    {
        if (expiration <= DateTime.UtcNow)
        {
            throw new RefreshTokenExpiredException();
        }
    }

    private TokenPairDto CreateTokenPair(User user, string? refreshToken = default)
    {
        return new TokenPairDto
        {
            AccessToken = _jwtService.CreateAccessToken(user.Id, user.UserName!, user.Email!),
            RefreshToken = refreshToken ?? user.RefreshToken ?? throw new Exception("Refresh token is not set!")
        };
    }

    private async Task UpdateUserRefreshToken(User user, RefreshToken refreshToken)
    {
        user.RefreshToken = refreshToken.Token;
        user.TokenExpiresAt = refreshToken.ExpirationTime;
        await _userManager.UpdateAsync(user);
    }
}