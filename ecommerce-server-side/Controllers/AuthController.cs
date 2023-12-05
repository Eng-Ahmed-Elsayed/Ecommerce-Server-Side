using System.Data;
using System.Security.Claims;
using AutoMapper;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Identity;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.WebUtilities;
using Models.DataTransferObjects;
using Models.DataTransferObjects.Auth;
using Models.Models;
using Security.JWT;
using Utility.Email;
using Utility.ManageFiles;

namespace ecommerce_server_side.Controllers
{
    [Route("api/auth")]
    [ApiController]
    public class AuthController : ControllerBase
    {
        private readonly UserManager<User> _userManager;
        private readonly IMapper _mapper;
        private readonly IManageFiles _manageFiles;
        private readonly JwtHandler _jwtHandler;
        private readonly IEmailSender _emailSender;

        public AuthController(UserManager<User> userManager,
            IMapper mapper,
            JwtHandler jwtHandler,
            IEmailSender emailSender,
            IManageFiles manageFiles)
        {
            _userManager = userManager;
            _mapper = mapper;
            _jwtHandler = jwtHandler;
            _emailSender = emailSender;
            _manageFiles = manageFiles;
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
            try
            {
                if (userForRegistrationDto == null || !ModelState.IsValid)
                {
                    var error = string.Join(" | ", ModelState.Values
                       .SelectMany(v => v.Errors)
                       .Select(e => e.ErrorMessage));
                    return BadRequest(new RegistrationResponseDto { Error = error, IsSuccessfulRegistration = false });
                }
                var user = _mapper.Map<User>(userForRegistrationDto);
                user.CreatedAt = DateTime.Now;

                var result = await _userManager.CreateAsync(user, userForRegistrationDto.Password);
                if (!result.Succeeded)
                {
                    var errors = result.Errors.Select(e => e.Description);
                    return BadRequest(new RegistrationResponseDto { Errors = errors, IsSuccessfulRegistration = false });
                }

                await SendEmailConfirmationEmail(user, userForRegistrationDto.ClientURI);

                await _userManager.AddToRoleAsync(user, "Viewer");
                return StatusCode(201, new RegistrationResponseDto { IsSuccessfulRegistration = true });
            }
            catch (Exception ex)
            {
                return StatusCode(500, $"The server encountered an unexpected condition. Please try again later.");
            }

        }

