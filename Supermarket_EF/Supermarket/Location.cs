using System;
using System.ComponentModel.DataAnnotations;

namespace Supermarket_EF.Supermarket
{
    public class Location
    {
        [Key]
        public int Id { get; set; }
        public string City { get; set; }
        public string Country { get; set; }
        public string BranchName { get; set; }
        public string Address { get; set; }
    }
}
