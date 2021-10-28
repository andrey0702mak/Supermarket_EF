using System;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;


namespace Supermarket_EF.Supermarket
{
    public class Sales
    {
        [Key]
        public int Id { get; set; }
        public int Quantity { get; set; }
        public int Ticket_Id { get; set; }
        public int SpecificProduct_Id { get; set; }
        [ForeignKey("Ticket_Id")]
        public Ticket Ticket { get; set; }
        [ForeignKey("SpecificProduct_Id")]
        public SpacificProduct SpacificProduct { get; set; }
    }
}
