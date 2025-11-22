namespace AuthLibrary.Models
{
    public class UserSecurityData
    {
        public int CustomerID { get; set; }
        public string PasswordHash { get; set; } = string.Empty;
        public string PasswordSalt { get; set; } = string.Empty;
        public DateTime ModifiedDate { get; set; }
    }

}
