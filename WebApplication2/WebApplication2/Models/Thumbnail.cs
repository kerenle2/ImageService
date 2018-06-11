using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Web;

namespace WebApplication2.Models
{
    public class Thumbnail
    {
        public Thumbnail(string name, string year, string month, string fullPath, int picNumber)
        {
            this.name = name;
            this.month = month;
            this.year = year;
            this.fullPath = fullPath;
            this.picNumber = picNumber;
        }

        public void copy(Thumbnail thumb)
        {
            this.name = thumb.name;
            this.year = thumb.year;
            this.month = thumb.month;
            this.fullPath = thumb.fullPath;
        }

        [Required]
        [DataType(DataType.Text)]
        [Display(Name = "name")]
        public string name{ get; set; }

        [Required]
        [DataType(DataType.Text)]
        [Display(Name = "year")]
        public string year { get; set; }

        [Required]
        [DataType(DataType.Text)]
        [Display(Name = "month")]
        public string month { get; set; }

        [Required]
        [DataType(DataType.Text)]
        [Display(Name = "path")]
        public string fullPath { get; set; }

        [Required]
        [DataType(DataType.Text)]
        [Display(Name = "picNumber")]
        public int picNumber { get; set; }

    }
}