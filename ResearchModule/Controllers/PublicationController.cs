using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using ResearchModule.Components.Models;
using ResearchModule.Components.Models.Interfaces;
using ResearchModule.Models;
using ResearchModule.Repository.Interfaces;
using ResearchModule.Service;
using ResearchModule.ViewModels;
using System.Collections.Generic;
using ResearchModule.Extensions;
using System.Linq;

namespace ResearchModule.Controllers
{
    //RedirectToAction("Test", "FullTextSearch", new { area = "EleWise.ELMA.BPM.Web.Common" });
    [Authorize]
    public class PublicationController : BaseController
    {
        private const string PublicationName = "Publication";

        private readonly PublicationService publicationService;
        private readonly SelectListService selectListService;

        public PublicationController(PublicationService publicationService, SelectListService selectListService) 
        {
            this.publicationService = publicationService;
            this.selectListService = selectListService;
        }

        [HttpGet]
        public IActionResult CreatePublicationNew()
        {
            var model = new CreatePublicationViewModel();
            model.PublicationTypes = selectListService.Create<PublicationType>(null);
            
            return View("CreatePublication", model);
        }

        [HttpPost]
        public IActionResult CreatePublicationNew(CreatePublicationViewModel createPublication)
        {
            createPublication.PublicationTypes = selectListService
                .Create<PublicationType>(createPublication.Publication.PublicationTypeId);

            return View("CreatePublication", createPublication);
        }

        [HttpPost]
        public IActionResult Filter(PublicationFilterViewModel viewModel, int first = 1)
        {
            var publications = publicationService.Filter(viewModel, first, "FilterPage", PublicationName, "PublicationFilterFormId");
            return View("Publications", publications);
        }

        public PartialViewResult FilterPage(PublicationFilterViewModel viewModel, int first = 1)
        {
            var publications = publicationService.Filter(viewModel, first, "FilterPage", PublicationName, "PublicationFilterFormId");
            return PartialView("PublicationView", publications);
        }

        [HttpGet]
        public ActionResult Create()
        {
            return View();
        }

        [HttpGet]
        public ActionResult Publications(int first = 1)
        {
            var view = publicationService.Page(first, "PublicationPage", PublicationName);
            return View(view);
        }

        [HttpPost]
        public PartialViewResult PublicationPage(int first)
        {
            var view = publicationService.Page(first, "PublicationPage", PublicationName);
            return PartialView("PublicationView", view);
        }

        public ActionResult Edit(int id)
        {
            var item = publicationService.Load(id) as Publication;
            return View(item);
        }

        [HttpPost]
        public ActionResult Search(string character)
        {
            if (character == null) return RedirectToAction("Index", "Base");
            var filter = new PublicationFilterViewModel();
            filter.Publication = new Publication();
            filter.Publication.PublicationName = character;
            var view = publicationService.Filter(filter, 1, "FilterPage", PublicationName, "PublicationFilterFormId");
            return View("Publications", view);
        }

        
        [HttpPost]
        public ActionResult Create(IEnumerable<Author> Author, [Bind(Prefix ="Search")]IEnumerable<Author> Search,
            Publication Publication, PublicationType PublicationType, IFormFile FormFile, int PublicationTypeId)
        {
            IResult result = null;
            if (PublicationTypeId != 0) Publication.PublicationTypeId = PublicationTypeId;
            
            if (Publication.Id == 0)
            {
                result = publicationService.Create(Publication, PublicationType, FormFile, Author, Search);
            }
            else
            {
                result = publicationService.Update(Publication, PublicationType, FormFile, Author, Search);
            }

            if (result.Failed)
            {
                ViewData["result"] = result;
                return View();
            }

            return RedirectToAction("Publications");
        }

        public IActionResult View(int id)
        {
            var publication = publicationService.Load(id) as Publication;
            return View(publication);
        }

        public ActionResult Update(PublicationViewModel model)
        {
            return View();
        }
    }
}