using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
using System.Linq;
using System.Threading.Tasks;

namespace BladeRazerExamples.Models
{
    public enum OrderStatus
    {
        Pending = 1,
        Approved = 2,
        Paid = 3,
        Delivered = 4
    }

    public class Order
    {
        [Key]
        public int Id { get; set; }

        [Display(Name ="Product Name")]
        public string  ProductName { get; set; }
        
        [Display(Name = "Product Code")]
        public int ProductCode { get; set; }
        
        [DataType(DataType.Date)]
        public DateTime Date { get; set; }
        
        [Display(Name = "Order Status")]
        public OrderStatus OrderStatus { get; set; }

        [Display(Name = "Customer")]
        public int CustomerId { get; set; }
        
        [ForeignKey("CustomerId")]
        public Customer Customer { get; set; }

    }
}
