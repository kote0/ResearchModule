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
        public IActionResult CreatePublication(FormWork formWork)
        {
            //var json = JsonConvert.DeserializeObject(data);
            return View("Test/Index");
        }

        public JsonResult GetType(string character)
        {
            if (character == null)
                return Json(null);
            var types = TPM.GetByFunction(a => a.TypePublicationName != null && a.TypePublicationName.ToLower().Contains(character)).ToList();
            if (types.Count() == 0)
                return Json(null);

            var list = TPM.DropbdownList(types).ToString();
            //if (types.Count() == 0)
            //    types.Add(new TypePublication { TypePublicationName = "Нет данных для отображения" });
            //var list = new SelectList(types).ToString()
            return Json(new { Data = list });
        }
        
        public PartialViewResult AddAuthor(long authorPrefix)
        {
            return PartialView(authorPrefix);
        }
        public PartialViewResult GetAuthors(string character)
        {
            if (character == null)
                // return Json(null);
                return PartialView("Authors");
             //проверить есть ли данные в Authors
             var authors = AM.GetByFunction(a => (a.LastName != null && a.LastName.ToLower().Contains(character))
                || (a.Surname != null && a.Surname.ToLower().Contains(character))
                || (a.Name != null && a.Name.ToLower().Contains(character))).ToList();
            if (authors.Count() == 0)
                return PartialView("Authors");
                 // return Json(null);
             ViewBag.Authors = authors;
            
            /*var list = new List<Models.SelectListItem>();
            foreach (var author in authors)
            {
                list.Add(new Models.SelectListItem
                {
                    Value = author.LastName,
                    Text = author.Name
                });
            }
            var selectlist = new Models.SelectList
            {
                Elements = list
            };*/
            return PartialView("Components/SelectList", null);
           // string list = null;//AM.DropbdownList(authors).ToString();
            //return Json(new { Data = list });
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