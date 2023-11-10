using Microsoft.AspNetCore.Components.Web;
using Microsoft.AspNetCore.Components.WebAssembly.Hosting;
using Blazored.LocalStorage;
using Blazored.Toast;
using Chat.WebUI;
using Chat.WebUI.Extensions;

var builder = WebAssemblyHostBuilder.CreateDefault(args);

builder.RootComponents.Add<App>("#app");
builder.RootComponents.Add<HeadOutlet>("head::after");

builder.Services.AddBlazoredLocalStorage();
builder.Services.AddBlazoredToast();
builder.Services.AddAuthorizationCore();
builder.Services.AddCustomServices(builder.Configuration);

await builder.Build().RunAsync();
