﻿using System.ComponentModel.DataAnnotations;

namespace Models.DataTransferObjects.Auth
{
    public class AuthResponseDto
    {
        public bool IsAuthSuccessful { get; set; }
        [EmailAddress]
        public string? Email { get; set; }
        public string? ErrorMessage { get; set; }
        public string? Token { get; set; }
        public bool? Is2StepVerificationRequired { get; set; }
        public string? Provider { get; set; }
    }
}
