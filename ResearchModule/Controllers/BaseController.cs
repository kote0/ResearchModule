using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Mvc;
using ResearchModule.Managers;

namespace ResearchModule.Controllers
{
    public class BaseController : Controller
    {
        public BaseManager manager = new BaseManager();
    }
}