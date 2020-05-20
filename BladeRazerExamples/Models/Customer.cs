using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Threading.Tasks;

namespace BladeRazerExamples.Models
{

    public enum CustomerType
    {
        Cash = 1,
        Credit = 2
    }

    public class Customer
    {
        [Key]
        public int Id { get; set; }
        
        [Display(Name = "First Name")]
        public string FirstName { get; set; }
        
        [Display(Name = "Last Name")]
        public string LastName { get; set; }
        
        public bool Active { get; set; }
        
        public string Telephone { get; set; }
        
        public string Email { get; set; }
        
        public string Address { get; set; }
        
        public CustomerType CustomerType { get; set; }
        
        [Display(Name = "Name")]
        public string FullName => $"{FirstName} {LastName}";
    }
}
