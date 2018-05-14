using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Mvc;
using ResearchModule.Components.Models;
using ResearchModule.Service;
using ResearchModule.ViewModels;

namespace ResearchModule.Controllers
{
    public class AnalystController : Controller
    {
        private readonly ChartService chartService;

        public AnalystController(ChartService chartService)
        {
            this.chartService = chartService;
        }

        public IActionResult Index()
        {
            return View(new PublicationFilterViewModel());
        }

        public IActionResult PublicationChart(DateTime? date)
        {
            DateTime dateTime;
            if (date.HasValue)
            {
                dateTime = date.Value;
            }
            else
            {
                dateTime = DateTime.Now;
            }
            ViewData["DateTimeNow"] = dateTime;
            return View(chartService.PublicationCount(dateTime));
        }
    }
}