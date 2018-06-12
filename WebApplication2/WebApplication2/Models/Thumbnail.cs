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

            string string1 = fullPath;
            string string2 = "Thumbnails\\";
            fullPathToPic = string1.Replace(string2, "");
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
            this.fullPath = thumb.fullPath;

        }

        /// <summary>
        /// convert image to Base64
        /// </summary>
        /// <param name="path"></param>
        /// <returns></returns>
        public string SingleBase64(string fullPath)
        {
            if (System.IO.File.Exists(fullPath))
            {
                byte[] bytes = System.IO.File.ReadAllBytes(fullPath);
                string base64 = System.Convert.ToBase64String(bytes);
                return base64;
            }
            return null;
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

        [Required]
        [DataType(DataType.Text)]
        [Display(Name = "fullPathToPic")]
        public string fullPathToPic { get; set; }

    }
}