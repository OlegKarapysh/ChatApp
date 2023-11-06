using Microsoft.AspNetCore.Components.Authorization;
using Microsoft.AspNetCore.Components.Web;
using Microsoft.AspNetCore.Components.WebAssembly.Hosting;
using Blazored.LocalStorage;
using Blazored.Toast;
using BlazorSpinner;
using Chat.WebUI;
using Chat.WebUI.Extensions;
using Chat.WebUI.Providers;

var builder = WebAssemblyHostBuilder.CreateDefault(args);

builder.RootComponents.Add<App>("#app");
builder.RootComponents.Add<HeadOutlet>("head::after");

builder.Services.AddBlazoredLocalStorage();
builder.Services.AddBlazoredToast();
builder.Services.AddScoped<SpinnerService>();
builder.Services.AddAuthorizationCore();
builder.Services.AddScoped<AuthenticationStateProvider, JwtAuthenticationStateProvider>();
builder.Services.AddScoped<INotifyAuthenticationChanged, JwtAuthenticationStateProvider>();
builder.Services.AddCustomHttpClient(builder.Configuration);
builder.Services.AddCustomServices();

await builder.Build().RunAsync();
