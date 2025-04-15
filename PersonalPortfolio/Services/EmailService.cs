using System.Net.Http.Json;

namespace PersonalPortfolio.Services
{
    public class EmailService : IEmailService
    {
        private readonly HttpClient _httpClient;

        public EmailService(HttpClient httpClient)
        {
            _httpClient = httpClient;
        }

        public async Task<bool> SendEmail(string name, string email, string subject, string message)
        {
            try
            {
                // For production, you'd use a real email API service like SendGrid, Mailchimp, etc.
                // This is just a placeholder for the demo
                var emailRequest = new EmailRequest
                {
                    Name = name,
                    Email = email,
                    Subject = subject,
                    Message = message
                };

                // Simulating an API call
                await Task.Delay(1000);
                
                // In a real application, you would send this to your API
                // var response = await _httpClient.PostAsJsonAsync("/api/email", emailRequest);
                // return response.IsSuccessStatusCode;
                
                return true;
            }
            catch
            {
                return false;
            }
        }
    }

    public class EmailRequest
    {
        public string Name { get; set; } = string.Empty;
        public string Email { get; set; } = string.Empty;
        public string Subject { get; set; } = string.Empty;
        public string Message { get; set; } = string.Empty;
    }
}