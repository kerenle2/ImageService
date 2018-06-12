using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Web;

namespace WebApplication2.Models
{
    public class Thumbnail
    {
        public Thumbnail(string name, string year, string month, string path)
        {
            this.name = name;
            this.month = month;
            this.year = year;
            this.path = path;
        }
        /// <summary>
        /// copt the details of thumbnail to property
        /// </summary>
        /// <param name="thumb"></param>
        public void copy(Thumbnail thumb)
        {
            this.name = thumb.name;
            this.year = thumb.year;
            this.month = thumb.month;
            this.path = thumb.path;
        }

        [Required]
        [DataType(DataType.Text)]
        [Display(Name = "Name")]
        public string name{ get; set; }

        [Required]
        [DataType(DataType.Text)]
        [Display(Name = "Year")]
        public string year { get; set; }

        [Required]
        [DataType(DataType.Text)]
        [Display(Name = "Month")]
        public string month { get; set; }

        [Required]
        [DataType(DataType.Text)]
        [Display(Name = "Path")]
        public string path { get; set; }
    }
}