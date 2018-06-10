using ImageService.Communication;
using ImageService.Infrastructure.Enums;
using ImageService.Logging;
using ImageService.Logging.Model;
using Newtonsoft.Json;
using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Web;

namespace WebApplication2.Models
{
    public class LogModel
    {
        
        [Required]
        [DataType(DataType.Text)]
        [Display(Name = "Type")]
        public string Type { get;  set; }

        [Required]
        [DataType(DataType.Text)]
        [Display(Name = "Message")]
        public string Message { get;  set; }

        public LogModel()
        {
            
        }

      
       

    }


}