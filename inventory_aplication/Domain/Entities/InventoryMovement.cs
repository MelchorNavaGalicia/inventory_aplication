using System;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace inventory_application.Data.Entities
{
    [Table("InventoryMovements")]
    public class InventoryMovement
    {
        [Key]
        [DatabaseGenerated(DatabaseGeneratedOption.Identity)]
        public int Id { get; set; }
        [Required]
        public int ProductId { get; set; }
        [Required]
        public int Quantity { get; set; }

        [Required]
        [MaxLength(10)]
        public string MovementType { get; set; }
        [Required]
        public DateTime MovementDate { get; set; } = DateTime.UtcNow;
        [Required]
        public int UserId { get; set; }

        [Required]
        public bool IsActive { get; set; } = true;
        public DateTime CreatedAt { get; set; } = DateTime.UtcNow;
    }
}
