using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using Microsoft.Net.Http.Headers;
using ResearchModule.Components.Models;
using ResearchModule.Managers;
using ResearchModule.Models;
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

        [HttpPost]
        public ActionResult Create(IEnumerable<Author> Author, [Bind("Id", Prefix = "Search")]IEnumerable<Author> Search,
            Publication Publication, PublicationType PublicationType, IFormFile FormFile)
        {
            var selectedAuthors = Search.Where(s => s.Id != 0);
            var createdAuthors = Author.Where(a => a.IsValid());
            if (!(!string.IsNullOrEmpty(Publication.PublicationName) && (createdAuthors.Count() > 0 || selectedAuthors.Count() > 0)))
            {
                ViewBag.Result = "Данные введены не корректно";
                return View("Publication");
            }
            
            var file = (Publication.PublicationFileUid == null)
                ? Create_FileDetails(FormFile)
                : null;
            if (file != null)
            {
                Publication.PublicationFileName = file.Name;
                Publication.PublicationFileUid = file.Uid;
                // электронное или аудиальное
                if (Publication.PublicationForm == 2 || Publication.PublicationForm == 4)
                {
                    Publication.Volume = file.Size;
                }
                else
                {
                    // TODO: Исправить
                    Publication.Volume = 0;
                }
            }

            if (!string.IsNullOrEmpty(PublicationType.Name)) {
                Publication.PublicationType = Create_PublicationType(PublicationType);
            }

            Create_PA(
                CreateOrUpdate_Publication(Publication), 
                createdAuthors, selectedAuthors);

            var list = new List<Publication>();
            list.Add(Publication);
            return View("Publications", list);
        }


        private FileDetails Create_FileDetails(IFormFile file)
        {
            if (file == null) return null;
            FileDetails fileDetails = new FileDetails() {
                Uid = Guid.NewGuid().ToString("N"), 
                Size = file.Length,
                Name = file.FileName,
                FormFile = file
            };
            SaveFile(fileDetails);
            return fileDetails;
        }

        /// <summary>
        /// Сохранение файла в директории Files
        /// </summary>
        private void SaveFile(FileDetails fileDetails)
        {
            var path = Path.Combine(Directory.GetCurrentDirectory(), "Files");
            // Проверка на существование директории Files
            if (!Directory.Exists(path))
            {
                Directory.CreateDirectory(path);
            }
            using (var stream = new FileStream(Path.Combine(path, fileDetails.Uid), FileMode.Create))
            {
                fileDetails.FormFile.CopyTo(stream);
            }
        }

        private long Create_PublicationType(PublicationType publicationType)
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

        private void Create_PA(long publicationId, IEnumerable<Author> createdAuthors, IEnumerable<Author> selectedAuthors)
        {
            var mngPA = new PAManager();

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
                mngPA.Create(listAuthors, publicationId);
            }
            catch (Exception ex)
            {
                throw new Exception("Не удалось добавить авторов", ex);
            }
        }

        #endregion


        // TODO: исправить на страницы
        public ActionResult Publications()
        {
            return View(manager.GetAll<Publication>());
        }

        public ActionResult Edit(long id)
        {
            return View(manager.Get<Publication>(id));
        }

    }
}