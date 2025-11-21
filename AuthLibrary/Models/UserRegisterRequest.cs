namespace AuthLibrary.Models
{
    public class UserRegisterRequest
    {
        public string? Title { get; set; }

        public required string FirstName { get; set; }

        public string? MiddleName { get; set; }

        public required string LastName { get; set; }

        public string? Suffix { get; set; }

        public string? CompanyName { get; set; }

        public string? SalesPerson { get; set; }

        public required string EmailAddress { get; set; }

        public string? Phone { get; set; }

        public required string Password { get; set; }

        public required string ConfirmPassword { get; set; }

    }
}
