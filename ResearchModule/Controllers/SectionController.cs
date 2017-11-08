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
        PublicationManager Pm = new PublicationManager();
        SectionManager Manager = new SectionManager();
        FormWorkManager fm = new FormWorkManager();


        public IActionResult Index()
        {
            return View(Manager.GetAll());
        }
        public IActionResult Add()
        {
            return View(fm.GetAll());
        }
        [HttpPost]
        public IActionResult Add(string formName, string shortName)
        {
            FormWork record = new FormWork()
            {
                FormName = formName,
                ShortName = shortName
            };
            fm.Create(record);
            return RedirectToAction("Add");
        }
       
    }
}