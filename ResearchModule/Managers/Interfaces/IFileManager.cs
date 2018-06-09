using Microsoft.AspNetCore.Http;
using ResearchModule.Components.Models;
using ResearchModule.Components.Models.Interfaces;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace ResearchModule.Managers.Interfaces
{
    public interface IFileManager
    {
        /// <summary>
        /// Создание инфо. о файле
        /// </summary>
        /// <param name="file"></param>
        /// <returns></returns>
        FileDetail CreateInfo(IFormFile file);

        /// <summary>
        /// Создание записи о файле
        /// </summary>
        /// <param name="fileDetails"></param>
        /// <returns></returns>
        IResult CreateOrUpdate(FileDetail fileDetails);

        /// <summary>
        /// Сохранение файла в директории Files
        /// </summary>
        void SaveFileInServer(FileDetail fileDetails);

        /// <summary>
        /// Удаление файла из сервера
        /// </summary>
        void Delete(string uid);

        /// <summary>
        /// Измение информации о файле
        /// </summary>
        /// <param name="file"></param>
        /// <param name="publicationFileId"></param>
        /// <returns></returns>
        FileDetail UpdateFile(IFormFile file, int publicationFileId);

        FileDetail Get(int id);

        FileDetail Get(string uid);

        string Download(string uid);

        string GetContentType(string name);

        void Save();
    }
}
