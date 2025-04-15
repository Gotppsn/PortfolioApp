using PortfolioApp.Shared.Models;

namespace PortfolioApp.Client.Services
{
    public interface IAuthService
    {
        Task<LoginResponse> Login(LoginModel loginModel);
        Task Logout();
        Task<Response> Register(RegisterModel registerModel);
        Task<Response> ForgotPassword(ForgotPasswordModel forgotPasswordModel);
        Task<Response> ResetPassword(ResetPasswordModel resetPasswordModel);
        Task<UserInfoResponse?> GetUserInfo();
        Task<bool> IsUserAuthenticated();
        Task<string> TryRefreshToken();
    }
}