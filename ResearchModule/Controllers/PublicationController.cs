using Microsoft.AspNetCore.Mvc;
using ResearchModule.Managers;
using ResearchModule.Models;
using System.Collections.Generic;
using System.Linq;

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

        [HttpPost]
        public ActionResult Create(IEnumerable<Author> Author, [Bind("Id", Prefix = "Search")]IEnumerable<Author> Search,
            Publication Publication, PublicationType PublicationType, string TranslateText)
        {
            var selectedAuthors = Search.Where(s => s.Id != 0);
            var createdAuthors = Author.Where(a => a.IsValid());
            if (!(!string.IsNullOrEmpty(Publication.PublicationName) && (createdAuthors.Count() > 0 || selectedAuthors.Count() > 0)))
            {
                ViewBag.Result = "Данные введены не корректно";
                return View("Publication");
            }
            if (PublicationType.IsValid())
            {
                manager.Create(PublicationType);
                Publication.PublicationType = PublicationType.Id;
            }
            if(!string.IsNullOrEmpty(TranslateText))
            {
                Publication.TranslateText = TranslateText;
            }
            if (Publication.Id != 0)
            {
                manager.Update<Publication>(Publication);
            }
            else
            {
                manager.Create(Publication);
            }
            CreatePA(Publication.Id, createdAuthors, selectedAuthors);
            var list = new List<Publication>();
            list.Add(Publication);
            return View("Publications", list);
        }

        // TODO: исправить на страницы
        public ActionResult Publications()
        {
            return View(manager.GetAll<Publication>());
        }

        public ActionResult Edit(long id)
        {
            return View("Create", manager.Get<Publication>(id));
        }

        #region privateMembers

        /// <summary>
        /// Создание PA
        /// </summary>
        private void CreatePA(long publicationId, IEnumerable<Author> createdAuthors, IEnumerable<Author> selectedAuthors)
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

            //добавить запись PA
            mngPA.Create(listAuthors, publicationId);
        }


        #endregion
    }
}