using Microsoft.AspNetCore.Http;
using ResearchModule.Components.Models;
using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Threading.Tasks;

namespace ResearchModule.Managers
{
    public class FileManager
    {
        private string filesDirectory
        {
            get
            {
                var path = Path.Combine(Directory.GetCurrentDirectory(), "Files");
                // Проверка на существование директории Files
                if (!Directory.Exists(path))
                {
                    Directory.CreateDirectory(path);
                }
                return path;
            }
        }

        /// <summary>
        /// Создание инфо. о файле
        /// </summary>
        /// <param name="file"></param>
        /// <returns></returns>
        public FileDetails CreateFileDetails(IFormFile file)
        {
            if (file == null) return null;
            if (file.Length == 0 || string.IsNullOrEmpty(file.FileName)) return null;
            FileDetails fileDetails = new FileDetails()
            {
                Uid = Guid.NewGuid().ToString("N"),
                Size = file.Length,
                Name = file.FileName,
                FormFile = file
            };
            return fileDetails;
        }

        /// <summary>
        /// Сохранение файла в директории Files
        /// </summary>
        public void SaveFile(FileDetails fileDetails)
        {
            using (var stream = new FileStream(Path.Combine(filesDirectory, fileDetails.Uid), FileMode.Create))
            {
                fileDetails.FormFile.CopyTo(stream);
            }
        }
    }
}
