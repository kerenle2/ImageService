using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.IO;
using System.Linq;
using System.Web;

namespace WebApplication2.Models
{

    public class ThumbnailsModel
    {

        [Required]
        [Display(Name = "thumbs")]
        public List<Thumbnail> thumbs { get; set; }

        private string outputDir;

        //count number ot thumbs       
        public int count { get; set; }

        public ThumbnailsModel(string outputDir)
        {
            count = 0;
            this.outputDir = outputDir;

            thumbs = new List<Thumbnail>();
            getThumbsFromDir(outputDir);
        }
        /// <summary>
        /// add a thumbnail to the list
        /// </summary>
        /// <param name="thumb"></param>
        public void AddThumb(Thumbnail thumb)
        {
            this.thumbs.Add(thumb);
            count++;
        }
        /// <summary>
        /// delete thumbnail from list
        /// </summary>
        /// <param name="thumb"></param>
        public void deleteThumb(Thumbnail thumb)
        {
            this.thumbs.Remove(thumb);
            this.count--;
        }

        /// <summary>
        /// get all yhumbs from the output directory
        /// </summary>
        /// <param name="outputDir"></param>
        public void getThumbsFromDir(string outputDir)
        {
            thumbs.Clear(); //yes? im going this way?
            string[] extensions = { ".jpg", ".png", ".gif", ".bmp" };

            if (Directory.Exists(outputDir + "\\Thumbnails"))
                {
                    string[] paths = Directory.GetFiles(outputDir + "\\Thumbnails", "*.*", SearchOption.AllDirectories);
                    foreach (string path in paths)
                {
                    if(extensions.Contains(Path.GetExtension(path)))
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