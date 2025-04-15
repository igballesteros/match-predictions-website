using System.Net.Http.Json;
using System.Security.Claims;
using Microsoft.AspNetCore.Components.Authorization;

public class AuthService
{
    private readonly HttpClient _httpClient;
    private readonly AuthenticationStateProvider _authStateProvider;

    public AuthService(HttpClient httpClient, AuthenticationStateProvider authStateProvider)
    {
        _httpClient = httpClient;
        _authStateProvider = authStateProvider;
    }

    public async Task<LoginResult> Login(LoginModel loginModel)
    {
        var response = await _httpClient.PostAsJsonAsync("api/auth/login", loginModel);

        if (!response.IsSuccessStatusCode)
        {
            return new LoginResult { Succes = false, Error = "Invalid Credentials" };
        }

        var loginResult = await response.Content.ReadFromJsonAsync<LoginResult>();

        if (loginResult?.Success == true)
        {
            await ((CustomAuthStateProvider)_authStateProvider).NotifyUserAuthentication(loginResult.Token!);
        }

        return loginResult!;
    }

    public async Task Logout()
    {
        await ((CustomAuthStateProvider)_authStateProvider).NotifyUserLogout();
    }
}

public class LoginModel
{
    public string Username { get; set; } = string.Empty;
    public string Password { get; set; } = string.Empty;
}

public class LoginResult
{
    public bool Success { get; set; }
    public string? Token { get; set; }
    public string? Error { get; set; }
    public string? Role { get; set; }
    public string? Username { get; set; }
}