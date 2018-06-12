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
        [DataType(DataType.Text)]
        [Display(Name = "thumbs")]
        public List<Thumbnail> thumbs { get; set; }

        private string outputDir;

        public int picToDelete { get; set; }
        public Thumbnail thumbToDelete { get; set; }
        public string picToShow { get; set; }


        [Required]
        [DataType(DataType.Text)]
        [Display(Name = "Count")]
        public int count { get; set; }

        public ThumbnailsModel(string outputDir)
        {
            count = 0;
            this.outputDir = outputDir;

            thumbs = new List<Thumbnail>();
            getThumbsFromDir();
            
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

        public int getThumbsFromDir()
        {

            count = 0;
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
                        Thumbnail thumb = new Thumbnail(imageName, year, month, path, count+1);
                        AddThumb(thumb);
                    }

                }
            }
            return count;
        }




    }
}