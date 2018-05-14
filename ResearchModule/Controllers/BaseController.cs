using Microsoft.AspNetCore.Mvc;
using ResearchModule.Repository;
using ResearchModule.Repository.Interfaces;

namespace ResearchModule.Controllers
{
    public class BaseController : Controller
    {

        public IActionResult Index()
        {
            return View();
        }

        public IActionResult SearchUsers()
        {
            return View();
        }
    }
}