namespace PersonalPortfolio.Services
{
    public interface IEmailService
    {
        Task<bool> SendEmail(string name, string email, string subject, string message);
    }
}