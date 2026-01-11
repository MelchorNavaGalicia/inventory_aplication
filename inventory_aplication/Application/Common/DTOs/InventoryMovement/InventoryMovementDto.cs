namespace inventory_aplication.Application.Common.DTOs.InventoryMovement
{
    public class InventoryMovementDto
    {
        public int ProductId { get; set; }
        public int Quantity { get; set; }
        public string MovementType { get; set; } // "IN" o "OUT"
    }
}
