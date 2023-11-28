global using System.Net.Http.Json;
global using System.Net.Http.Headers;
global using Xunit;
global using FluentAssertions;
global using Microsoft.AspNetCore.Mvc.Testing;
global using Microsoft.AspNetCore.Hosting;
global using Microsoft.AspNetCore.Authentication.JwtBearer;
global using Microsoft.EntityFrameworkCore;
global using Microsoft.Extensions.DependencyInjection;
global using Microsoft.Extensions.DependencyInjection.Extensions;
global using Chat.Persistence.Contexts;
global using Chat.Domain.DTOs.Users;
global using Chat.Application.RequestExceptions;
global using Chat.Domain.DTOs.Authentication;
global using Chat.Domain.Entities;
global using Chat.Domain.Enums;
global using Chat.Domain.Web;

