using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Mvc;
using ResearchModule.Managers;
using ResearchModule.Models;

namespace ResearchModule.Controllers
{
    public class TestController : BaseController
    {
        public IActionResult Index()
        {
            return View("Index");//RedirectToAction("Test", "FullTextSearch", new { area = "EleWise.ELMA.BPM.Web.Common" });
        }

        public IActionResult List()
        {
            
            var list = manager.GetByFunction<Author>(a => a.IsValid())
                .Select(a =>
                    new SelectListItem
                    {
                        Value = a.Id,
                        Text = string.Format("{0}.{1}. {2}", a.Surname.Substring(0, 1), a.LastName.Substring(0, 1), a.Name)
                    })
                .ToList();

            var selectList = new SelectList();
            selectList.SetName("Author");
            selectList.AddRange(list);
            return View("Components/SelectList", selectList);
        }

    }
}