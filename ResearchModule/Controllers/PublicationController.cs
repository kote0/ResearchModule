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

        public PublicationController(FileManager fileManager, PAManager paManager) 
        {
            this.fileManager = fileManager;
            this.paManager = paManager;
        }

        public IActionResult Index()
        {
            return View();
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
                if (Publication.PublicationForm.Equals(PublicationForm.Forms.electronic_source)|| 
                    Publication.PublicationForm.Equals(PublicationForm.Forms.audiovisual))
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
                CreateOrUpdate_Publication(Publication),
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

        private long CreatePublicationType(PublicationType publicationType)
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
            return publicationType.Id;
        }

        private long CreateOrUpdate_Publication(Publication publication)
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

        private void CreatePA(long publicationId, IEnumerable<Author> createdAuthors, IEnumerable<Author> selectedAuthors)
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


        // TODO: исправить на страницы
        public ActionResult Publications(int first, int seconde)
        {
            
            
            //t.Authors = 
            var e  = manager.Page<Publication>(first, seconde);
            var t = new PublicationsViewModel(e);
            return View();
        }

        public ActionResult Edit(long id)
        {
            return View(manager.Get<Publication>(id));
        }

    }
}