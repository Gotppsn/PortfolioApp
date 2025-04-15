using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Identity;
using Microsoft.AspNetCore.Mvc;
using PortfolioApp.Server.Models;
using PortfolioApp.Server.Services;
using PortfolioApp.Shared.Models;
using System.Security.Claims;
using System.Text;

namespace PortfolioApp.Server.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    public class AuthController : ControllerBase
    {
        private readonly UserManager<ApplicationUser> _userManager;
        private readonly SignInManager<ApplicationUser> _signInManager;
        private readonly RoleManager<IdentityRole> _roleManager;
        private readonly ITokenService _tokenService;
        private readonly IEmailService _emailService;
        private readonly IConfiguration _configuration;
        private readonly ILogger<AuthController> _logger;

        public AuthController(
            UserManager<ApplicationUser> userManager,
            SignInManager<ApplicationUser> signInManager,
            RoleManager<IdentityRole> roleManager,
            ITokenService tokenService,
            IEmailService emailService,
            IConfiguration configuration,
            ILogger<AuthController> logger)
        {
            _userManager = userManager;
            _signInManager = signInManager;
            _roleManager = roleManager;
            _tokenService = tokenService;
            _emailService = emailService;
            _configuration = configuration;
            _logger = logger;
        }

        [HttpPost]
        [Route("register")]
        public async Task<IActionResult> Register([FromBody] RegisterModel model)
        {
            if (!ModelState.IsValid)
                return BadRequest(ModelState);

            var userExists = await _userManager.FindByNameAsync(model.UserName);
            if (userExists != null)
                return StatusCode(StatusCodes.Status400BadRequest, new Response { Status = "Error", Message = "User already exists!" });

            var userWithEmail = await _userManager.FindByEmailAsync(model.Email);
            if (userWithEmail != null)
                return StatusCode(StatusCodes.Status400BadRequest, new Response { Status = "Error", Message = "Email already in use!" });

            ApplicationUser user = new()
            {
                Email = model.Email,
                SecurityStamp = Guid.NewGuid().ToString(),
                UserName = model.UserName,
                FirstName = model.FirstName,
                LastName = model.LastName
            };

            var result = await _userManager.CreateAsync(user, model.Password);
            if (!result.Succeeded)
                return StatusCode(StatusCodes.Status500InternalServerError, new Response { Status = "Error", Message = "User creation failed! " + string.Join(", ", result.Errors.Select(e => e.Description)) });

            // Add to User role
            if (!await _roleManager.RoleExistsAsync("User"))
                await _roleManager.CreateAsync(new IdentityRole("User"));

            await _userManager.AddToRoleAsync(user, "User");

            // Send confirmation email
            var token = await _userManager.GenerateEmailConfirmationTokenAsync(user);
            var confirmationLink = Url.Action("ConfirmEmail", "Auth", new { userId = user.Id, token = token }, Request.Scheme);
            var message = new StringBuilder();
            message.AppendLine("<html><body>");
            message.AppendLine($"<h1>Welcome to Portfolio App</h1>");
            message.AppendLine($"<p>Hi {user.FirstName},</p>");
            message.AppendLine($"<p>Thank you for registering. Please confirm your email by clicking the link below:</p>");
            message.AppendLine($"<p><a href=\"{confirmationLink}\">Confirm Email</a></p>");
            message.AppendLine("<p>Thank you!</p>");
            message.AppendLine("</body></html>");

            await _emailService.SendEmailAsync(user.Email, "Confirm your email", message.ToString());

            return Ok(new Response { Status = "Success", Message = "User created successfully! Please check your email to confirm your account." });
        }

        [HttpPost]
        [Route("login")]
        public async Task<IActionResult> Login([FromBody] LoginModel model)
        {
            if (!ModelState.IsValid)
                return BadRequest(ModelState);

            var user = await _userManager.FindByNameAsync(model.UserName) ?? await _userManager.FindByEmailAsync(model.UserName);
            if (user == null)
                return Unauthorized(new Response { Status = "Error", Message = "Invalid credentials" });

            var result = await _signInManager.PasswordSignInAsync(user.UserName, model.Password, model.RememberMe, false);
            if (!result.Succeeded)
                return Unauthorized(new Response { Status = "Error", Message = "Invalid credentials" });

            if (!await _userManager.IsEmailConfirmedAsync(user) && bool.Parse(_configuration["RequireEmailConfirmation"] ?? "true"))
                return Unauthorized(new Response { Status = "Error", Message = "Email not confirmed" });

            var userRoles = await _userManager.GetRolesAsync(user);
            var token = _tokenService.GenerateJwtToken(user, userRoles);
            var refreshToken = _tokenService.GenerateRefreshToken();

            user.RefreshToken = refreshToken;
            user.RefreshTokenExpiryTime = DateTime.Now.AddDays(7);
            await _userManager.UpdateAsync(user);

            return Ok(new LoginResponse
            {
                Token = token,
                RefreshToken = refreshToken,
                Expiration = DateTime.Now.AddHours(24),
                Status = "Success",
                Message = "Login successful"
            });
        }

        [HttpPost]
        [Route("refresh-token")]
        public async Task<IActionResult> RefreshToken([FromBody] TokenModel tokenModel)
        {
            if (tokenModel is null || string.IsNullOrEmpty(tokenModel.AccessToken) || string.IsNullOrEmpty(tokenModel.RefreshToken))
                return BadRequest("Invalid client request");

            var principal = _tokenService.GetPrincipalFromExpiredToken(tokenModel.AccessToken);
            if (principal == null)
                return BadRequest("Invalid access token or refresh token");

            string username = principal.Identity.Name;
            var user = await _userManager.FindByNameAsync(username);

            if (user == null || user.RefreshToken != tokenModel.RefreshToken || user.RefreshTokenExpiryTime <= DateTime.Now)
                return BadRequest("Invalid access token or refresh token");

            var newAccessToken = _tokenService.GenerateJwtToken(user, await _userManager.GetRolesAsync(user));
            var newRefreshToken = _tokenService.GenerateRefreshToken();

            user.RefreshToken = newRefreshToken;
            await _userManager.UpdateAsync(user);

            return new ObjectResult(new
            {
                accessToken = newAccessToken,
                refreshToken = newRefreshToken
            });
        }

        [HttpPost]
        [Route("logout")]
        [Authorize]
        public async Task<IActionResult> Logout()
        {
            var userId = User.FindFirstValue(ClaimTypes.NameIdentifier);
            if (string.IsNullOrEmpty(userId))
                return BadRequest("User not found");

            var user = await _userManager.FindByIdAsync(userId);
            if (user == null)
                return BadRequest("User not found");

            user.RefreshToken = null;
            await _userManager.UpdateAsync(user);
            await _signInManager.SignOutAsync();

            return Ok(new Response { Status = "Success", Message = "Logged out successfully" });
        }

        [HttpGet]
        [Route("confirm-email")]
        public async Task<IActionResult> ConfirmEmail(string userId, string token)
        {
            if (string.IsNullOrEmpty(userId) || string.IsNullOrEmpty(token))
                return BadRequest("Invalid email confirmation link");

            var user = await _userManager.FindByIdAsync(userId);
            if (user == null)
                return BadRequest("User not found");

            var result = await _userManager.ConfirmEmailAsync(user, token);
            if (!result.Succeeded)
                return BadRequest("Email confirmation failed");

            return Redirect("/login?emailConfirmed=true");
        }

        [HttpPost]
        [Route("forgot-password")]
        public async Task<IActionResult> ForgotPassword([FromBody] ForgotPasswordModel model)
        {
            if (!ModelState.IsValid)
                return BadRequest(ModelState);

            var user = await _userManager.FindByEmailAsync(model.Email);
            if (user == null || !(await _userManager.IsEmailConfirmedAsync(user)))
                return Ok(new Response { Status = "Success", Message = "If your email is registered, you will receive a password reset link." });

            var token = await _userManager.GeneratePasswordResetTokenAsync(user);
            var resetLink = Url.Action("ResetPassword", "Auth", new { email = model.Email, token = token }, Request.Scheme);

            var message = new StringBuilder();
            message.AppendLine("<html><body>");
            message.AppendLine("<h1>Password Reset</h1>");
            message.AppendLine($"<p>Hi {user.FirstName},</p>");
            message.AppendLine("<p>You requested a password reset. Please click the link below to reset your password:</p>");
            message.AppendLine($"<p><a href=\"{resetLink}\">Reset Password</a></p>");
            message.AppendLine("<p>If you didn't request this, please ignore this email.</p>");
            message.AppendLine("</body></html>");

            await _emailService.SendEmailAsync(model.Email, "Reset Your Password", message.ToString());

            return Ok(new Response { Status = "Success", Message = "Password reset link has been sent to your email." });
        }

        [HttpGet]
        [Route("reset-password")]
        public IActionResult ResetPassword(string email, string token)
        {
            if (string.IsNullOrEmpty(email) || string.IsNullOrEmpty(token))
                return BadRequest("Invalid password reset link");

            return Redirect($"/reset-password?email={Uri.EscapeDataString(email)}&token={Uri.EscapeDataString(token)}");
        }

        [HttpPost]
        [Route("reset-password")]
        public async Task<IActionResult> ResetPassword([FromBody] ResetPasswordModel model)
        {
            if (!ModelState.IsValid)
                return BadRequest(ModelState);

            var user = await _userManager.FindByEmailAsync(model.Email);
            if (user == null)
                return BadRequest("Invalid reset attempt");

            var result = await _userManager.ResetPasswordAsync(user, model.Token, model.Password);
            if (!result.Succeeded)
                return BadRequest(new Response { Status = "Error", Message = "Password reset failed: " + string.Join(", ", result.Errors.Select(e => e.Description)) });

            return Ok(new Response { Status = "Success", Message = "Password has been reset successfully" });
        }

        [HttpGet]
        [Route("user-info")]
        [Authorize]
        public async Task<IActionResult> GetUserInfo()
        {
            var userId = User.FindFirstValue(ClaimTypes.NameIdentifier);
            if (string.IsNullOrEmpty(userId))
                return Unauthorized();

            var user = await _userManager.FindByIdAsync(userId);
            if (user == null)
                return NotFound("User not found");

            var roles = await _userManager.GetRolesAsync(user);

            return Ok(new UserInfoResponse
            {
                Id = user.Id,
                UserName = user.UserName,
                Email = user.Email,
                FirstName = user.FirstName,
                LastName = user.LastName,
                ProfileImageUrl = user.ProfileImageUrl,
                Title = user.Title,
                Roles = roles.ToList()
            });
        }
    }
}