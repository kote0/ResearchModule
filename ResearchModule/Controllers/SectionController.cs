using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Mvc;
using ResearchModule.Models;
using ResearchModule.Managers;

namespace ResearchModule.Controllers
{
    public class SectionController : Controller
    {
        PublicationManager PM = new PublicationManager();
        SectionManager SM = new SectionManager();
        FormWorkManager FM = new FormWorkManager();
        AuthorManager AM = new AuthorManager();

        public IActionResult Index()
        {
            return View(SM.GetAll());
        }
        public IActionResult Add(long id)
        {
            return View(SM.Get(id));
        }
        [HttpPost]
        public IActionResult AddPublication(Section section, Author author, Publication publication, FormWork formWork,
            TypePublication typePublication)
        {
            
            if (!(author.IsValid() && section.IsValid() && publication.IsValid()
                && formWork.IsValid() && typePublication.IsValid()))
                ViewBag.Message = "Заполните все поля.";
            
            return RedirectToAction("Add");
        }

    }
}