using System;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace Supermarket_EF.Supermarket
{
    public class Product
    {
        [Key]
        public int Id { get; set; }
        [Required]
        public string Name { get; set; }
        public double Price { get; set; }
        public string Brand { get; set; }
        public string Barcode { get; set; }
        public int Category_Id { get; set; }
        [ForeignKey("Category_Id")]
        public Category Category { get; set; }
        public string OtherDetails { get; set; }
        public string SKU { get; set; }
    }
}
