using BladeRazor.Attributes;
using System;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace BladeRazorMvcExamples.Models
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
        [Form(FormInputType.Hidden)]
        public int Id { get; set; }

        [Display(Name = "Product Name")]
        public string ProductName { get; set; }

        [Display(Name = "Product Code")]
        public int ProductCode { get; set; } = 0;

        [DataType(DataType.Date)]
        public DateTime Date { get; set; } = DateTime.Today;

        [Form(FormInputType.TextArea, TextAreaRows = 2)]
        public string Notes { get; set; }

        [Display(Name = "Order Status")]
        [Form(FormInputType.Select, SelectItemsKey = "OrderStatus", SelectOptionName = "Select")]
        public OrderStatus OrderStatus { get; set; }

        [Display(Name = "Customer")]
        [Form(FormInputType.Select, DisplayView = false, SelectItemsKey = "Customers", SelectOptionName = "Select")]
        public int CustomerId { get; set; }

        [ForeignKey("CustomerId")]
        [Display(Name = "Customer")]
        [Form(ComplexDisplayProperty = "FullName")]
        public Customer Customer { get; set; }

    }
}
