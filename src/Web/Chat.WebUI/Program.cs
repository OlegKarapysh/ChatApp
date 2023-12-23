var builder = WebAssemblyHostBuilder.CreateDefault(args);

builder.RootComponents.Add<App>("#app");
builder.RootComponents.Add<HeadOutlet>("head::after");

builder.Services.AddBlazoredLocalStorage();
builder.Services.AddBlazoredToast();
builder.Services.AddScoped<DialogService>();
builder.Services.AddScoped<SpinnerService>();
builder.Services.AddAuthorizationCore();
builder.Services.AddCustomServices(builder.Configuration);

await builder.Build().RunAsync();
