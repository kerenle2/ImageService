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
        string absPath;
        public ImageServiceModal()
        {
            this.m_OutputFolder = ConfigurationManager.AppSettings.Get("OuptputDir");
            this.m_thumbnailSize = Int32.Parse(ConfigurationManager.AppSettings.Get("ThumbnailSize"));

        }
        public string AddFile(string path, out bool result)
        {
            try
            {
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
                if (!Directory.Exists(thumbnailsPath))
                    AddDirectory(thumbnailsPath);
                
                //add the year directory to the original and thumbanil dir
                if (!Directory.Exists(yearPath))
                     AddDirectory(yearPath);
                if (!Directory.Exists(yearPathThumbnails))
                    AddDirectory(yearPathThumbnails);

                //add the months directory to the original and thumbanil dir
                if (!Directory.Exists(yearMonthPath))
                    AddDirectory(yearMonthPath);
                if (!Directory.Exists(yearMonthPathThumbnails))
                    AddDirectory(yearMonthPathThumbnails);


                //copy the image to the new path
                try
                {
                    string absPath = yearMonthPath + imageName;
                    absPath = AppendFileNumberIfExists(absPath, Path.GetExtension(absPath));
                    System.IO.File.Move(path, absPath);
                    result = true;

                //}
                //catch (Exception e)
                //{
                //    result = false;
                //    return e.ToString();
                //}

                //make a thumbanils 
                //try
                //{
                 //   absPath = yearMonthPath + imageName;
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
                return "the transfer was successful, the image is in " + yearMonthPath;
            }
            catch (Exception e)
            {
                result = false;
                return e.ToString();
            }
            }
            



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
                Console.WriteLine("The directory was created successfully at {0}.", Directory.GetCreationTime(path)); //delete

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
            System.Threading.Thread.Sleep(1);
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

        // If the file exists, then do stuff. Otherwise, we just return the original file name.
        if (File.Exists(file)) {
                string folderPath = Path.GetDirectoryName(file); // The path to the file. No sense in dealing with this unecessarily. 
                string fileName = Path.GetFileNameWithoutExtension(file); // The file name with no extension. 
                string extension = string.Empty; // The file extension. 
                // This lets us pass in an empty string for the file extension if required. i.e. It just makes this function a bit more versatile. 
                if (ext == string.Empty) { 
                        extension = Path.GetExtension(file);
                }
                else {
                        extension = ext;
                }
 
                // at this point, find out if the fileName ends in a number, then get that number.
                int fileNumber = 0; // This stores the number as a number for us. 
                // need a regex here - \(([0-9]+)\)$
                Regex r = new Regex(@"\(([0-9]+)\)$"); // This matches the pattern we are using, i.e. ~(#).ext
                Match m = r.Match(fileName); // We pass in the file name with no extension.
                string addSpace = " "; // We'll add a space when we don't have our pattern in order to pad the pattern.
                if (m.Success) {
                        addSpace = string.Empty; // We have the pattern, so we don't add a space - it has already been added. 
                        string s = m.Groups[1].Captures[0].Value; // This is the single capture that we are looking for. Stored as a string.
                        // set fileNumber to the new number.
                        fileNumber = int.Parse(s); // Convert the number to an int.
                        // remove the numbering from the string as we're constructing it again below.
                        fileName = fileName.Replace("(" + s + ")", "");
                }                 
                
                // Start looping. 
                do
                {
                        fileNumber += 1; // Increment the file number that we have above. 
                        file = Path.Combine(folderPath, // Combine it all.
                                                String.Format("{0}{3}({1}){2}", // The pattern to combine.
                                                                          fileName,         // The file name with no extension. 
                                                                          fileNumber,       // The file number.
                                                                          extension,        // The file extension.
                                                                          addSpace));       // A space if needed to pad the initial ~(#).ext pattern.
                        }
                while (File.Exists(file)); // As long as the file name exists, keep looping. 
        }
        return file;
}




        #endregion
    }
}
