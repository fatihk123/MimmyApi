namespace ApiProjeKampi.WebApi.Entities
{
    public class Product
    {

        public int ProductId { get; set; }
        public string ProductName { get; set; } = string.Empty;
        public string ProductDescription { get; set; } = string.Empty;
        public decimal Price { get; set; }
        public string ImageUrl { get; set; } = string.Empty;
        public int? CategoryId { get; set; }
        public Category? Category { get; set; }

    }
}
