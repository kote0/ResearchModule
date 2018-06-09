using Microsoft.AspNetCore.Http;
using ResearchModule.Components.Models;
using ResearchModule.Components.Models.Interfaces;
using ResearchModule.Managers.Interfaces;
using ResearchModule.Repository.Interfaces;
using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Threading.Tasks;

namespace ResearchModule.Managers
{
    public class FileManager : IFileManager
    {
        private static string directory
        {
            get
            {
                var path = Path.Combine(AppDomain.CurrentDomain.BaseDirectory, "Files");
                // Проверка на существование директории Files
                if (!Directory.Exists(path))
                {
                    Directory.CreateDirectory(path);
                }
                return path;
            }
        }

        private static Dictionary<string, string> GetMimeTypes()
        {
            return new Dictionary<string, string>
            {
                {".txt", "text/plain"},
                {".pdf", "application/pdf"},
                {".doc", "application/vnd.ms-word"},
                {".docx", "application/vnd.ms-word"},
                {".xls", "application/vnd.ms-excel"},
                {".xlsx", "application/vnd.openxmlformats"},
                {".png", "image/png"},
                {".jpg", "image/jpeg"},
                {".jpeg", "image/jpeg"},
                {".gif", "image/gif"},
                {".csv", "text/csv"}
            };
        }

        private readonly IBaseRepository repository;

        public FileManager(IBaseRepository repository)
        {
            this.repository = repository;
        }

        public FileDetail CreateInfo(IFormFile file)
        {
            if (file == null) return null;

            if (file.Length == 0 || string.IsNullOrEmpty(file.FileName)) return null;

            FileDetail fileDetails = new FileDetail()
            {
                Uid = Guid.NewGuid().ToString("N"),
                Size = file.Length,
                Name = file.FileName,
                FormFile = file
            };

            return fileDetails;
        }

        public IResult CreateOrUpdate(FileDetail fileDetails)
        {
            if (fileDetails.Id != 0)
                return repository.Update(fileDetails);
            return repository.Add(fileDetails);
        }

        public void Save()
        {
            repository.Save();
        }

        /*public FileDetail CreateAndSave(IFormFile file)
        {
            var fileDetails = CreateInfo(file);

            SaveFileInServer(fileDetails);
            return fileDetails;
        }*/

        public void Delete(string uid)
        {
            var file = string.Concat(directory, uid);
            if (File.Exists(file))
                File.Delete(file);
        }

        public void SaveFileInServer(FileDetail fileDetails)
        {
            using (var stream = new FileStream(Path.Combine(directory, fileDetails.Uid), FileMode.Create))
            {
                fileDetails.FormFile.CopyTo(stream);
            }
        }

        public FileDetail UpdateFile(IFormFile file, int publicationFileId)
        {
            if (file != null)
            {
                var fileDetails = CreateInfo(file);
                var id = publicationFileId;
                var fileInfo = repository.Get<FileDetail>(id);
                Delete(fileInfo.Uid);
                fileDetails.Id = id;
                var res = Update(fileDetails);
                if (res.Succeeded)
                    return fileDetails;
            }
            return null;
        }

        private IResult Update(FileDetail fileDetails)
        {
            throw new NotImplementedException();
        }

        public FileDetail Get(int id)
        {
            return repository.Get<FileDetail>(id);
        }

        public FileDetail Get(string uid)
        {
            return repository.First<FileDetail>(a=>a.Uid.Equals(uid));
        }

        public string Download(string uid)
        {
            return Path.Combine(directory, uid);
        }

        public string GetContentType(string name)
        {
            var types = GetMimeTypes();
            var ext = Path.GetExtension(name).ToLowerInvariant();
            return types[ext];
        }

        
    }
}
