using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Drawing;
using System.Drawing.Imaging;
using System.IO;

namespace BugTracker.Models
{
    public static class FileUploadValidator
    {
        public static bool IsWebFriendlyFile(HttpPostedFileBase file)
        {
            // check for actual object
            if (file == null)
                return false;
            // check size - file must be less than 2 MB and greater than 1 KB
            if (file.ContentLength > 2 * 1024 * 1024 || file.ContentLength < 1024)
                return false;
            // check for extensions of the file
            string fileExt = Path.GetExtension(file.FileName).ToLower();
            if (fileExt == ".txt" || fileExt == ".pdf" || fileExt == ".doc" || fileExt == ".docx" || fileExt == ".xls" || fileExt == ".xlsx" || fileExt == ".jpg" || fileExt == ".png" || fileExt == ".gif" || fileExt == ".bmp")
            {
                return true;
            }
            else
            {
                return false;

            }
        }
    }
}