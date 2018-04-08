using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using Microsoft.Net.Http.Headers;
using ResearchModule.Components.Models;
using ResearchModule.Managers;
using ResearchModule.Managers.Interfaces;
using ResearchModule.Models;
using ResearchModule.Models.Filter;
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
            PublicationService publicationService, IBaseManager manager) : base(manager)
        {
            this.fileManager = fileManager;
            this.paManager = paManager;
            this.publicationService = publicationService;
            //this.Manager = manager;
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
            Publication Publication, PublicationType PublicationType, IFormFile FormFile, CreatePublicationViewModel view = null)
        {
            var selectedAuthors = Search.Where(s => s.Id != 0);
            var createdAuthors = Author.Where(a => a.IsValid());
            // проверка на наличие авторов
            if ((createdAuthors.Count() == 0 && selectedAuthors.Count() == 0))
            {
                Notify.SetInfo("Отсутсутствуют авторы");
                return View();
            }

            var file = (Publication.PublicationFileUid == null) 
                ? fileManager.CreateFileDetails(FormFile) 
                : null;

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
        

        private PublicationType CreatePublicationType(PublicationType publicationType)
        {
            try
            {
                if (publicationType.IsValid())
                {
                    Manager.Create(publicationType);
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
                    Manager.Update<Publication>(publication);
                }
                else
                {
                    publication.CreateDate = DateTime.Now;
                    Manager.Create(publication);
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
                    Manager.Create(item);
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
            var publications = Manager.Page<Publication>(first);
            return RedirectToAction("Publications", new { list = publications, first = first });
        }

        public ActionResult Publications(IEnumerable<Publication> list, int first = 1)
        {
            var pageInfo = new PageInfo(first, list.Count());
            PublicationsViewModel model = new PublicationsViewModel
                (publicationService, list, pageInfo);
            return View(model);
        }

        public ActionResult Edit(int id)
        {
            var item = Manager.Get<Publication>(id);
            return View(item);
        }


        public ActionResult Search(string character)
        {
            if (character == null) return null;
            var text = character.ToLower();

            var publications = Manager.GetByFunction<Publication>(a => {
                if (!string.IsNullOrEmpty(a.PublicationName))
                {
                    return a.PublicationName.ToLower().Contains(text);
                }
                else return false;
            });
            //return RedirectToAction("Publications", new { list = publications });
            return PartialView(publications.ToList());
        }
    }
}