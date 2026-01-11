namespace inventory_aplication.Application.Common.DTOs.Product
{
    public class ProductCreateDto
    {
        public string Name { get; set; }
        public string Description { get; set; }
        public int CategoryId { get; set; }
        public int Stock { get; set; }
        public decimal Price { get; set; }
    }
}
