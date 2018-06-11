using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.IO;
using System.Linq;
using System.Web;
using System.Web.Hosting;

namespace WebApplication2.Models
{

    public class ThumbnailsModel
    {

        [Required]
      //[DataType(DataType.)] // ??
        [Display(Name = "thumbs")]
        public List<Thumbnail> thumbs { get; set; }

        private string outputDir;

        //[Required]
        //[DataType()] // ??
        //[Display(Name = "Count")]
        public int count { get; set; }

        public ThumbnailsModel(string outputDir)
        {
            count = 0;
            this.outputDir = outputDir;

            thumbs = new List<Thumbnail>();
            getThumbsFromDir(outputDir);
            
        }

        public void AddThumb(Thumbnail thumb)
        {
            this.thumbs.Add(thumb);
            count++;
        }

        public void deleteThumb(Thumbnail thumb)
        {
            this.thumbs.Remove(thumb);
            this.count--;
        }

        public void getThumbsFromDir(string outputDir)
        {


            thumbs.Clear(); //yes? im going this way?
            string[] extensions = { ".jpg", ".png", ".gif", ".bmp" };

            if (Directory.Exists(outputDir + "\\Thumbnails"))
            {
                string[] paths = Directory.GetFiles(outputDir + "\\Thumbnails", "*.*", SearchOption.AllDirectories);
                foreach (string path in paths)
                {
                    if (extensions.Contains(Path.GetExtension(path)))
                    {
                        string month = Path.GetFileName(Path.GetDirectoryName(path));
                        string year = Path.GetFileName(Path.GetDirectoryName(Path.GetDirectoryName(path)));
                        string imageName = Path.GetFileName(path);

                        Thumbnail thumb = new Thumbnail(imageName, year, month, path);
                        AddThumb(thumb);
                    }

                }
            }

        }




    }
}