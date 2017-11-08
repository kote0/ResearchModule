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
            var types = TPM.GetAll().Select(m=>m.TypePublicationName);
            if (types.Count() == 0)
                return PartialView(null);
            return PartialView(new SelectList(types));
        }
        public PartialViewResult AddAuthor()
        {
            var types = AM.GetAll();
            if (types.Count() == 0)
                return PartialView(null);
            return PartialView(new SelectList(types));
        }

        public JsonResult GetAuthors(string character)
        {
            var authors = AM.GetAuthors(character);
            var listAutors = AM.ListAuthors(authors);
            return Json(new { authors = listAutors });
        }

        
    }
}