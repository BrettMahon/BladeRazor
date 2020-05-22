using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
using System.Linq;
using System.Threading.Tasks;
using BladeRazor.Attributes;

namespace BladeRazorExamples.Models
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

        [Form(FormInputType.TextArea, TextAreaRows =4)]
        public string Notes { get; set; }

        [Display(Name = "Order Status")]
        public OrderStatus OrderStatus { get; set; }

        [Display(Name = "Customer")]      
        [Form(DisplayView = false)]
        public int CustomerId { get; set; }
        
        [ForeignKey("CustomerId")]
        [Display(Name = "Customer")]
        [Form(ComplexDisplayProperty = "FullName")]
        public Customer Customer { get; set; }

    }
}
