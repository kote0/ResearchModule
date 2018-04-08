using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Mvc;
using ResearchModule.Managers;
using System.Runtime.CompilerServices;
using ResearchModule.Managers.Interfaces;

namespace ResearchModule.Controllers
{
    public class BaseController : Controller
    {
        public IBaseManager Manager
        {
            get
            {
                return BaseManager.Manager;
            }
        }

        public BaseController(IBaseManager Manager)
        {
            //this.Manager = Manager;
        }

        public IActionResult Index()
        {
            return View();
        }
    }
}