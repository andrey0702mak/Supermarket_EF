using System;
using System.ComponentModel.DataAnnotations;

namespace Supermarket_EF.Supermarket
{
    public class Payment
    {
        [Key]
        public int Id { get; set; }
        public string Type { get; set; }
        public string Brand { get; set; }
    }
}
