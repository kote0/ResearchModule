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
        private readonly PublicationManager manager;
        private PublicationPage publicationPage;

        public PublicationService( PublicationPage publicationPage,
            PublicationManager manager,
            SelectListService selectListService)
        {
            this.manager = manager;
            this.publicationPage = publicationPage;
        }

        public void Delete(int id)
        {
            manager.Delete(id);
            manager.Save();
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
            return manager.Load(id);
        }

        public IResult Create(CreatePublicationViewModel createPublication)
        {
            var result = new Result();
            result.Model = createPublication;
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
                return result.Set(res);
            }

            var fileDetails = res.Model as FileDetail;

            var publication = createPublication.Publication;

            publication.PublicationFile = fileDetails;

            // Добавлении информации в публикацию для элек. и видео файлов
            if (!PublicationElems.IsPrintForm(publication.PublicationForm))
            {
                publication.Volume = fileDetails.Size;
            }

            publication.PublicationFileId = fileDetails.Id;

            if (!string.IsNullOrEmpty(createPublication.PublicationTypeName))
            {
                var type = new PublicationType() { Name = createPublication.PublicationTypeName };
                publication.PublicationType = type;
                publication.PublicationTypeId = type.Id;
            }

            publication.PAs = manager.AppendPA(publication, createdAuthors, selectedAuthors);            

            manager.CreateOrUpdatePublication(publication);
            //manager.SQL("SET IDENTITY_INSERT dbo.Author ON");
            manager.Save();


            if (result.Failed) return result.Set("Не удалось добавить авторов");

            return result;
        }

    }
}
