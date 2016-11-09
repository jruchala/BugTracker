﻿using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Drawing;
using System.Drawing.Imaging;

namespace BugTracker.Models
{
    public class ImageUploadValidator
    {
        public bool IsWebFriendlyImage(HttpPostedFileBase file)
        {
            // Check if there is an object
            if (file == null)
                return false;
            // check size: must be less than 2 MB and grreater than 1 KB
            if (file.ContentLength > 2 * 1024 * 1024 || file.ContentLength < 1024)
                return false;

            try
            {
                using (var img = Image.FromStream(file.InputStream))
                {
                    return ImageFormat.Jpeg.Equals(img.RawFormat) ||
                        ImageFormat.Png.Equals(img.RawFormat) ||
                        ImageFormat.Gif.Equals(img.RawFormat);
                }
            }

            catch
            {
                return false;
            }
        }
    }
}