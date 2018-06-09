using Microsoft.AspNetCore.Http;
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Query;
using ResearchModule.Components.Models;
using ResearchModule.Components.Models.Interfaces;
using ResearchModule.Managers.Interfaces;
using ResearchModule.Models;
using ResearchModule.Repository;
using ResearchModule.Repository.Interfaces;
using ResearchModule.Service;
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

        public void Delete(int id)
        {
            repository.Delete<Publication>(id);
        }

        public IResult AppendFile(CreatePublicationViewModel createPublication)
        {
            var res = new Result();

            var oldFileName = createPublication.OldFileName;
            var fileId = createPublication.Publication.PublicationFileId;

            // нет записи о файле и есть файл
            if (fileId == 0 && createPublication.FormFile != null)
            {
                var file = fileManager.CreateInfo(createPublication.FormFile);
                fileManager.SaveFileInServer(file);
                createPublication.OldFileName = file.Uid;
                res.Model = file;
                return res;
            }
            // есть запись о файле
            // пришел новый файл
            if (fileId != 0 && createPublication.FormFile != null)
            {
                var oldFile = fileManager.Get(oldFileName);
                var file = fileManager.CreateInfo(createPublication.FormFile);
                oldFile.FormFile = file.FormFile;
                oldFile.Name = file.Name;
                oldFile.Size = file.Size;
                oldFile.Uid = file.Uid;
                createPublication.OldFileName = file.Uid;
                //fileManager.Delete(file.Uid);
                //fileManager.SaveFileInServer(file);
                res.Model = oldFile;
                return res;
            }
            // есть запись о файле и нет файла
            if (fileId != 0 && createPublication.FormFile == null)
            {
                res.Model = fileManager.Get(oldFileName);
                return res;
            }
            // нет записи о файле и нет самого файла
            return res.Set("Не добавлен файл");
        }

        public long Count(Expression<Func<Publication, bool>> func)
        {
            return repository.LongCount(func);
        }

        public long Count()
        {
            return repository.LongCount<Publication>();
        }

        public object Load(int id)
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
            return LoadWithAuthors().Where(p => Contains(p, filter));
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

        public void Save()
        {
            repository.Save();
        }

        private bool Contains(Publication model, PublicationFilterViewModel filter)
        {
            if (model.PAs != null)
            {
                foreach (var i in filter.Authors)
                {
                    if (model.PAs.FirstOrDefault(a => a.MultipleId == i) != null)
                    {
                        return true;
                    }
                }
            }

            if (filter.Publication != null)
            {
                var name = filter.Publication.PublicationName;
                if (!string.IsNullOrEmpty(name) && model.PublicationName.ToLower().Contains(name.ToLower()))
                    return true;

                if (!string.IsNullOrEmpty(filter.Publication.OutputData)
                    && model.OutputData.ToLower().Contains(filter.Publication.OutputData))
                    return true;
            }

            if (filter.PublicationTypesId.Count != 0)
            {
                return filter.PublicationTypesId.Count(t => t == model.PublicationTypeId) > 0;
            }
            
            return false;
        }

        #region otherTypes

        public List<PA> AppendPA(Publication publication, IEnumerable<Author> createdAuthors, IEnumerable<Author> selectedAuthors)
        {
            var list = new List<PA>();

            if (createdAuthors.Count() > 0)
            {
                list.AddRange(CreatePA(createdAuthors, publication));
            }

            if (selectedAuthors.Count() != 0)
            {
                var pas = repository.GetQuery<PA>(i => i.PublicationId == publication.Id).ToList();
                foreach (var item in selectedAuthors)
                {
                    var pa = pas.FirstOrDefault(i => i.MultipleId == item.Id);
                    if (pa == null)
                    {
                        list.Add(CreatePA(item, publication));
                    }
                    else
                    {
                        pa.Coauthor = item.Coauthor;
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
            
            return list;

        }

        private ICollection<PA> CreatePA(IEnumerable<Author> authors, Publication publication)
        {
            return authors.Select(a => CreatePA(a, publication)).ToList();
        }

        private PA CreatePA(Author author, Publication publication)
        {
            if (author.Id != 0)
            {
                return new PA()
                {
                    MultipleId = author.Id,
                    Coauthor = author.Coauthor,
                    Weight = author.Weight,
                    Publication = publication,
                    PublicationId = publication.Id
                };
            }
            return new PA()
            {
                MultipleId = author.Id,
                Multiple = author,
                Coauthor = author.Coauthor,
                Weight = author.Weight,
                Publication = publication,
                PublicationId = publication.Id
            };
        }

        public void SQL(string str)
        {
            repository.SQL(str);
        }

        public void CreatePA(IEnumerable<PA> authors)
        {
            repository.Add(authors);
        }
        #endregion

        public IIncludableQueryable<Publication, Author> LoadWithAuthors()
        {
            return repository.Include<Publication, ICollection<PA>>(a => a.PAs)
                .ThenInclude(p => p.Multiple);
        }
    }
}
