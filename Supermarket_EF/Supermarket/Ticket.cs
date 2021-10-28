using System;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace Supermarket_EF.Supermarket
{
    public class Ticket
    {
        [Key]
        public int Id { get; set; }
        public DateTime Date { get; set; }
        public int Employee_ID { get; set; }
        public int Customer_ID { get; set; }
        public int Location_ID { get; set; }
        public int Payment_ID { get; set; }
        [ForeignKey("Employee_ID")]
        public Employee Employee { get; set; }
        [ForeignKey("Customer_ID")]
        public Customer Customer { get; set; }
        [ForeignKey("Location_ID")]
        public Location Location { get; set; }
        [ForeignKey("Payment_ID")]
        public Payment Payment { get; set; }
    }
}
