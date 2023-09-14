namespace Models.DataTransferObjects.Auth
{
    public class RegistrationResponseDto
    {
        public bool? IsSuccessfulRegistration { get; set; }
        public IEnumerable<string>? Errors { get; set; }
        public string? Error { get; set; }
    }
}
