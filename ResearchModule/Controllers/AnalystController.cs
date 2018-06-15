using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Mvc;
using ResearchModule.Components.Models;
using ResearchModule.Service;
using ResearchModule.ViewModels;
using ResearchModule.Models;

namespace ResearchModule.Controllers
{
    public class AnalystController : Controller
    {
        private readonly ChartService chartService;
        private readonly SelectListService selectListService;

        public AnalystController(ChartService chartService, SelectListService selectListService)
        {
            this.selectListService = selectListService;
            this.chartService = chartService;
        }

        public IActionResult Index()
        {
            ViewData["types"] = selectListService.Create<PublicationType>(0);
            var model = new PublicationFilterViewModel();
            model.Publication.CreateDate = DateTime.Now;
            return View(model);
        }

        public IActionResult PublicationChart(PublicationFilterViewModel model)
        {
            var dateTime = model.Publication.CreateDate;
            ViewData["DateTimeNow"] = dateTime;

            return View(chartService.PublicationCount(dateTime));
        }
    }
}