using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Web;

namespace WebApplication2.Models
{
    public class Students
    {
        public Students()
        {

        }
        [Required]
        [DataType(DataType.Text)]
        [Display(Name = "Type")]
        public string ID { get; set; }

        [Required]
        [DataType(DataType.Text)]
        [Display(Name = "Type")]
        public string FirstName { get; set; }

        [Required]
        [DataType(DataType.Text)]
        [Display(Name = "Type")]
        public string LastName { get; set; }
    }
}