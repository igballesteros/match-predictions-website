using System.Security.Claims;
using Microsoft.AspNetCore.Components.Authorization;
using Blazored.LocalStorage;
using System.IdentityModel.Tokens.Jwt;

namespace PredictionsClient.Services
{
    public class CustomAuthStateProvider : AuthenticationStateProvider
    {
        private readonly ILocalStorageService _localStorage; // blazor locar storage - stores jwt in browser storage

        public CustomAuthStateProvider(ILocalStorageService localStorage)
        {
            _localStorage = localStorage;
        }

        public override async Task<AuthenticationState> GetAuthenticationStateAsync()
        {
            // checks for stored token from local storage set
            var token = await _localStorage.GetItemAsync<string>("authToken");

            // if null, return unauthenticated state
            if (string.IsNullOrEmpty(token))
            {
                return new AuthenticationState(new ClaimsPrincipal(new ClaimsIdentity()));
            }

            // parse claims from jwt
            var claims = ParseClaimsFromJwt(token);

            // create auth user
            var identity = new ClaimsIdentity(claims, "jwt");
            var user = new ClaimsPrincipal(identity);

            return new AuthenticationState(user);
        }

        public async Task NotifyUserAuthentication(string token)
        {
            // store token
            await _localStorage.SetItemAsync("authToken", token);

            // parse claim
            var claims = ParseClaimsFromJwt(token);
            var identity = new ClaimsIdentity(claims, "jwt");
            var user = new ClaimsPrincipal(identity);

            // notify all components
            NotifyAuthenticationStateChanged(
                Task.FromResult(new AuthenticationState(user)));
        }

        public async Task NotifyUserLogout()
        {
            // remove token
            await _localStorage.RemoveItemAsync("authToken");

            // create empty identity
            var identity = new ClaimsIdentity();
            var user = new ClaimsPrincipal(identity);

            // notify all components
            NotifyAuthenticationStateChanged(
                Task.FromResult(new AuthenticationState(user)));
        }

        private static IEnumerable<Claim> ParseClaimsFromJwt(string jwt)
        {
            var handler = new JwtSecurityTokenHandler();
            var token = handler.ReadJwtToken(jwt);
            return token.Claims;
        }
        public async Task<bool> IsInRoleAsync(string role)
        {
            var token = await _localStorage.GetItemAsync<string>("authToken");
            if (string.IsNullOrEmpty(token)) return false;

            var claims = ParseClaimsFromJwt(token);
            return claims.Any(c =>
                (c.Type == ClaimTypes.Role || c.Type == "role") &&
                c.Value == role);
        }
    }
}

/*
  Persists auth state - via local storage
  Parses JWT tokens to extract user claims
  Notifies components when auth state changes
  Provides current auth status to the entire app
*/