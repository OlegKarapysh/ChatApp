global using System.Text.RegularExpressions;
global using System.Linq.Expressions;
global using System.Net;
global using System.Security.Claims;
global using Xunit;
global using Moq;
global using FluentAssertions;
global using Bogus;
global using Microsoft.Extensions.Options;
global using Microsoft.AspNetCore.Mvc;
global using Microsoft.AspNetCore.Http;
global using Microsoft.AspNetCore.Identity;
global using Chat.WebAPI.Controllers;
global using Chat.Application.JWT;
global using Chat.Application.Services.JWT;
global using Chat.Application.Services.Authentication;
global using Chat.Domain.DTOs.Authentication;
global using Chat.Application.RequestExceptions;
global using Chat.Domain.Entities;
global using Chat.Application.Services.Conversations;
global using Chat.Application.Services.Messages;
global using Chat.Application.Services.Users;
global using Chat.Domain.Entities.Conversations;
global using Chat.DomainServices.Repositories;
global using Chat.DomainServices.UnitsOfWork;
global using Chat.Domain.DTOs.Conversations;
global using Chat.Domain.DTOs.Users;
global using Chat.Domain.Web;
global using Chat.UnitTests.TestHelpers;
global using Chat.Application.Extensions;



