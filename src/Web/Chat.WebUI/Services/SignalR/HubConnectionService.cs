﻿using Microsoft.AspNetCore.SignalR.Client;
using Chat.WebUI.Services.Auth;

namespace Chat.WebUI.Services.SignalR;

public class HubConnectionService
{
    private readonly ITokenService _tokenService;
    private readonly string _hubUrl;
    private HubConnection? _connection;
    
    public HubConnectionService(ITokenService tokenService, IConfiguration configuration)
    {
        _tokenService = tokenService;
        _hubUrl = configuration["SignalR:HubUrl"]!;
    }

    public async Task ConnectAsync()
    {
        _connection = new HubConnectionBuilder()
                      .WithUrl(_hubUrl,
                          options =>
                          {
                              options.AccessTokenProvider = async () => (await _tokenService.GetTokens()).AccessToken;
                          })
                      .WithAutomaticReconnect()
                      .Build();
    }
}