using System;
using System.ComponentModel.DataAnnotations;

namespace Supermarket_EF.Supermarket
{
    public class Customer
    {
        [Key]
        public int Id { get; set; }
        public string Name { get; set; }
        public string Soname { get; set; }
        public DateTime DateOfBirth { get; set; }
        public string Department { get; set; }
        public string Email { get; set; }
        public string Phone { get; set; }
        public string Address { get; set; }
        public string City { get; set; }
        public string Region { get; set; }
        [MaxLength(6)]
        public string ZIP { get; set; }
        public DateTime JoinDate { get; set; }
    }
}
