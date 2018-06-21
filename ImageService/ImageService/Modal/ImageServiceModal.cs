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
using System.Globalization;

namespace ImageService.Modal
{
    public class ImageServiceModal: IImageServiceModal
    {
        #region Members
        private string m_OutputFolder;            // The Output Folder
        private int m_thumbnailSize;              // The Size Of The Thumbnail Size
        #endregion
        /// <summary>
        /// constructor
        /// </summary>
        public ImageServiceModal(string m_OutputFolder, int m_thumbnailSize)
        {
            this.m_OutputFolder = m_OutputFolder;
            this.m_thumbnailSize = m_thumbnailSize;
        }
        /// <summary>
        /// add the image to the output dir
        /// </summary>
        /// <param name="path"></param>
        /// <param name="result"></param>
        /// <returns></returns>
        public string AddFile(string path, out bool result)
        {
            try
            {
                //make outputdir be a hidden dir
                if (!Directory.Exists(m_OutputFolder))
                {
                    DirectoryInfo di = Directory.CreateDirectory(m_OutputFolder);
                    di.Attributes = FileAttributes.Directory | FileAttributes.Hidden;
                }
                //get the date from the image
                DateTime date = GetDateTakenFromImage(path);

                //create strings:
                string year = date.Year.ToString();
                string month = date.Month.ToString();
                string thumbnailsPath = Path.Combine(m_OutputFolder, "Thumbnails");
                string yearPath = Path.Combine(m_OutputFolder, year);
                string yearPathThumbnails = Path.Combine(thumbnailsPath, year);
                string yearMonthPath = Path.Combine(yearPath, month);
                string yearMonthPathThumbnails = Path.Combine(yearPathThumbnails, month);
                string imageName = path.Substring(path.LastIndexOf("\\"));


                //add the Thumbnaile directory 
                AddDirectory(thumbnailsPath);

                //add the year directory to the original and thumbanil dir
                AddDirectory(yearPath);
                AddDirectory(yearPathThumbnails);

                //add the months directory to the original and thumbanil dir
                AddDirectory(yearMonthPath);
                AddDirectory(yearMonthPathThumbnails);


                //copy the image to the new path
                try
                {
                    string absPath = yearMonthPath + imageName;
                     //absPath = AppendFileNumberIfExists(absPath, Path.GetExtension(absPath));

                    //instead of append number, delete  photo if allready exists:
                    if (File.Exists(absPath))
                    {
                        File.Delete(absPath);
                    }

                    System.IO.File.Move(path, absPath);
                    result = true;
                    Image image = Image.FromFile(absPath);
                    Image thumb = image.GetThumbnailImage(m_thumbnailSize, m_thumbnailSize, () => false, IntPtr.Zero);
                    string thumbAbsPath = yearMonthPathThumbnails + imageName;
                    thumbAbsPath = AppendFileNumberIfExists(thumbAbsPath, Path.GetExtension(thumbAbsPath));
                    thumb.Save(thumbAbsPath);
                    result = true;

                }
                catch (Exception e)
                {
                    result = false;
                    return e.ToString();
                }

                //if all success, return the new path of the image
                return "the transfer was successful, the image from " + path + " is moved to " + yearMonthPath;

                
            }
            catch (Exception e)
            {
                result = false;
                return e.ToString();
            }
        }

        /// <summary>
        /// if the dir is not exists already - create it.
        /// </summary>
        /// <param name="path"></param>
        public void AddDirectory(string path)
        {
            try
            {
                // Determine whether the directory exists.
                if (Directory.Exists(path))
                {
                    return;
                }
                // Try to create the directory.
                DirectoryInfo di = Directory.CreateDirectory(path);
            }
            catch (Exception e)
            {
                Console.WriteLine("The process failed: {0}", e.ToString());
            }
            finally { }

        }
       
        private static Regex r = new Regex(":");

        
        /// <summary>
        /// take the datetime from image
        /// </summary>
        /// <param name="path"></param>
        /// <returns></returns>
        public static DateTime GetDateTakenFromImage(string path)
        {
            System.Threading.Thread.Sleep(1000);
            using (FileStream fs = new FileStream(path, FileMode.Open, FileAccess.Read))
            using (Image myImage = Image.FromStream(fs, false, false))
            {
                PropertyItem propertyItem = myImage.PropertyItems.FirstOrDefault(i => i.Id == 306);
                if (propertyItem == null)
                {
                    
                    return DateTime.Now;
                }
                else
                {
                    // Extract the property value as a String. 
                    ASCIIEncoding encoding = new ASCIIEncoding();
                    string text = encoding.GetString(propertyItem.Value, 0, propertyItem.Len - 1);

                    // Parse the date and time. 
                    CultureInfo provider = CultureInfo.InvariantCulture;
                    return DateTime.ParseExact(text, "yyyy:MM:d H:m:s", provider);
                }
            }
        }
        /// <summary>
        /// A function to add an incremented number at the end of a file name if a file already exists. 
        /// </summary>
        /// <param name="file">The file. This should be the complete path.</param>
        /// <param name="ext">This can be empty.</param>
        /// <returns>An incremented file name. </returns>
        private string AppendFileNumberIfExists(string file, string ext)
        {
            if (File.Exists(file)) {
                string folderPath = Path.GetDirectoryName(file);  
                string fileName = Path.GetFileNameWithoutExtension(file); 
                string extension = string.Empty; 
                if (ext == string.Empty) { 
                        extension = Path.GetExtension(file);
                }
                else {
                        extension = ext;
                }
 
                // if the fileName ends in a number- get that number.
                int fileNumber = 0; 
                Regex r = new Regex(@"\(([0-9]+)\)$"); 
                Match m = r.Match(fileName); 
                string addSpace = " "; 
                if (m.Success) {
                        addSpace = string.Empty;  
                        string s = m.Groups[1].Captures[0].Value; 
                        fileNumber = int.Parse(s); 
                        fileName = fileName.Replace("(" + s + ")", "");
                }                 
                //check what is the last number of the extension, and add the next number 
                do
                {
                    fileNumber += 1; 
                    file = Path.Combine(folderPath, String.Format("{0}{3}({1}){2}", 
                        fileName, fileNumber, extension, addSpace));
                    }
                while (File.Exists(file)); // As long as the file name exists, keep looping. 
            }
            return file;
        }
        
    }
}
