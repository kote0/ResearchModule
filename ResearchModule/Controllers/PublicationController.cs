using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using Microsoft.Net.Http.Headers;
using ResearchModule.Components.Models;
using ResearchModule.Managers;
using ResearchModule.Models;
using ResearchModule.Service;
using ResearchModule.ViewModels;
using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Threading.Tasks;

namespace ResearchModule.Controllers
{
    //RedirectToAction("Test", "FullTextSearch", new { area = "EleWise.ELMA.BPM.Web.Common" });
    public class PublicationController : BaseController
    {
        private readonly FileManager fileManager;
        private readonly PAManager paManager;
        private readonly PublicationService publicationService;

        public PublicationController(FileManager fileManager, PAManager paManager,
            BaseManager manager, PublicationService publicationService) : base(manager)
        {
            this.fileManager = fileManager;
            this.paManager = paManager;
            this.publicationService = publicationService;
        }


        [HttpGet]
        public ActionResult Create()
        {
            return View();
        }

        #region CreatePublication

        //TODO: Заменить на CreatePublicationViewModel
        [HttpPost]
        public ActionResult Create(IEnumerable<Author> Author, [Bind("Id", Prefix = "Search")]IEnumerable<Author> Search,
            Publication Publication, PublicationType PublicationType, IFormFile FormFile)
        {
            var selectedAuthors = Search.Where(s => s.Id != 0);
            var createdAuthors = Author.Where(a => a.IsValid());
            if ((createdAuthors.Count() == 0 && selectedAuthors.Count() == 0))
            {
                Notify.SetInfo("Отсутсутствуют авторы");
                return View();
            }

            var file = (Publication.PublicationFileUid == null) ? CreateFile(FormFile) : null;
            if (file != null)
            {
                Publication.PublicationFileName = file.Name;
                Publication.PublicationFileUid = file.Uid;
                // электронное или аудиальное
                if (Publication.PublicationForm.Equals((int)PublicationElements.FormEnum.electronic_source)|| 
                    Publication.PublicationForm.Equals((int)PublicationElements.FormEnum.audiovisual))
                {
                    Publication.Volume = file.Size;
                }
            }

            if (!string.IsNullOrEmpty(PublicationType.Name))
            {
                Publication.PublicationType = CreatePublicationType(PublicationType);
            }

            CreatePA(
                CreateOrUpdatePublication(Publication),
                createdAuthors, selectedAuthors);
            
            return RedirectToAction("Publications");
        }


        private FileDetails CreateFile(IFormFile file)
        {
            var source = fileManager.CreateFileDetails(file);
            if (source != null)
                fileManager.SaveFile(source);
            return source;
        }

        private PublicationType CreatePublicationType(PublicationType publicationType)
        {
            try
            {
                if (publicationType.IsValid())
                {
                    manager.Create(publicationType);
                }
                return publicationType;
            }
            catch (Exception ex)
            {
                Notify.SetError("Не удалось создать Вид публикации", ex.Message);
                throw;
            }
        }

        private int CreateOrUpdatePublication(Publication publication)
        {
            try
            {
                if (publication.Id != 0)
                {
                    publication.ModifyDate = DateTime.Now;
                    manager.Update<Publication>(publication);
                }
                else
                {
                    publication.CreateDate = DateTime.Now;
                    manager.Create(publication);
                }
                return publication.Id;
            }
            catch (Exception ex)
            {
                Notify.SetError("Не удалось создать/изменить Публикацию", ex.Message);
                throw;
            }
        }

        private void CreatePA(int publicationId, IEnumerable<Author> createdAuthors, IEnumerable<Author> selectedAuthors)
        {
            //собрать найденных и созданных
            var listAuthors = new List<Author>();

            //создать авторов 
            if (createdAuthors.Count() > 0)
            {
                foreach (var item in createdAuthors)
                {
                    manager.Create(item);
                }
                listAuthors.AddRange(createdAuthors);
            }

            //использовать найденные
            if (selectedAuthors.Count() > 0)
            {
                listAuthors.AddRange(selectedAuthors);
            }

            try
            {
                //добавить запись PA
                paManager.Create(listAuthors, publicationId);
            }
            catch (Exception ex)
            {
                Notify.SetError("Не удалось добавить авторов", ex.Message);
                throw;
            }
        }

        #endregion


        public ActionResult Publications(int first = 1)
        {
            var pageInfo = new PageInfo();
            pageInfo.PageNumber = first;
            var listPublications  = manager.Page<Publication>(first, pageInfo.PageSize);
            pageInfo.TotalItems = listPublications.Count();
            PublicationsViewModel model = new PublicationsViewModel(manager, publicationService);
            if (pageInfo.TotalItems != 0)
            {
                model.Create(listPublications);
                model.PageInfo = pageInfo;
            }
            return View(model);
        }

        public ActionResult Edit(int id)
        {
            return View(manager.Get<Publication>(id));
        }

    }
}