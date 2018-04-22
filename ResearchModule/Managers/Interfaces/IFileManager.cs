﻿using Microsoft.AspNetCore.Http;
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
        FileDetail Create(IFormFile file);

        /// <summary>
        /// Создание записи о файле
        /// </summary>
        /// <param name="fileDetails"></param>
        /// <returns></returns>
        IResult Create(FileDetail fileDetails);

        /// <summary>
        /// Изменение записи о файле
        /// </summary>
        /// <param name="fileDetail"></param>
        /// <returns></returns>
        IResult Update(FileDetail fileDetail);

        /// <summary>
        /// Создание и сохранение файла
        /// </summary>
        /// <param name="file"></param>
        /// <returns></returns>
        FileDetail CreateAndSave(IFormFile file);

        /// <summary>
        /// Сохранение файла в директории Files
        /// </summary>
        void SaveFIle(FileDetail fileDetails);

        /// <summary>
        /// Удаление файла из сервера
        /// </summary>
        void Delete();
    }
}