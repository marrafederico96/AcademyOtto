namespace AuthLibrary.Models
{
    public class UserProductionData
    {
        public int CustomerID { get; set; }
        public string? Title { get; set; }
        public required string FirstName { get; set; }
        public string? MiddleName { get; set; }
        public required string LastName { get; set; }
        public string? Suffix { get; set; }
        public string? CompanyName { get; set; }
        public string? SalesPerson { get; set; }
        public required string EmailAddress { get; set; }
        public string? Phone { get; set; }

    }
}
