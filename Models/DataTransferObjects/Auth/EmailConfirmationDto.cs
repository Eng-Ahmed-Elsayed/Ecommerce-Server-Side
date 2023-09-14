namespace Models.DataTransferObjects.Auth
{
    public class EmailConfirmationDto
    {
        public string? Email { get; set; }
        public string? Token { get; set; }
    }
}
