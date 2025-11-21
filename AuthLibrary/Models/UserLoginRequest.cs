namespace AuthLibrary.Models
{
    public record UserLoginRequest
    {
        public required string EmailAddress { get; set; }
        public required string Password { get; set; }
    }
}
