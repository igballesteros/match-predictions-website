using System.Net.Http.Json;
using System.Security.Claims;
using Microsoft.AspNetCore.Components.Authorization;
using Blazored.LocalStorage;

namespace PredictionsClient.Services
{

    public class AuthService
    {
        private readonly HttpClient _httpClient;
        private readonly AuthenticationStateProvider _authStateProvider;

        // constructor with dep injections
        public AuthService(HttpClient httpClient, AuthenticationStateProvider authStateProvider)
        {
            _httpClient = httpClient; // calls to backend
            _authStateProvider = authStateProvider; // notifies app auth change
        }

        public async Task<LoginResult> Login(LoginModel loginModel)
        {
            Console.WriteLine($"Attempting login with: {loginModel.Username}/{loginModel.Password}");
            try
            {
                // api call to validate credentials
                var response = await _httpClient.PostAsJsonAsync("api/auth/login", loginModel);
                Console.WriteLine($"Status: {response.StatusCode}");

                var responseContent = await response.Content.ReadAsStringAsync();
                Console.WriteLine($"Response: {responseContent}");
                // process response - handle failed login

                if (!response.IsSuccessStatusCode)
                {
                    return new LoginResult { Success = false, Error = await response.Content.ReadAsStringAsync() };
                }

                // On success, extract token
                var loginResult = await response.Content.ReadFromJsonAsync<LoginResult>();

                // notify state about successful provider
                if (loginResult != null && !string.IsNullOrEmpty(loginResult.Token))
                {
                    loginResult.Success = true;
                    await ((CustomAuthStateProvider)_authStateProvider).NotifyUserAuthentication(loginResult.Token);
                }
                else
                {
                    loginResult.Success = false;
                    loginResult.Error = "Invalid response from server";
                }

                return loginResult;
            }
            catch (Exception ex)
            {
                Console.WriteLine($"Exception: {ex}");
                return new LoginResult
                {
                    Success = false,
                    Error = ex.Message
                };
            }
        }

        // logout method
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
        public string? Token { get; set; } // JWT token from api
        public string? Error { get; set; } // error message
        public string? Role { get; set; } // user role from api
        public string? Username { get; set; } // username from api
    }
}
/*
  handles authentication operations (login/logout) and communicates with your backend api
  - performs credential validation - calls api login endpoint
  - manages the login/logout process - initiates these action
  - handles api communication - sends credentials recieves tokens
  - transforms apo responses - application friendly formats
*/