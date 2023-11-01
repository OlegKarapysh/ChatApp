using Microsoft.AspNetCore.Components.Authorization;
using Microsoft.AspNetCore.Components.Web;
using Microsoft.AspNetCore.Components.WebAssembly.Hosting;
using Blazored.LocalStorage;
using Chat.WebUI;
using Chat.WebUI.Extensions;
using Chat.WebUI.Providers;
using Chat.WebUI.Services.Notifications;

var builder = WebAssemblyHostBuilder.CreateDefault(args);

builder.Services.AddSingleton<INotificationService, NotificationService>();

builder.RootComponents.Add<App>("#app");
builder.RootComponents.Add<HeadOutlet>("head::after");

builder.Services.AddBlazoredLocalStorage();
builder.Services.AddAuthorizationCore();

builder.Services.AddScoped<AuthenticationStateProvider, JwtAuthenticationStateProvider>();
builder.Services.AddScoped<INotifyAuthenticationChanged, JwtAuthenticationStateProvider>();
builder.Services.AddCustomHttpClient(builder.Configuration);
builder.Services.AddCustomServices();

await builder.Build().RunAsync();
