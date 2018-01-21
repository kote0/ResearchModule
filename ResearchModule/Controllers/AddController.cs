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
        [HttpGet]
        public IActionResult Add()
        {
            return View();
        }

        // TODO: Изменить входные параметры CreatePublication или нет?
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
            return PartialView(manager.GetByFunction<Publication>(p=>p.IsValid()).ToList());
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

        #region select list

        private ResearchModule.Models.SelectList selectListCreate(List<ResearchModule.Models.SelectListItem> list, string name)
        {
            var selectList = new ResearchModule.Models.SelectList();
            if (list.Count != 0)
                selectList.AddRange(list);

            selectList.SetName(name);
            return selectList;
        }

        public PartialViewResult LoadSelectAuthor()
        {
            var list = manager.GetByFunction<Author>(a => a.IsValid()) //сделать асинхронным
                .Select(a =>
                    new ResearchModule.Models.SelectListItem
                    {
                        Value = a.Id,
                        Text = string.Format("{0}.{1}. {2}", a.Surname.Substring(0, 1), a.LastName.Substring(0, 1), a.Name)
                    })
                .ToList();

            return PartialView("Components/SelectList", selectListCreate(list, "Author"));
        }


        public PartialViewResult LoadSelectSection()
        {
            var list = manager.GetByFunction<Section>(a => a.IsValid())
                .Select(a =>
                    new ResearchModule.Models.SelectListItem
                    {
                        Value = a.Id,
                        Text = a.SectionName
                    })
                .ToList();

            return PartialView("Components/SelectList", selectListCreate(list, "Section"));
        }

        public PartialViewResult LoadSelectTypePublication()
        {
            var list = manager.GetByFunction<TypePublication>(a => a.IsValid())
                .Select(a =>
                    new ResearchModule.Models.SelectListItem
                    {
                        Value = a.Id,
                        Text = a.TypePublicationName
                    })
                .ToList();

            return PartialView("Components/SelectList", selectListCreate(list, "TypePublicationId"));
        }

        public PartialViewResult LoadSelectFormWork()
        {
            var list = manager.GetByFunction<FormWork>(a => a.IsValid())
                .Select(a =>
                    new ResearchModule.Models.SelectListItem
                    {
                        Value = a.Id,
                        Text = string.Format("{0}({1})", a.FormName, a.ShortName)
                    })
                .ToList();

            var selectList = new ResearchModule.Models.SelectList();
            selectList.SetName("FormWorkId");
            selectList.AddRange(list);

            return PartialView("Components/SelectList", selectListCreate(list, "FormWorkId"));
        }

        #endregion
    }
}