using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Mvc;
using ResearchModule.Managers;
using Microsoft.AspNetCore.Mvc.Rendering;
using ResearchModule.Models;

namespace ResearchModule.Controllers
{
    public class AddController : Controller
    {
        
        TypePublicationManager TPM = new TypePublicationManager();
        AuthorManager AM = new AuthorManager();

        public PartialViewResult AddType()
        {
            return PartialView(TPM.AllTypePublication());
        }
        public JsonResult GetType(string character)
        {
            if (character == null)
                return Json(null);
            var types = TPM.GetByFunction(a => a.TypePublicationName.ToLower().Contains(character));
            if (types.Count() == 0)
                return Json(null);
            var list = TPM.DropbdownList(types);
            return Json(new { list });
        }
        
        public PartialViewResult AddAuthor()
        {
            return PartialView();
        }
        public JsonResult GetAuthors(string character)
        {
            if (character == null)
                return Json(null);
            //проверить есть ли данные в Authors
            var authors = AM.GetByFunction(a => a.LastName.ToLower().Contains(character)
                || a.Surname.ToLower().Contains(character)
                || a.Name.ToLower().Contains(character));
            if (authors.Count() == 0)
                return Json(null);
            var list = AM.DropbdownList(authors);
            return Json(new { list });
        }


    }
}