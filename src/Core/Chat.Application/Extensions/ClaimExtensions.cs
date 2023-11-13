﻿using System.Security.Claims;
using Chat.Application.RequestExceptions;
using Chat.Application.Services.JWT;

namespace Chat.Application.Extensions;

public static class ClaimExtensions
{
    public static int GetIdClaim(this ClaimsPrincipal claimsPrincipal)
    {
        var isParsed = int.TryParse(claimsPrincipal.FindFirstValue(IJwtService.IdClaimName), out var id);
        if (!isParsed)
        {
            throw new MissingClaimException("id");
        }

        return id;
    }
}