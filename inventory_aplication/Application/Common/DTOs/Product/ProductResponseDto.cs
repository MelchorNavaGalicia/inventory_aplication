namespace inventory_aplication.Application.Common.DTOs.Product
{
    public class ProductResponseDto
    {
        public int Id { get; set; }
        public string Name { get; set; }
        public string Description { get; set; }
        public int CategoryId { get; set; }
        public string CategoryName { get; set; }
        public int Stock { get; set; }
        public decimal Price { get; set; }
    }
}
