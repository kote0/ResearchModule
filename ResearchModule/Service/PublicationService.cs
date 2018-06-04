using Microsoft.AspNetCore.Http;
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
            PublicationManager manager, UserManager userManager)
        {
            this.publicationElements = publicationElements;
            this.fileManager = fileManager;
            this.authorService = authorService;
            this.manager = manager;
            this.publicationPage = new PublicationPage(manager, authorService, this, userManager);
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

        public CreatePublicationViewModel Create(CreatePublicationViewModel createPublication)
        {
            //var file = fileManager.CreateInfo(createPublication.FormFile);
            var oldFileName = createPublication.OldFileName;
            if (createPublication.FormFile != null)
            {
                // изменить на Uid
                // FileName не нужен, так как используется Html.FileFor
                createPublication.OldFileName = string.IsNullOrEmpty(oldFileName) 
                    ? createPublication.FormFile.FileName
                    : !oldFileName.Equals(createPublication.FormFile.FileName)
                        ? createPublication.FormFile.FileName
                        : oldFileName;
            }
            return createPublication;
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
            var electronicSource = (int)PublicationElements.FormEnum.electronic_source;
            var audioVisual = (int)PublicationElements.FormEnum.audiovisual;

            publication.PublicationFile = fileDetails;

            if (publication.PublicationForm.Equals(electronicSource) || publication.PublicationForm.Equals(audioVisual))
            {
                publication.Volume = fileDetails.Size;
            }

        }


    }
}
