using ImageService.Logging;
using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Web;

namespace WebApplication2.Models
{
    public class LogModel: CommunicationModel
    {
        [Required]
        [DataType(DataType.Text)]
        [Display(Name = "Type")]
        public string Type { get;  set; }

        [Required]
        [DataType(DataType.Text)]
        [Display(Name = "Message")]
        public string Message { get;  set; }


    }


}