namespace AdventureWorks.Models.ProductModels
{
    public class Review
    {
        public int Id { get; set; }
        public string ReviewText { get; set; } = string.Empty;
        public bool Class { get; set; }
    }
}
