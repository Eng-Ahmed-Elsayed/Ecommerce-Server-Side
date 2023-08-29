using AutoMapper;
using Microsoft.AspNetCore.Identity;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.WebUtilities;
using Models.DataTransferObjects;
using Models.Models;
using Security.JWT;
using Utility.Email;

namespace ecommerce_server_side.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    public class AuthController : ControllerBase
    {
        private readonly UserManager<User> _userManager;
        private readonly IMapper _mapper;
        private readonly JwtHandler _jwtHandler;
        private readonly IEmailSender _emailSender;

        public AuthController(UserManager<User> userManager,
            IMapper mapper,
            JwtHandler jwtHandler,
            IEmailSender emailSender)
        {
            _userManager = userManager;
            _mapper = mapper;
            _jwtHandler = jwtHandler;
            _emailSender = emailSender;

        }

        // Method to send confirmation email
        private async Task SendEmailConfirmationEmail(User user, string ClientURI)
        {
            var token = await _userManager.GenerateEmailConfirmationTokenAsync(user);
            var param = new Dictionary<string, string?>
            {
                {"token", token},
                {"email", user.Email }
            };
            var callback = QueryHelpers.AddQueryString(ClientURI, param);

            var message = new Message(new string[] { user.Email }, "Email Confirmation token", callback);
            await _emailSender.SendEmailAsync(message);
        }

        [HttpPost("registration")]
        public async Task<IActionResult> RegisterUser(
            [FromBody] UserForRegistrationDto userForRegistrationDto)
        {
            if (userForRegistrationDto == null || !ModelState.IsValid)
            {
                return BadRequest();
            }
            var user = _mapper.Map<User>(userForRegistrationDto);

            var result = await _userManager.CreateAsync(user, userForRegistrationDto.Password);
            if (!result.Succeeded)
            {
                var errors = result.Errors.Select(e => e.Description);
                return BadRequest(new RegistrationResponseDto { Errors = errors, IsSuccessfulRegistration = false });
            }

            await SendEmailConfirmationEmail(user, userForRegistrationDto.ClientURI);

            await _userManager.AddToRoleAsync(user, "Viewer");
            return StatusCode(201);
        }

        // Login action
        [HttpPost("login")]
        public async Task<IActionResult> Login([FromBody] UserForAuthenticationDto userForAuthentication)
        {
            var user = await _userManager.FindByNameAsync(userForAuthentication.Email);
            if (user == null)
            {
                return BadRequest("Invalid Request");
            }

            if (await _userManager.IsLockedOutAsync(user))
            {
                var content = $"Your account is locked out. To reset the password click this link: {userForAuthentication.ClientURI}";
                var message = new Message(new string[] { userForAuthentication.Email },
                    "Locked out account information", content);
                await _emailSender.SendEmailAsync(message);
                return Unauthorized(new AuthResponseDto { ErrorMessage = "The account is locked out" });
            }

            if (!await _userManager.CheckPasswordAsync(user, userForAuthentication.Password))
            {
                await _userManager.AccessFailedAsync(user);
                return Unauthorized(new AuthResponseDto { ErrorMessage = "Invalid Authentication" });
            }

            if (!await _userManager.IsEmailConfirmedAsync(user))
            {
                return Unauthorized(new AuthResponseDto
                {
                    ErrorMessage = "Email is not confirmed"
                });
            }

            if (await _userManager.GetTwoFactorEnabledAsync(user))
            {
                return await GenerateOTPFor2StepVerification(user);
            }

            string token = await _jwtHandler.GenerateToken(user);

            await _userManager.ResetAccessFailedCountAsync(user);

            return Ok(new AuthResponseDto { IsAuthSuccessful = true, Token = token, Email = user.Email });
        }

        // OTP for the 2-Step Verification Process
        private async Task<IActionResult> GenerateOTPFor2StepVerification(User user)
        {
            var providers = await _userManager.GetValidTwoFactorProvidersAsync(user);
            if (!providers.Contains("Email"))
            {
                return Unauthorized(new AuthResponseDto
                {
                    ErrorMessage = "Invalid 2-Step Verification Provider.",
                    IsAuthSuccessful = false
                });
            }
            var token = await _userManager.GenerateTwoFactorTokenAsync(user, "Email");
            var message = new Message(new string[] { user.Email }, "Authentication token", token);
            await _emailSender.SendEmailAsync(message);

            return Ok(new AuthResponseDto
            {
                Is2StepVerificationRequired = true,
                Provider = "Email",
            });
        }

        // TwoStepVerification Login Action
        [HttpPost("two-step-verification")]
        public async Task<IActionResult> TwoStepVerification([FromBody] TwoFactorDto twoFactorDto)
        {
            if (!ModelState.IsValid)
            {
                return BadRequest();
            }

            var user = await _userManager.FindByEmailAsync(twoFactorDto.Email);
            if (user == null)
            {
                return BadRequest("Invalid Request");
            }
            bool validVerification = await _userManager.VerifyTwoFactorTokenAsync(user,
                twoFactorDto.Provider, twoFactorDto.Token);
            if (!validVerification)
            {
                return BadRequest("Invalid Token Verification");
            }

            string token = await _jwtHandler.GenerateToken(user);

            return Ok(new AuthResponseDto { IsAuthSuccessful = true, Token = token, Email = user.Email });
        }

        [HttpPost("forgot-password")]
        public async Task<IActionResult> ForgotPassword([FromBody] ForgotPasswordDto forgotPasswordDto)
        {
            if (!ModelState.IsValid)
            {
                return BadRequest();
            }

            var user = await _userManager.FindByEmailAsync(forgotPasswordDto.Email);
            if (user == null)
            {
                return BadRequest("Invalid Request");
            }

            var token = await _userManager.GeneratePasswordResetTokenAsync(user);
            var param = new Dictionary<string, string?>
            {
                {"token", token},
                {"email", forgotPasswordDto.Email}
            };

            var callback = QueryHelpers.AddQueryString(forgotPasswordDto.ClientURI, param);
            var message = new Message(new string[] { user.Email }, "Reset password token", callback);
            await _emailSender.SendEmailAsync(message);

            return Ok();
        }

        [HttpPost("reset-password")]
        public async Task<IActionResult> RestPassword([FromBody] ResetPasswordDto resetPasswordDto)
        {
            if (!ModelState.IsValid)
            {
                return BadRequest();
            }
            var user = await _userManager.FindByEmailAsync(resetPasswordDto.Email);

            if (user == null)
            {
                return BadRequest("Invalid request.");
            }
            var restPasswordResult = await _userManager.ResetPasswordAsync(user,
                resetPasswordDto.Token, resetPasswordDto.Password);

            if (!restPasswordResult.Succeeded)
            {
                var errors = restPasswordResult.Errors.Select(e => e.Description);
                return BadRequest(new { Errors = errors });
            }

            await _userManager.SetLockoutEndDateAsync(user, new DateTime(2000, 1, 1));

            return Ok();
        }

        [HttpPost("email-confirmation")]
        public async Task<IActionResult> EmailConfirmation([FromBody] EmailConfirmationDto emailConfirmationDto)
        {
            if (!ModelState.IsValid)
            {
                return BadRequest();
            }
            var user = await _userManager.FindByEmailAsync(emailConfirmationDto.Email);
            if (user == null)
            {
                return BadRequest("Invalid email confirmation request");
            }

            if (user.EmailConfirmed)
            {
                return BadRequest("Your email is already confirmed");
            }

            var confirmResult = await _userManager.ConfirmEmailAsync(user, emailConfirmationDto.Token);
            if (!confirmResult.Succeeded)
            {
                var errors = confirmResult.Errors.Select(e => e.Description);
                return BadRequest(errors);
            }
            return Ok();
        }

        [HttpPost("send-email-confirmation")]
        public async Task<IActionResult> SendEmailConfirmation([FromBody] SendEmailConfirmationDto sendEmailConfirmationDto)
        {
            var user = await _userManager.FindByEmailAsync(sendEmailConfirmationDto.Email);
            if (user == null)
            {
                return BadRequest("Invalid Request");
            }
            if (user.EmailConfirmed)
            {
                return BadRequest("Your email is already confirmed");
            }
            await SendEmailConfirmationEmail(user, sendEmailConfirmationDto.ClientURI);
            return Ok();
        }

        [HttpPost("external-login")]
        public async Task<IActionResult> ExternalLogin([FromBody] ExternalAuthDto externalAuth)
        {
            var payload = await _jwtHandler.VerifyGoogleToken(externalAuth);
            if (payload == null)
                return BadRequest("Invalid External Authentication.");

            var info = new UserLoginInfo(externalAuth.Provider, payload.Subject, externalAuth.Provider);

            var user = await _userManager.FindByLoginAsync(info.LoginProvider, info.ProviderKey);
            if (user == null)
            {
                user = await _userManager.FindByEmailAsync(payload.Email);

                if (user == null)
                {
                    user = new User { Email = payload.Email, UserName = payload.Email };
                    await _userManager.CreateAsync(user);

                    //prepare and send an email for the email confirmation

                    await _userManager.AddToRoleAsync(user, "Viewer");
                    await _userManager.AddLoginAsync(user, info);
                }
                else
                {
                    await _userManager.AddLoginAsync(user, info);
                }
            }

            if (user == null)
                return BadRequest("Invalid External Authentication.");

            //check for the Locked out account

            var token = await _jwtHandler.GenerateToken(user);
            return Ok(new AuthResponseDto { Token = token, IsAuthSuccessful = true, Email = user.Email });
        }

        [HttpPost("logout")]
        public async Task<IActionResult> Logout([FromBody] LogoutDto revokeTokenDto)
        {
            if (!ModelState.IsValid)
            {
                return BadRequest("Invalid Authentication");
            }

            var user = await _userManager.FindByEmailAsync(revokeTokenDto.Email);
            if (user == null)
            {
                return BadRequest("Invalid Authentication");
            }
            // TBD: We need to black list the token because we can not revoke the token directly.
            return Ok();
        }
    }
}
