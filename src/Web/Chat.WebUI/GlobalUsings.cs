﻿global using System.Security.Claims;
global using System.Text.Json;
global using System.Net.Http.Json;
global using System.Net;
global using System.Net.Http.Headers;
global using System.Globalization;
global using System.Reflection;
global using Microsoft.Extensions.Primitives;
global using Microsoft.AspNetCore.Components.Authorization;
global using Microsoft.AspNetCore.Components.Web;
global using Microsoft.AspNetCore.Components.WebAssembly.Hosting;
global using Microsoft.AspNetCore.SignalR.Client;
global using Microsoft.JSInterop;
global using Microsoft.AspNetCore.WebUtilities;
global using Microsoft.AspNetCore.Components.Forms;
global using Microsoft.AspNetCore.Components;
global using OpenAI.Threads;
global using Blazored.LocalStorage;
global using Blazored.Toast;
global using BlazorSpinner;
global using Blazored.Toast.Services;
global using Radzen;
global using Chat.WebUI;
global using Chat.WebUI.Extensions;
global using Chat.Domain.DTOs;
global using Chat.Domain.DTOs.Users;
global using Chat.Domain.Errors;
global using Chat.Domain.Web;
global using Chat.WebUI.HttpHandlers;
global using Chat.WebUI.Services.Auth;
global using Chat.Application.SignalR;
global using Chat.Domain.DTOs.Calls;
global using Chat.Domain.DTOs.Messages;
global using Chat.Domain.DTOs.AssistantFiles;
global using Chat.Domain.DTOs.Groups;
global using Chat.Domain.DTOs.Conversations;
global using Chat.Domain.DTOs.Authentication;
global using Chat.Domain.Enums;
global using Chat.WebUI.Providers;
global using Chat.WebUI.Pages;
global using Chat.Domain.Attributes;
global using Chat.WebUI.Services.Conversations;
global using Chat.WebUI.Services.Groups;
global using Chat.WebUI.Services.Messages;
global using Chat.WebUI.Services.OpenAI;
global using Chat.WebUI.Services.SignalR;
global using Chat.WebUI.Services.Users;
global using Chat.WebUI.Services.WebRtc;
global using Chat.Domain.DTOs.AmazonSearch;
global using Chat.WebUI.Services.Amazon;
global using Chat.WebUI.Services.AiCopilot;
