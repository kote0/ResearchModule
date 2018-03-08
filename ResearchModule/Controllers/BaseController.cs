﻿using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Mvc;
using ResearchModule.Managers;

namespace ResearchModule.Controllers
{
    public class BaseController : Controller
    {
        public BaseManager manager;

        public BaseController(BaseManager manager) 
        {
            this.manager = manager;
        }

        public IActionResult Index()
        {
            return View();
        }
    }
}