using System;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace Supermarket_EF.Supermarket
{
    public class SpacificProduct
    {
        [Key]
        public int Id { get; set; }
        public DateTime ExpirationDate { get; set; }
        [Required]
        public int Product_Id { get; set; }
        [ForeignKey("Product_Id")]
        public Product Product { get; set; }
        public DateTime DateOfManufacture { get; set; }
        public int Quantity { get; set; }
    }
}
