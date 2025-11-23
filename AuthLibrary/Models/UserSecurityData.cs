namespace AuthLibrary.Models
{
    public class UserSecurityData
    {
        public int CustomerID { get; set; }
        public string EmailAddress { get; set; } = string.Empty;
        public string PasswordHash { get; set; } = string.Empty;
        public string PasswordSalt { get; set; } = string.Empty;
        public Guid Rowguid { get; set; }
        public DateTime ModifiedDate { get; set; }
    }

}
