using ImageService.Infrastructure;

using System;
using System.Collections.Generic;
using System.Drawing;
using System.Drawing.Imaging;
using System.IO;
using System.Linq;
using System.Text;
using System.Threading;
using System.Text.RegularExpressions;
using System.Threading.Tasks;
using System.Configuration;

namespace ImageService.Modal
{
    public class ImageServiceModal: IImageServiceModal
    {
        #region Members
        private string m_OutputFolder;            // The Output Folder
        private int m_thumbnailSize;              // The Size Of The Thumbnail Size

        public ImageServiceModal()
        {
            this.m_OutputFolder = ConfigurationManager.AppSettings.Get("OuptputDir");
            this.m_thumbnailSize = Int32.Parse(ConfigurationManager.AppSettings.Get("ThumbnailSize"));

        }
        public string AddFile(string path, out bool result)
        {
            DateTime date = GetDateTakenFromImage(path);
           
            string year = date.Year.ToString();
            string month = date.Month.ToString();
            //add the Thumbnaile directory 
            string thumbnailsPath = Path.Combine(m_OutputFolder, "Thumbnails");
            AddDirectory(thumbnailsPath);

            //add the year directory to the original and thumbanil dir
            string yearPath = Path.Combine(m_OutputFolder, year);
            AddDirectory(yearPath);
            string yearPathThumbnails = Path.Combine(thumbnailsPath, year);
            AddDirectory(yearPathThumbnails);


            //add the months directory to the original and thumbanil dir
            string monthPath = Path.Combine(yearPath, month);
            AddDirectory(monthPath);
            string monthPathThumbnails = Path.Combine(thumbnailsPath, month);
            AddDirectory(monthPathThumbnails);

            //copy the image to the new path
            try
            {
                System.IO.File.Move(m_OutputFolder, monthPath);
                result = true;
                
            }
            catch (Exception e)
            {
                result = false;
                return e.ToString();
            }

            //make a thumbanils 
            try
            {
                Image image = Image.FromFile(m_OutputFolder);
                Image thumb = image.GetThumbnailImage(m_thumbnailSize, m_thumbnailSize, () => false, IntPtr.Zero);
                thumb.Save(monthPathThumbnails);
                
            }
            catch (Exception e)
            {
                result = false;
                return e.ToString();
            }
            //if all success, return the new path of the image
            return "the transfer was successful, the image is in " + monthPath;
        }
        public void AddDirectory(string path)
        {
            try
            {
                // Determine whether the directory exists.
                if (Directory.Exists(path))
                {
                    Console.WriteLine("That path exists already."); //delete
                    return;
                }

                // Try to create the directory.
                DirectoryInfo di = Directory.CreateDirectory(path);
                Console.WriteLine("The directory was created successfully at {0}.", Directory.GetCreationTime(path)); //delete

                //// Delete the directory.
                //di.Delete();
                //Console.WriteLine("The directory was deleted successfully.");
            }
            catch (Exception e)
            {
                Console.WriteLine("The process failed: {0}", e.ToString());
            }
            finally { }

        }
        //we init this once so that if the function is repeatedly called
        //it isn't stressing the garbage man
        private static Regex r = new Regex(":");

        //retrieves the datetime WITHOUT loading the whole image
        public static DateTime GetDateTakenFromImage(string path)
        {
            using (FileStream fs = new FileStream(path, FileMode.Open, FileAccess.Read))
            using (Image myImage = Image.FromStream(fs, false, false))
            {
                PropertyItem propItem = myImage.GetPropertyItem(36867);
                string dateTaken = r.Replace(Encoding.UTF8.GetString(propItem.Value), "-", 2);
                return DateTime.Parse(dateTaken);
            }
        }

        #endregion
    }
}
