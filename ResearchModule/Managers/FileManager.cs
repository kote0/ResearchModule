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
        private static string filesDirectory
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

        private readonly IBaseRepository repository;

        public FileManager(IBaseRepository repository)
        {
            this.repository = repository;
        }

        public FileDetail Create(IFormFile file)
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

        public IResult Create(FileDetail fileDetails)
        {
            return repository.Create(fileDetails);
        }

        public IResult Update(FileDetail fileDetail)
        {
            return repository.Update(fileDetail);

        }

        public FileDetail CreateAndSave(IFormFile file)
        {
            var fileDetails = Create(file);
            SaveFIle(fileDetails);
            return fileDetails;
        }

        public void Delete()
        {
            //TODO: Реализовать
            throw new NotImplementedException();
        }

        public void SaveFIle(FileDetail fileDetails)
        {
            using (var stream = new FileStream(Path.Combine(filesDirectory, fileDetails.Uid), FileMode.Create))
            {
                fileDetails.FormFile.CopyTo(stream);
            }
        }
    }
}
