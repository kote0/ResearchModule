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

        public PublicationController(FileManager fileManager, PAManager paManager, BaseManager manager) : base(manager)
        {
            this.fileManager = fileManager;
            this.paManager = paManager;
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
            Publication Publication, PublicationType PublicationType, IFormFile FormFile, CreatePublicationViewModel createPublication)
        {
            var selectedAuthors = Search.Where(s => s.Id != 0);
            var createdAuthors = Author.Where(a => a.IsValid());
            if ((createdAuthors.Count() > 0 || selectedAuthors.Count() > 0))
            {
                ViewBag.Result = "Отсутсутствуют авторы";
                return View("Publications");
            }
            if (!ModelState.IsValid)
            {
                return View("Publications");
            }

            var file = (Publication.PublicationFileUid == null) ? CreateFile(FormFile) : null;
            if (file != null)
            {
                Publication.PublicationFileName = file.Name;
                Publication.PublicationFileUid = file.Uid;
                // электронное или аудиальное
                if (Publication.PublicationForm.Equals(PublicationElements.FormEnum.electronic_source)|| 
                    Publication.PublicationForm.Equals(PublicationElements.FormEnum.audiovisual))
                {
                    Publication.Volume = file.Size;
                }
                else
                {
                    // TODO: Исправить
                    Publication.Volume = 0;
                }
            }

            if (!string.IsNullOrEmpty(PublicationType.Name))
            {
                Publication.PublicationType = CreatePublicationType(PublicationType);
            }

            CreatePA(
                CreateOrUpdatePublication(Publication),
                createdAuthors, selectedAuthors);

            var list = new List<Publication>();
            list.Add(Publication);
            return View("Publications", list);
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
            }
            catch (Exception ex)
            {
                throw new Exception("Не удалось создать Вид публикации", ex);
            }
            return publicationType;
        }

        private int CreateOrUpdatePublication(Publication publication)
        {
            try
            {
                publication.CreateDate = DateTime.Now;
                if (publication.Id != 0)
                {
                    manager.Update<Publication>(publication);
                }
                else
                {
                    manager.Create(publication);
                }
            }
            catch (Exception ex)
            {
                throw new Exception("Не удалось создать/изменить Публикацию", ex);
            }
            return publication.Id;
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
                throw new Exception("Не удалось добавить авторов", ex);
            }
        }

        #endregion


        public ActionResult Publications(int first = 1)
        {
            var pageInfo = new PageInfo();
            pageInfo.PageNumber = first;
            var listPublications  = manager.Page<Publication>(first, pageInfo.PageSize);
            pageInfo.TotalItems = listPublications.Count();
            PublicationsViewModel model;
            if (pageInfo.TotalItems != 0)
            {
                model = new PublicationsViewModel(listPublications) { PageInfo = pageInfo };
            }
            else
            {
                model = new PublicationsViewModel();
            }
            return View(model);
        }

        public ActionResult Edit(int id)
        {
            return View(manager.Get<Publication>(id));
        }

    }
}