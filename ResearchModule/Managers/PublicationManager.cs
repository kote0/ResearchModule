using Microsoft.AspNetCore.Http;
using Microsoft.EntityFrameworkCore;
using ResearchModule.Components.Models;
using ResearchModule.Components.Models.Interfaces;
using ResearchModule.Managers.Interfaces;
using ResearchModule.Models;
using ResearchModule.Repository;
using ResearchModule.Repository.Interfaces;
using ResearchModule.ViewModels;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Linq.Expressions;
using System.Threading.Tasks;

namespace ResearchModule.Managers
{
    public class PublicationManager
    {
        private readonly IBaseRepository repository;
        private readonly IFileManager fileManager;
        private readonly PAuthorRepository paRepository;

        public PublicationManager(IBaseRepository repository, IFileManager fileManager,
             PAuthorRepository paRepository)
        {
            this.repository = repository;
            this.fileManager = fileManager;
            this.paRepository = paRepository;
        }

        public IResult AppendFile(CreatePublicationViewModel createPublication)
        {
            var res = new Result();

            if (createPublication.FormFile == null)
                return res.Set("Не добавлен файл");

            var oldFileName = createPublication.OldFileName;
            // изменить на Uid
            var fileName = string.IsNullOrEmpty(oldFileName)
                ? createPublication.FormFile.FileName
                : !oldFileName.Equals(createPublication.FormFile.FileName)
                    ? createPublication.FormFile.FileName
                    : oldFileName;

            var file = fileManager.CreateInfo(createPublication.FormFile);
            if (!fileName.Equals(oldFileName))
            {
                fileManager.SaveFileInServer(file);
            }

            res.Model = file;
            return res;
        }

        public long Count(Expression<Func<Publication, bool>> func)
        {
            return repository.LongCount(func);
        }

        public long Count()
        {
            return repository.LongCount<Publication>();
        }

        public object Get(int id)
        {
            return repository
                .Include<Publication, FileDetail>(f => f.PublicationFile)
                .Include(r => r.PublicationType)
                .Include(r => r.PAs)
                    .ThenInclude(w => w.Multiple)
                .Include(r => r.PFs)
                    .ThenInclude(w => w.Multiple)
                .Where(i => i.Id == id).FirstOrDefault();
        }

        public IQueryable<Publication> FilterQuery(PublicationFilterViewModel filter)
        {
            return repository.GetQuery<Publication>(p => Contains(p, filter));
        }

        public IQueryable<Publication> Page(int first)
        {
            return repository.Page<Publication>(first);
        }
        
        public object CreateOrUpdatePublication(Publication publication)
        {
            publication.ModifyDate = DateTime.Now;
            if (publication.Id != 0)
            {
                repository.Update(publication);
                return publication;
            }

            publication.CreateDate = DateTime.Now;
            repository.Add(publication);
            return publication;
        }

        private bool Contains(Publication model, PublicationFilterViewModel filter)
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
            if (filter.PublicationTypes != null)
            {
                var type = filter.PublicationTypes.Where(t => t.Id == model.PublicationTypeId).FirstOrDefault();
                if (type != null)
                    return true;
            }
            return false;
        }

        #region otherTypes

        public void UpdatePublicationType(PublicationType publicationType)
        {
            repository.Update(publicationType);
        }

        /// <summary>
        /// Сохранение изменений в PublicationType
        /// </summary>
        public int CreateOrUpdatePublicationType(PublicationType publicationType, int publicationTypeId)
        {
            if (publicationType == null || (publicationType != null && !publicationType.IsValid()))
                return -1; // не успех

            if (publicationTypeId != 0)
            {
                publicationType.Id = publicationTypeId;
                UpdatePublicationType(publicationType);
                return 0; // id не изменилось
            }

            repository.Add(publicationType);
            return publicationType.Id;
        }

        public FileDetail UpdateFile(IFormFile file, int publicationFileId)
        {
            if (file != null)
            {
                var fileDetails = fileManager.CreateInfo(file);
                var id = publicationFileId;
                var fileInfo = repository.Get<FileDetail>(id);
                // удалить файл из сервера по fileInfo.Uid
                //fileManager.Delete();
                fileDetails.Id = id;
                fileManager.Update(fileDetails);
                return fileDetails;
            }
            return null;
        }

        /// <summary>
        /// Изменение записи в РА
        /// </summary>
        /// <param name="publicationId"></param>
        /// <param name="createdAuthors">Авторы не существующие в системе</param>
        /// <param name="selectedAuthors">Авторы существующие в системе</param>
        /// <param name="result"></param>
        public void UpdatePA(int publicationId, IEnumerable<Author> createdAuthors, IEnumerable<Author> selectedAuthors)
        {
            paRepository.Create(createdAuthors, publicationId);

            if (selectedAuthors.Count() != 0)
            {
                var pas = repository.GetQuery<PA>(i => i.PublicationId == publicationId).ToList();
                foreach (var item in selectedAuthors)
                {
                    var pa = pas.FirstOrDefault(i => i.PublicationId == publicationId && i.MultipleId == item.Id);
                    if (pa == null)
                    {
                        pa = paRepository.Create(item, publicationId);
                    }
                    else
                    {
                        pa.Weight = item.Weight;
                        repository.Update(pa);
                    }
                    pas.Remove(pa);
                }

                if (pas.Count != 0)
                {
                    repository.DeleteRange(pas);
                }
            }

        }

        /// <summary>
        /// Создание записи в РА
        /// </summary>
        /// <param name="publicationId"></param>
        /// <param name="createdAuthors">Авторы не существующие в системе</param>
        /// <param name="selectedAuthors">Авторы существующие в системе</param>
        /// <param name="result"></param>
        public void CreatePA(int publicationId, IEnumerable<Author> createdAuthors, IEnumerable<Author> selectedAuthors)
        {
            var listAuthors = new List<Author>();
            listAuthors.AddRange(createdAuthors);
            listAuthors.AddRange(selectedAuthors);
            paRepository.Create(listAuthors, publicationId);
        }

        public void CreatePA(int publicationId, IEnumerable<Author> authors)
        {
            paRepository.Create(authors, publicationId);
        }
        #endregion
    }
}
