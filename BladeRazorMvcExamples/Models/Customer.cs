using BladeRazor.Attributes;
using System;
using System.ComponentModel.DataAnnotations;

namespace BladeRazorMvcExamples.Models
{

    public enum CustomerType
    {
        Cash = 1,
        Credit = 2
    }

    public class Customer
    {
        [Key]
        [Form(FormInputType.Hidden, DisplayView = false)]
        public int Id { get; set; }

        [Display(Name = "Name")]
        public string FullName => $"{FirstName} {LastName}";

        [Display(Name = "First Name")]
        [Form(DisplayView = false)]
        public string FirstName { get; set; }

        [Required]
        [Display(Name = "Last Name")]
        [Form(DisplayView = false)]
        public string LastName { get; set; }

        [DataType(DataType.PhoneNumber)]
        public string Telephone { get; set; }

        [DataType(DataType.EmailAddress)]
        public string Email { get; set; }

        [DataType(DataType.MultilineText)]
        public string Address { get; set; }

        [Display(Name = "Customer Type")]
        [Form(FormInputType.Select, SelectOptionName = "Select")]
        public CustomerType CustomerType { get; set; }

        [Form(DisplayView = false)]
        public int CustomerNumber { get; set; }

        public bool Active { get; set; } = true;

        [DataType(DataType.Date)]       
        public DateTime Registered { get; set; } = DateTime.Today;

    }
}