        // Login action
        [HttpPost("login")]
        public async Task<IActionResult> Login([FromBody] UserForAuthenticationDto userForAuthentication)
        {
            try
            {
                if (!ModelState.IsValid)
                {
                    return BadRequest();
                }

                var user = await _userManager.FindByNameAsync(userForAuthentication.UserName);
                if (user == null)
                {
                    return BadRequest("Invalid username or password.");
                }

                // Check if the account is locked out
                if (await _userManager.IsLockedOutAsync(user))
                {
                    var content = $"Your account is locked out. To reset the password click this link: {userForAuthentication.ClientURI}";
                    var message = new Message(new string[] { user.Email },
                        "Locked out account information", content);
                    await _emailSender.SendEmailAsync(message);
                    return Unauthorized(new AuthResponseDto { ErrorMessage = "Your account is locked out, please reset your password." });
                }

                // Check the password
                if (!await _userManager.CheckPasswordAsync(user, userForAuthentication.Password))
                {
                    await _userManager.AccessFailedAsync(user);
                    return Unauthorized(new AuthResponseDto { ErrorMessage = "Invalid username or password." });
                }

                // Check if the email is confirmed
                if (!await _userManager.IsEmailConfirmedAsync(user))
                {
                    return Unauthorized(new AuthResponseDto
                    {
                        ErrorMessage = "Email is not confirmed."
                    });
                }

                // Check if the 2-Step Verification is enabled
                if (await _userManager.GetTwoFactorEnabledAsync(user))
                {
                    return await GenerateOTPFor2StepVerification(user);
                }
                // Generate jwt token
                string token = await _jwtHandler.GenerateToken(user);
                // Rest the access failed count
                await _userManager.ResetAccessFailedCountAsync(user);

                return Ok(new AuthResponseDto { IsAuthSuccessful = true, Token = token, Email = user.Email });
            }

            catch (Exception ex)
            {
                return StatusCode(500, $"The server encountered an unexpected condition. Please try again later.");
            }
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
            // If last email was sent before one or more mins.
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

            // If last email was sent before one or more mins.
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
                    user = new User { Email = payload.Email, UserName = payload.Email, CreatedAt = DateTime.Now };
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

        [Authorize]
        [HttpGet]
        public async Task<IActionResult> GetUser()
        {
            try
            {
                var user = await _userManager.FindByEmailAsync(User.FindFirst(ClaimTypes.Email)?.Value);
                if (user == null)
                {
                    return NotFound();
                }
                var userDto = _mapper.Map<UserDto>(user);
                return Ok(userDto);
            }
            catch (Exception ex)
            {
                return StatusCode(500, $"The server encountered an unexpected condition. Please try again later.");
            }
        }

        [Authorize]
        [HttpPut]
        public async Task<IActionResult> UpdateUserPassword(UserChangePasswordDto userChangePasswordDto)
        {
            try
            {
                if (ModelState.IsValid)
                {
                    var user = await _userManager.FindByEmailAsync(User.FindFirst(ClaimTypes.Email)?.Value);
                    if (user == null)
                    {
                        return NotFound();
                    }
                    var result = await _userManager.ChangePasswordAsync(user,
                        userChangePasswordDto.CurrentPassword,
                        userChangePasswordDto.Password);
                    if (result.Succeeded)
                    {
                        return Ok();
                    }
                    return BadRequest();
                }
                else
                {
                    var errors = new Dictionary<string, string[]>();
                    foreach (var key in ModelState.Keys)
                    {
                        var state = ModelState[key];
                        if (state.Errors.Count > 0)
                        {
                            errors[key] = state.Errors.Select(e => e.ErrorMessage).ToArray();
                        }
                    }

                    return BadRequest(new ValidationProblemDetails(errors));
                }

            }
            catch (Exception ex)
            {
                return StatusCode(500, $"The server encountered an unexpected condition. Please try again later.");
            }
        }

        [Authorize, HttpPatch]
        public async Task<IActionResult> UpdateUser(UserDto userDto)
        {
            try
            {
                if (ModelState.IsValid)
                {
                    var user = await _userManager.FindByEmailAsync(User.FindFirst(ClaimTypes.Email)?.Value);
                    if (user == null)
                    {
                        return NotFound();
                    }
                    // Update fields
                    user.FirstName = userDto.FirstName;
                    user.LastName = userDto.LastName;
                    user.PhoneNumber = userDto.PhoneNumber;
                    user.Birthdate = userDto.Birthdate;
                    user.UpdatedAt = DateTime.Now;
                    var result = await _userManager.UpdateAsync(user);
                    if (result.Succeeded)
                    {
                        return Ok();
                    }
                    return BadRequest();
                }
                else
                {
                    var errors = new Dictionary<string, string[]>();
                    foreach (var key in ModelState.Keys)
                    {
                        var state = ModelState[key];
                        if (state.Errors.Count > 0)
                        {
                            errors[key] = state.Errors.Select(e => e.ErrorMessage).ToArray();
                        }
                    }

                    return BadRequest(new ValidationProblemDetails(errors));
                }
            }
            catch (Exception ex)
            {
                return StatusCode(500, $"The server encountered an unexpected condition. Please try again later.");
            }
        }

        [HttpPost, DisableRequestSizeLimit]
        [Route("upload-file")]
        [Authorize]
        public async Task<IActionResult> UploadUserImage()
        {
            try
            {
                var formCollection = await Request.ReadFormAsync();
                var file = formCollection.Files.First();
                if (file.Length > 0)
                {
                    var dbPath = _manageFiles.UploadFile(file, "User");
                    var user = await _userManager.FindByEmailAsync(User.FindFirst(ClaimTypes.Email)?.Value);
                    _manageFiles.DeleteImage(user.ImgPath);
                    user.ImgPath = dbPath;
                    var result = await _userManager.UpdateAsync(user);
                    if (result.Succeeded)
                    {
                        return Ok();
                    }
                }
                return BadRequest();
            }
            catch (Exception ex)
            {
                return StatusCode(500, $"The server encountered an unexpected condition. Please try again later.");
            }
        }

    }
}
