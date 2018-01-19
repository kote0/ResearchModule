using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Mvc;
using ResearchModule.Managers;
using Microsoft.AspNetCore.Mvc.Rendering;
using ResearchModule.Models;
using Newtonsoft.Json;

namespace ResearchModule.Controllers
{
    public class AddController : BaseController
    {
        public IActionResult Add()
        {
            return View();
        }

        public IActionResult CreatePublication(List<Author> Author, [Bind(Prefix = "Search")]List<Author> Search,
            Publication publication, FormWork formWork, long Section, TypePublication typePublication, long TypePublicationId, long FormWorkId)
        {
            var selectedAuthors = Search.Where(s => s.Id != 0);
            var createdAuthors = Author.Where(a => a.IsValid());
            if (!(publication.IsValid() && Section != 0 && (createdAuthors.Count() > 0 || selectedAuthors.Count() > 0)))
            {
                ViewBag.Result = "Данные не введены";
                return View("Publication");
            }
            if (formWork.IsValid())
            {
                manager.Create(formWork);
                publication.FormWork = formWork;
            }
            if (typePublication.IsValid())
            {
                manager.Create(typePublication);
                publication.TypePublicationId = typePublication.Id;
            }
            if (TypePublicationId != 0)
            {
                publication.TypePublicationId = TypePublicationId;
            }
            if (FormWorkId != 0)
            {
                publication.FormWorkId = FormWorkId;
            }
            if (publication.IsValid())
            {
                publication.SectionId = Section;
                manager.Create(publication);
            }
            CreatePA(publication.Id, createdAuthors, selectedAuthors);

            return View("Publication", publication);
        }


        public PartialViewResult AddAuthor(long authorPrefix)
        {
            return PartialView(authorPrefix);
        }


        public PartialViewResult Authors(long idPublication)
        {
            return PartialView("Authors", new PAManager().FindAuthorByPublication(idPublication));
        }

        public ActionResult Publication()
        {
            return PartialView(manager.GetAll<Publication>());
        }

        #region privateMembers

        /// <summary>
        /// Создание PA
        /// </summary>
        /// <param name="publicationId"></param>
        /// <param name="createdAuthors"></param>
        /// <param name="selectedAuthors"></param>
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