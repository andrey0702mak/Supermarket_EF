using System;
using System.ComponentModel.DataAnnotations;

namespace Supermarket_EF.Supermarket
{
    public class Category
    {
        [Key]
        public int Id { get; set; }
        [Required]
        public string Name { get; set; }
        public string Title { get; set; }
    }
}
