using Microsoft.AspNetCore.Components.Web;
using Microsoft.AspNetCore.Components.WebAssembly.Hosting;
using PredictionsClient;
using Microsoft.AspNetCore.Components.Authorization;
using Blazored.LocalStorage;
using PredictionsClient.Services;
using Microsoft.Extensions.Http;
using Microsoft.Extensions.DependencyInjection;

var builder = WebAssemblyHostBuilder.CreateDefault(args);
builder.RootComponents.Add<App>("#app");
builder.RootComponents.Add<HeadOutlet>("head::after");

// Add HttpClient
builder.Services.AddHttpClient("AuthAPI", client =>
{
  client.BaseAddress = new Uri("http://localhost:5082");
});

// Add auth services

builder.Services.AddScoped<AdminService>();

builder.Services.AddBlazoredLocalStorage();

builder.Services.AddScoped<AuthenticationStateProvider, CustomAuthStateProvider>();

// register auth service
builder.Services.AddScoped<AuthService>();

// add authorization capabilites 
builder.Services.AddAuthorizationCore(config =>
{
  config.AddPolicy("Admin", policy => policy.RequireRole("Admin"));
  config.AddPolicy("User", policy => policy.RequireRole("User"));
});

// primary httpclient
builder.Services.AddScoped(sp =>
{
  var factory = sp.GetRequiredService<IHttpClientFactory>();
  return factory.CreateClient("AuthAPI");
});

builder.Logging.SetMinimumLevel(LogLevel.Debug);

await builder.Build().RunAsync();

/*
  libraries:
  - Microsoft.AspNetCore.Components.Authorization:
    provides base classes for authentication in blazor

  - Blazored.LocalStorag:
    stores jwt token in browser's local storage
    persists local state across page refreshes

  - System.IdentityModel.Tokens.Jwt:
    parses jwt token and reading claims
*/