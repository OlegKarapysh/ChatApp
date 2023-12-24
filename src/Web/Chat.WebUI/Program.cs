var builder = WebAssemblyHostBuilder.CreateDefault(args);

builder.RootComponents.Add<App>("#app");
builder.RootComponents.Add<HeadOutlet>("head::after");

builder.Services.AddBlazoredLocalStorage();
builder.Services.AddBlazoredToast();
builder.Services.AddScoped<DialogService>();
builder.Services.AddScoped<SpinnerService>();
builder.Services.AddAuthorizationCore();
builder.Services.AddCoreServices(builder.Configuration);
builder.Services.AddWebApiServices();
builder.Services.AddSignallingServices();

await builder.Build().RunAsync();
