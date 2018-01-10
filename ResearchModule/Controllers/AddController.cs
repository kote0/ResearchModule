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
    public class AddController : Controller
    {
        
        TypePublicationManager TPM = new TypePublicationManager();
        AuthorManager AM = new AuthorManager();

        public IActionResult Add()
        {
            return View();
        }

        //public IActionResult CreatePublication(string data, string author1, string author2)
        public IActionResult CreatePublication(List<Author> Author, [Bind(Prefix = "Search")]List<Author> Search,
            Publication publication, FormWork formWork, long Section, TypePublication typePublication, long TypePublicationId, long FormWorkId)
        {
            var selectedAuthors = Search.Where(s => s.Id != 0);
            var createdAuthors = Author.Where(a => a.IsValid());
            if (!(publication.IsValid()&& Section !=0 && (createdAuthors.Count() > 0 || selectedAuthors.Count() > 0))){
                ViewBag.Result = "Данные не введены";
                return View("Publication");
            }
            if (formWork.IsValid())
            {
                using (BaseManager<FormWork> mngFW = new BaseManager<FormWork>())
                {
                    mngFW.Create(formWork);
                }
                publication.FormWork = formWork;
            }
            if (typePublication.IsValid())
            {
                using (BaseManager<TypePublication> mngTP = new BaseManager<TypePublication>())
                {
                    mngTP.Create(typePublication);
                }
                publication.TypePublicationId = typePublication.Id;
            }
            if (TypePublicationId != 0)
            {
                publication.TypePublicationId = TypePublicationId;
            }
            if (FormWorkId!= 0)
            {
                publication.FormWorkId = FormWorkId;
            }
            if (publication.IsValid())
            {
                publication.SectionId = Section;
                using (BaseManager<Publication> mngPublication = new BaseManager<Publication>())
                {

                    mngPublication.Create(publication);
                }
            }

            using (PAManager mngPA = new PAManager())
            {
                //создать авторов 
                if (createdAuthors.Count() > 0)
                {
                    var mngAuthor = new BaseManager<Author>();
                    foreach (var item in createdAuthors)
                    {
                        mngAuthor.Create(item);
                    }
                    mngPA.Create(createdAuthors, publication.Id);
                }

                //использовать найденные
                if (selectedAuthors.Count() > 0)
                {
                    mngPA.Create(selectedAuthors, publication.Id);
                }
            }


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
            return PartialView(new BaseManager<Publication>().GetAll());
        }

    }
}