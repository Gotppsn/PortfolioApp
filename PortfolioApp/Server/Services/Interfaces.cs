using PortfolioApp.Server.Models;
using System.Security.Claims;

namespace PortfolioApp.Server.Services
{
    public interface IFileStorageService
    {
        Task<string> SaveFileAsync(byte[] fileData, string fileName, string contentType);
        Task DeleteFileAsync(string fileUrl);
    }

    public interface IEmailService
    {
        Task SendEmailAsync(string email, string subject, string message);
    }

    public interface ITokenService
    {
        string GenerateJwtToken(ApplicationUser user, IList<string> roles);
        ClaimsPrincipal GetPrincipalFromExpiredToken(string token);
        string GenerateRefreshToken();
    }
}