using System.Net.Http.Headers;
using System.Net.Http.Json;
using System.Security.Claims;
using System.Text.Json;
using Blazored.LocalStorage;
using Microsoft.AspNetCore.Components.Authorization;
using PortfolioApp.Shared.Models;

namespace PortfolioApp.Client.Services
{
    public class AuthService : IAuthService
    {
        private readonly HttpClient _httpClient;
        private readonly AuthenticationStateProvider _authStateProvider;
        private readonly ILocalStorageService _localStorage;

        public AuthService(HttpClient httpClient, 
                           AuthenticationStateProvider authStateProvider,
                           ILocalStorageService localStorage)
        {
            _httpClient = httpClient;
            _authStateProvider = authStateProvider;
            _localStorage = localStorage;
        }

        public async Task<LoginResponse> Login(LoginModel loginModel)
        {
            var response = await _httpClient.PostAsJsonAsync("api/auth/login", loginModel);
            var loginResult = await response.Content.ReadFromJsonAsync<LoginResponse>();

            if (response.IsSuccessStatusCode && loginResult != null)
            {
                await _localStorage.SetItemAsync("authToken", loginResult.Token);
                await _localStorage.SetItemAsync("refreshToken", loginResult.RefreshToken);
                await _localStorage.SetItemAsync("tokenExpiration", loginResult.Expiration);
                
                ((ApiAuthenticationStateProvider)_authStateProvider).MarkUserAsAuthenticated(loginResult.Token);
                _httpClient.DefaultRequestHeaders.Authorization = new AuthenticationHeaderValue("bearer", loginResult.Token);
            }

            return loginResult ?? new LoginResponse { Status = "Error", Message = "Login failed" };
        }

        public async Task Logout()
        {
            try
            {
                await _httpClient.PostAsync("api/auth/logout", null);
            }
            catch { }
            finally
            {
                await _localStorage.RemoveItemAsync("authToken");
                await _localStorage.RemoveItemAsync("refreshToken");
                await _localStorage.RemoveItemAsync("tokenExpiration");
                
                ((ApiAuthenticationStateProvider)_authStateProvider).MarkUserAsLoggedOut();
                _httpClient.DefaultRequestHeaders.Authorization = null;
            }
        }

        public async Task<Response> Register(RegisterModel registerModel)
        {
            var response = await _httpClient.PostAsJsonAsync("api/auth/register", registerModel);
            var result = await response.Content.ReadFromJsonAsync<Response>();
            return result ?? new Response { Status = "Error", Message = "Registration failed" };
        }

        public async Task<Response> ForgotPassword(ForgotPasswordModel forgotPasswordModel)
        {
            var response = await _httpClient.PostAsJsonAsync("api/auth/forgot-password", forgotPasswordModel);
            var result = await response.Content.ReadFromJsonAsync<Response>();
            return result ?? new Response { Status = "Error", Message = "Request failed" };
        }

        public async Task<Response> ResetPassword(ResetPasswordModel resetPasswordModel)
        {
            var response = await _httpClient.PostAsJsonAsync("api/auth/reset-password", resetPasswordModel);
            var result = await response.Content.ReadFromJsonAsync<Response>();
            return result ?? new Response { Status = "Error", Message = "Password reset failed" };
        }

        public async Task<UserInfoResponse?> GetUserInfo()
        {
            var response = await _httpClient.GetAsync("api/auth/user-info");
            if (response.IsSuccessStatusCode)
            {
                return await response.Content.ReadFromJsonAsync<UserInfoResponse>();
            }
            return null;
        }

        public async Task<bool> IsUserAuthenticated()
        {
            var authState = await _authStateProvider.GetAuthenticationStateAsync();
            return authState.User.Identity?.IsAuthenticated ?? false;
        }

        public async Task<string> TryRefreshToken()
        {
            var savedToken = await _localStorage.GetItemAsync<string>("authToken");
            var refreshToken = await _localStorage.GetItemAsync<string>("refreshToken");

            if (string.IsNullOrEmpty(savedToken) || string.IsNullOrEmpty(refreshToken))
            {
                return string.Empty;
            }

            var tokenModel = new TokenModel
            {
                AccessToken = savedToken,
                RefreshToken = refreshToken
            };

            var response = await _httpClient.PostAsJsonAsync("api/auth/refresh-token", tokenModel);
            if (!response.IsSuccessStatusCode)
            {
                return string.Empty;
            }

            var refreshResult = await response.Content.ReadFromJsonAsync<RefreshTokenResponse>();
            if (refreshResult == null)
            {
                return string.Empty;
            }

            await _localStorage.SetItemAsync("authToken", refreshResult.AccessToken);
            await _localStorage.SetItemAsync("refreshToken", refreshResult.RefreshToken);
            
            _httpClient.DefaultRequestHeaders.Authorization = new AuthenticationHeaderValue("bearer", refreshResult.AccessToken);

            return refreshResult.AccessToken;
        }
    }

    public class RefreshTokenResponse
    {
        public string AccessToken { get; set; } = string.Empty;
        public string RefreshToken { get; set; } = string.Empty;
    }
}