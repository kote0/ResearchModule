﻿using Microsoft.AspNetCore.Http;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.Logging;
using ResearchModule.Components.Models;
using ResearchModule.Components.Models.Interfaces;
using ResearchModule.Components.Page;
using ResearchModule.Extensions;
using ResearchModule.Managers;
using ResearchModule.Managers.Interfaces;
using ResearchModule.Models;
using ResearchModule.Repository;
using ResearchModule.Repository.Interfaces;
using ResearchModule.Service.Interfaces;
using ResearchModule.ViewModels;
using System;
using System.Collections.Generic;
using System.Linq;

namespace ResearchModule.Service
{
    public class PublicationService : IPublicationService
    {
        private readonly PublicationElements publicationElements;
        private readonly IFileManager fileManager;
        private readonly AuthorService authorService;
        private readonly PublicationManager manager;
        private PublicationPage publicationPage;

        public PublicationService(
            PublicationElements publicationElements, IFileManager fileManager,
            AuthorService authorService,
            PublicationManager manager, UserManager userManager,
            SelectListService selectListService)
        {
            this.publicationElements = publicationElements;
            this.fileManager = fileManager;
            this.authorService = authorService;
            this.manager = manager;
            this.publicationPage = new PublicationPage(manager, authorService, 
                this, userManager.CurrentAuthor(), selectListService);
        }



        public string GetFormName(int id)
        {
            var formWork = publicationElements.Forms.FirstOrDefault(o => id.Equals(o.Id));
            return formWork.Name;
        }

        public string GetPartitionName(int id)
        {
            return publicationElements.Partitions.FirstOrDefault(o => o.Id == id).Name;
        }

        #region PublicationsViewModel

        public PublicationsViewModel Page(int first, string action, string controller, string dataId = null)
        {
            return publicationPage.CreatePagination(first, action, controller, dataId) as PublicationsViewModel;
        }

        public PublicationsViewModel Filter(PublicationFilterViewModel filter, int first, string action, string controller, string dataId = null)
        {
            return publicationPage.CreatePagination(filter, first, action, controller, dataId) as PublicationsViewModel;
        }

        #endregion

        public object Load(int id)
        {
            return manager.Get(id);
        }

        

        public IResult Create(CreatePublicationViewModel createPublication)
        {
            var result = new Result();
            result.Model = createPublication;
            //createPublication.Authors = createPublication.Authors.Distinct();
           var selectedAuthors = createPublication.Authors.Where(s => s.Id != 0);
            var createdAuthors = createPublication.Authors.Where(a => a.Id == 0 && a.IsValid());
            var author = createPublication.Authors.Count(a => !a.Coauthor);
            if (author > 1)
                return result.Set("Автором публикации может быть только один");

            if (author == 0)
                return result.Set("Не указан автор публикации");

            // проверка на наличие авторов
            if (createdAuthors.Count() == 0 && selectedAuthors.Count() == 0)
                return result.Set("Отсутсутствуют автор или соавторы");

            var res = manager.AppendFile(createPublication);
            if (res.Failed)
            {
                result.Set(res);
                return result;
            }

            var fileDetails = res.Model as FileDetail;

            if (fileManager.Create(fileDetails).Failed) return result.Set("Не удалось создать Файл");

            var publication = createPublication.Publication;
            if (fileDetails != null)
                AppendFileInfo(fileDetails, ref publication);

            publication.PublicationFileId = fileDetails.Id;

            var typeId = manager.CreateOrUpdatePublicationType(createPublication.Publication.PublicationType, publication.PublicationTypeId);
            if (typeId > 0)
                publication.PublicationTypeId = typeId;

            bool newPublication = publication.Id == 0;
            manager.CreateOrUpdatePublication(publication);

            if (newPublication)
            {
                manager.CreatePA(publication.Id, createPublication.Authors);
            }
            else
            {
                manager.UpdatePA(publication.Id, createdAuthors, selectedAuthors);
            }

            if (result.Failed) return result.Set("Не удалось добавить авторов");

            return result;
        }

        public IResult Create(Publication publication, PublicationType type, IFormFile file, 
            IEnumerable<Author> createdAuthors, IEnumerable<Author> selectedAuthors)
        {
            IResult result = new Result();

            selectedAuthors = selectedAuthors.Where(s => s.Id != 0);
            createdAuthors = createdAuthors.Where(a => a.IsValid());

            // проверка на наличие авторов
            if (createdAuthors.Count() == 0 && selectedAuthors.Count() == 0)
                return result.Set("Отсутсутствуют авторы");

            var fileDetails = fileManager.CreateInfo(file);

            if (fileManager.Create(fileDetails).Failed) return result.Set("Не удалось создать Файл");
            
            if (fileDetails != null)
                AppendFileInfo(fileDetails, ref publication);

            publication.PublicationFileId = fileDetails.Id;

            var typeId = manager.CreateOrUpdatePublicationType(type, publication.PublicationTypeId);
            if (typeId > 0)
                publication.PublicationTypeId = typeId;

            manager.CreateOrUpdatePublication(publication);

            manager.CreatePA(publication.Id, createdAuthors, selectedAuthors);

            if (result.Failed) return result.Set("Не удалось добавить авторов");

            fileManager.SaveFileInServer(fileDetails);

            return result;
        }

        public IResult Update(Publication publication, PublicationType type, IFormFile file, 
            IEnumerable<Author> createdAuthors, IEnumerable<Author> selectedAuthors)
        {
            IResult result = new Result();

            selectedAuthors = selectedAuthors.Where(s => s.Id != 0);
            createdAuthors = createdAuthors.Where(a => a.IsValid());

            // проверка на наличие авторов
            if (createdAuthors.Count() == 0 && selectedAuthors.Count() == 0)
                return result.Set("Отсутсутствуют авторы");

            // изменение файла
            var fileDetails = fileManager.UpdateFile(file, publication.PublicationFileId);
            if (fileDetails != null)
                AppendFileInfo(fileDetails, ref publication);

            // измение вида публикации
            var typeId = manager.CreateOrUpdatePublicationType(type, publication.PublicationTypeId);
            if (typeId > 0)
                publication.PublicationTypeId = typeId;

            // измение публикации
            manager.CreateOrUpdatePublication(publication);

            manager.UpdatePA(publication.Id, createdAuthors, selectedAuthors);

            if (result.Failed) return result.Set("Не удалось добавить авторов");

            if (fileDetails != null) fileManager.SaveFileInServer(fileDetails);

            return result;
        }

        /// <summary>
        /// Добавлении информации в публикацию для элек. и видео файлов
        /// </summary>
        /// <param name="file">Файл</param>
        /// <param name="result">Результат</param>
        /// <returns></returns>
        private void AppendFileInfo(FileDetail fileDetails, ref Publication publication)
        {
            publication.PublicationFile = fileDetails;

            if (publication.PublicationForm.Equals(PublicationElems.electronicSource.Id) || publication.PublicationForm.Equals(PublicationElems.audiovisual.Id))
            {
                publication.Volume = fileDetails.Size;
            }

        }


    }
}
