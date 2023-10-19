﻿using DAL.Common;
using Microsoft.AspNetCore.Http;

namespace BusinessCalendar.Helpers
{
    public static class ImageFileHelper
    {
        public static void DeleteImageFile(string name)
        {
            if (name != null)
            {
                var path = Path.Combine(".", ConfigurationHelper.GetString("imagesPath"), name);
                var fileInfo = new FileInfo(path);
                fileInfo.Delete();
            }
        }

        public static void CreateImageFile(IFormFile formFile, string name)
        {
            var filePath = Path.Combine(".", ConfigurationHelper.GetString("imagesPath"), name);
            var fileInfo = new FileInfo(filePath);
            if (!fileInfo.Exists)
            {
                using var fs = fileInfo.Create();
                formFile.CopyTo(fs);
            }
            else
            {
                throw new Exception("Image file already exists");
            }
        }
    }
}