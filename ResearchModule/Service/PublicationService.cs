using Microsoft.AspNetCore.Http;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.Logging;
using ResearchModule.Components.Models;
using ResearchModule.Components.Models.Interfaces;
using ResearchModule.Extensions;
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
        private readonly IBaseRepository repository;
        private readonly PARepository paRepository;
        private readonly PublicationElements publicationElements;
        private readonly IFileManager fileManager;
        private readonly ILogger logger;
        private readonly AuthorService authorService;
        private Result result;

        public PublicationService(PARepository paRepository, IBaseRepository repository,
            PublicationElements publicationElements, IFileManager fileManager, ILogger<PublicationService> logger, AuthorService authorService)
        {
            this.repository = repository;
            this.paRepository = paRepository;
            this.publicationElements = publicationElements;
            this.logger = logger;
            this.fileManager = fileManager;
            this.authorService = authorService;
            this.result = new Result();
        }

        public PublicationsViewModel Filter(PublicationFilterViewModel filter)
        {
            var query = repository.GetQuery<Publication>(p => Contains(p, ref filter));
            var list = repository.Page(query, 1).ToList();
            var viewModel = Page(list);
            viewModel.PublicationFilterViewModel = filter;
            return viewModel;
        }

        private bool Contains(Publication model, ref PublicationFilterViewModel filter)
        {
            if (filter.Publication != null)
            {
                var name = filter.Publication.PublicationName;
                if (!string.IsNullOrEmpty(name) && model.PublicationName.ToLower().Contains(name.ToLower()))
                    return true;

                if (!string.IsNullOrEmpty(filter.Publication.OutputData)
                    && model.OutputData.ToLower().Contains(filter.Publication.OutputData))
                    return true;
            }
            if (filter.PublicationType.Count > 0)
            {
                var type = filter.PublicationType.Where(t => t.Id == model.PublicationTypeId).FirstOrDefault();
                if (type != null)
                    return true;
            }
            return false;
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

        public PublicationsViewModel Page(int first = 1)
        {
            var publications = repository.Page<Publication>(first);
            return Page(publications, first);
        }

        public PublicationsViewModel Page(IEnumerable<Publication> publications,  int first = 1)
        {
            return CreateView(publications, first);
        }

        public PublicationsViewModel Search(string character)
        {
            var publications = repository.Get<Publication>(a => a.Contains(character));
            var modelView = CreateView(publications);
            return modelView;
        }


        public PublicationsViewModel CreateView(IEnumerable<Publication> list, int first = 1)
        {
            var pageInfo = new PageInfo(first, list.Count());
            return new PublicationsViewModel(this, authorService, list, pageInfo);
        }

        #endregion

        public Publication LoadWithFile(int id)
        {
            var publication = repository
                .Include<Publication, FileDetail>(f => f.PublicationFile)
                .Where(i => i.Id == id).FirstOrDefault();

            return publication;
        }

        public Publication Load(int id)
        {
            var publication = repository
                .Include<Publication, FileDetail>(f => f.PublicationFile)
                .Include(r => r.PublicationType)
                .Include(r => r.PAs)
                    .ThenInclude(w => w.Multiple)
                .Include(r => r.PFs)
                    .ThenInclude(w => w.Multiple)
                .Where(i => i.Id == id).FirstOrDefault();

            return publication;
        }

        #region create


        /// <summary>
        /// Создание публикации
        /// </summary>
        /// <param name="publication">Публикация</param>
        /// <param name="type">ВИд публикации</param>
        /// <param name="file">Вложение</param>
        /// <param name="createdAuthors">Авторы для создания</param>
        /// <param name="selectedAuthors">Найденные авторы</param>
        /// <returns></returns>
        public IResult Create(Publication publication, PublicationType type, IFormFile file, IEnumerable<Author> createdAuthors, IEnumerable<Author> selectedAuthors)
        {
            selectedAuthors = selectedAuthors.Where(s => s.Id != 0);
            createdAuthors = createdAuthors.Where(a => a.IsValid());

            // проверка на наличие авторов
            if (createdAuthors.Count() == 0 && selectedAuthors.Count() == 0)
                result.Set("Отсутсутствуют авторы");

            if (result.Failed) return result;

            var fileDetails = fileManager.Create(file);
            result.Set(fileManager.Create(fileDetails).Error);

            if (fileDetails == null || result.Failed) return result.Set("Не удалось создать Файл");

            FileInformation(fileDetails, ref publication);
            CreatePublicationType(type, ref publication);

            if (result.Failed) return result.Set("Не удалось создать Вид публикации");

            CreateOrUpdatePublication(ref publication);

            if (result.Failed) return result.Set("Не удалось создать/изменить Публикацию");

            CreatePA(publication.Id, createdAuthors, selectedAuthors);

            if (result.Failed) return result.Set("Не удалось добавить авторов");

            fileManager.SaveFIle(fileDetails);

            return result;
        }

        /// <summary>
        /// Добавлении информации в публикацию для элек. и видео файлов
        /// </summary>
        /// <param name="file">Файл</param>
        /// <param name="result">Результат</param>
        /// <returns></returns>
        private void FileInformation(FileDetail fileDetails, ref Publication publication)
        {
            var electronicSource = (int)PublicationElements.FormEnum.electronic_source;
            var audioVisual = (int)PublicationElements.FormEnum.audiovisual;

            publication.PublicationFile = fileDetails;

            if (publication.PublicationForm.Equals(electronicSource) || publication.PublicationForm.Equals(audioVisual))
            {
                publication.Volume = fileDetails.Size;
            }

        }

        /// <summary>
        /// Сохранение изменений в PublicationType
        /// </summary>
        /// <param name="publicationType"></param>
        /// <param name="publication"></param>
        /// <param name="result"></param>
        private void CreatePublicationType(PublicationType publicationType, ref Publication publication)
        {
            if (publicationType.IsValid())
            {
                result.Set(repository.Create(publicationType)
                    .Error);
            }
            else if (publicationType.Id != 0)
            {
                result.Set(repository.Update(publicationType)
                    .Error);
            }
            publication.PublicationTypeId = publicationType.Id;
        }

        /// <summary>
        /// Изменение публикации
        /// </summary>
        /// <param name="publication"></param>
        /// <param name="result"></param>
        private void CreateOrUpdatePublication(ref Publication publication)
        {
            publication.ModifyDate = DateTime.Now;
            if (publication.Id != 0)
            {
                result.Set(repository.Update(publication).Error);
            }
            else
            {
                publication.CreateDate = DateTime.Now;
                result.Set(repository.Create(publication).Error);
            }
        }

        /// <summary>
        /// Создание записи в РА
        /// </summary>
        /// <param name="publicationId"></param>
        /// <param name="createdAuthors"></param>
        /// <param name="selectedAuthors"></param>
        /// <param name="result"></param>
        private void CreatePA(int publicationId, IEnumerable<Author> createdAuthors, IEnumerable<Author> selectedAuthors)
        {
            var listAuthors = new List<Author>();
            listAuthors.AddRange(createdAuthors);
            // найти существующих авторов
            // если существуют вернуть их Id
            // для создании записи
            listAuthors.AddRange(selectedAuthors);
            result.Set(paRepository.Create(listAuthors, publicationId).Error);
        }

        #endregion

        #region update

        public IResult Update(Publication publication, PublicationType type, IFormFile file, IEnumerable<Author> createdAuthors, IEnumerable<Author> selectedAuthors)
        {
            selectedAuthors = selectedAuthors.Where(s => s.Id != 0);
            createdAuthors = createdAuthors.Where(a => a.IsValid());

            // проверка на наличие авторов
            if (createdAuthors.Count() == 0 && selectedAuthors.Count() == 0)
                return result.Set("Отсутсутствуют авторы");
            
            var fileDetails = UpdateFile(file, ref publication);
            CreatePublicationType(type, ref publication);
            CreateOrUpdatePublication(ref publication);

            if (result.Failed) return result.Set("Не удалось создать/изменить Публикацию");

            UpdatePA(publication.Id, createdAuthors, selectedAuthors);

            if (result.Failed) return result.Set("Не удалось добавить авторов");

            if (fileDetails != null) fileManager.SaveFIle(fileDetails);

            return result;
        }

        private FileDetail UpdateFile(IFormFile file, ref Publication publication)
        {
            if (file != null)
            {
                var fileDetails = fileManager.Create(file);
                var id = publication.PublicationFileId;
                var fileInfo = repository.Get<FileDetail>(id);
                // удалить файл из сервера по fileInfo.Uid
                //fileManager.Delete();
                fileDetails.Id = id;
                result.Set(fileManager.Update(fileDetails).Error);
                FileInformation(fileDetails, ref publication);

                return fileDetails;
            }
            return null;
        }

        /// <summary>
        /// Изменение записи в РА
        /// </summary>
        /// <param name="publicationId"></param>
        /// <param name="createdAuthors"></param>
        /// <param name="selectedAuthors"></param>
        /// <param name="result"></param>
        private void UpdatePA(int publicationId, IEnumerable<Author> createdAuthors, IEnumerable<Author> selectedAuthors)
        {
            result.Set(paRepository.Create(createdAuthors, publicationId).Error);
            // найти существующих авторов
            // если существуют вернуть их Id
            // для создании записи
            if (selectedAuthors.Count() != 0)
            {
                var pas = repository.GetQuery<PA>(o => o.PublicationId == publicationId).ToList();
                foreach (var item in pas)
                {
                    var author = selectedAuthors.FirstOrDefault(s => s.Id == item.MultipleId);
                    if (author != null)
                    {
                        result.Set(paRepository.Update(author, publicationId).Error);
                    }
                    else
                    {
                        result.Set(paRepository.Delete(item).Error);
                    }
                }
            }
            
        }

        #endregion


    }
}
