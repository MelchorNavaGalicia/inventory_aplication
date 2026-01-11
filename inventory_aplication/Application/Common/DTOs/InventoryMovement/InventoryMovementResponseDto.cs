namespace inventory_aplication.Application.Common.DTOs.InventoryMovement
{
    public class InventoryMovementResponseDto
    {
        public int Id {  get; set; }
        public int ProductId { get; set; }
        public int CategoryId { get; set; }
        public int UserId { get; set; }
        public int Quantity { get; set; }
        public string MovementType { get; set; }
        public string CategoryName { get; set; }
        public string ProductName { get; set; }
        public string MovementBy { get; set; }
        public DateTime MovementDate { get; set; }
    }
}
