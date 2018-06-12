using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.ComponentModel.DataAnnotations;
using System.Web.Hosting;
using System.IO;

namespace WebApplication2.Models
{
    public class ImageWebModel
    {
        static string StudentsPath = HostingEnvironment.MapPath("~/App_Data/Students.txt");
        //make a list with the details of the students
        public List<Students> StudentsData = new List<Students>()
        {
            new Students {ID = getStudents()[0], FirstName = getStudents()[1], LastName = getStudents()[2] },
            new Students {ID = getStudents()[3], FirstName = getStudents()[4], LastName = getStudents()[5] }
        };
        [Required]
        [DataType(DataType.Text)]
        [Display(Name = "IsConnect")]
        public string IsConnect { get; set; }

        [Required]
        [DataType(DataType.Text)]
        [Display(Name = "ImagesNum")]
        public int ImagesNum { get; set; }


        public ImageWebModel()
        {
        
        }
        /// <summary>
        /// get the students details from file
        /// </summary>
        /// <returns></returns>
        static string[] getStudents()
        {
            string[] data = null;
            using (var reader = new StreamReader(StudentsPath))
            {
                data = reader.ReadLine().Split(null);
            }
            return data;
        }
        
    }
}