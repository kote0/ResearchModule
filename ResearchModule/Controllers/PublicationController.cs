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
using System;
using Microsoft.AspNetCore.Http.Internal;
using ResearchModule.Managers.Interfaces;
using System.IO;
using System.Threading.Tasks;

namespace ResearchModule.Controllers
{
    //RedirectToAction("Test", "FullTextSearch", new { area = "EleWise.ELMA.BPM.Web.Common" });
    [Authorize]
    public class PublicationController : BaseController
    {
        private const string PublicationControllerName = "Publication";

        private readonly PublicationService publicationService;
        private readonly SelectListService selectListService;
        private readonly IFileManager fileManager;

        public PublicationController(PublicationService publicationService, SelectListService selectListService, IFileManager fileManager) 
        {
            this.publicationService = publicationService;
            this.selectListService = selectListService;
            this.fileManager = fileManager;
        }

        [HttpGet]
        public IActionResult CreatePublicationNew()
        {
            var model = new CreatePublicationViewModel();
            model.PublicationTypes = selectListService.Create<PublicationType>(0);
            model.PublicationPartions = selectListService.Create(PublicationElems.GetPartions(), PublicationElems.scientific.Id);
            model.PublicationForms = selectListService.Create(PublicationElems.GetForms(), PublicationElems.electronicSource.Id);
            model.PublicationFilters = selectListService.Create<PublicationFilters>(0);

            return View("CreatePublication", model);
        }

        private bool ValidFile(int publicationFileId, IFormFile file)
        {
            return publicationFileId == 0 && file != null
                || publicationFileId != 0 && file != null
                || publicationFileId != 0 && file == null;
        }

        private bool Valid(CreatePublicationViewModel createPublication)
        {
            var publicationFileId = createPublication.Publication.PublicationFileId;
            var file = createPublication.FormFile;
            var validFile = publicationFileId == 0 && file != null
                || publicationFileId != 0 && file != null
                || publicationFileId != 0 && file == null;

            var typeId = createPublication.Publication.PublicationTypeId;
            var typeName = string.IsNullOrEmpty(createPublication.PublicationTypeName);

            var validType = typeId == 0 && !typeName || typeId != 0;

            return validFile && validType;
        }

        [HttpPost]
        public IActionResult CreatePublicationNew(CreatePublicationViewModel createPublication)
        {
            if (ModelState.IsValid || (!ModelState.IsValid && Valid(createPublication)))
            {
                var res = publicationService.Create(createPublication);
                createPublication = res.Model as CreatePublicationViewModel;
                if (res.Failed)
                {
                    ViewData["result"] = res;
                }
                else
                {
                    return RedirectToAction("View", new { id = createPublication.Publication.Id });
                }
            }

            createPublication.PublicationTypes = selectListService
                .Create<PublicationType>(createPublication.Publication.PublicationTypeId);
            createPublication.PublicationPartions = selectListService
                .Create(PublicationElems.GetPartions(), createPublication.Publication.PublicationPartition);
            createPublication.PublicationForms = selectListService
                .Create(PublicationElems.GetForms(), createPublication.Publication.PublicationForm);
            createPublication.PublicationFilters = selectListService
                .Create<PublicationFilters>(createPublication.PublicationFiltersId);

            return View("CreatePublication", createPublication);
        }

        [HttpPost]
        public IActionResult Filter(PublicationFilterViewModel viewModel, int first = 1)
        {
            ViewData["types"] = selectListService.Create<PublicationType>(viewModel.PublicationTypesId);
            return View("Publications", GetViewModel(viewModel, first));
        }

        public IActionResult Delete(int id)
        {
            publicationService.Delete(id);
            return RedirectToAction("Publications");
        }

        public PartialViewResult FilterPage(PublicationFilterViewModel viewModel, int first = 1)
        {
            ViewData["types"] = selectListService.Create<PublicationType>(viewModel.PublicationTypesId);
            return PartialView("PublicationView", GetViewModel(viewModel, first));
        }

        private PublicationsViewModel GetViewModel(PublicationFilterViewModel viewModel, int first)
        {
            var publications = publicationService.Filter(viewModel, first, "FilterPage", PublicationControllerName, "PublicationFilterFormId");
            if (publications.PublicationFilter == null)
            {
                publications.PublicationFilter = new PublicationFilterViewModel();
            }
            publications.PublicationFilter.AuthorList = selectListService.Create<Author>(viewModel.Authors);
            return publications;
        }

        [HttpGet]
        public ActionResult Create()
        {
            return View();
        }

        [HttpGet]
        public ActionResult Publications(int first = 1)
        {
            var view = publicationService.Page(first, "PublicationPage", PublicationControllerName);
            if (view.PublicationFilter == null)
            {
                view.PublicationFilter = new PublicationFilterViewModel();
            }
            ViewData["types"] = selectListService.Create<PublicationType>(0);
            view.PublicationFilter.AuthorList = selectListService.Create<Author>(0);
            return View(view);
        }

        [HttpPost]
        public PartialViewResult PublicationPage(int first)
        {
            var view = publicationService.Page(first, "PublicationPage", PublicationControllerName);
            return PartialView("PublicationView", view);
        }

        public ActionResult Edit(int id)
        {
            var item = publicationService.Load(id) as Publication;

            var createPublication = new CreatePublicationViewModel();
            createPublication.Publication = item;
            createPublication.Authors = item.PAs.Select(a => a.Multiple);
            createPublication.OldFileName = item.PublicationFile.Uid;

            createPublication.PublicationTypes = selectListService
                .Create<PublicationType>(item.PublicationTypeId);
            createPublication.PublicationPartions = selectListService
                .Create(PublicationElems.GetPartions(), item.PublicationPartition);
            createPublication.PublicationForms = selectListService
                .Create(PublicationElems.GetForms(), item.PublicationForm);
            createPublication.PublicationFilters = selectListService
                .Create<PublicationFilters>(item.PFs.Select(f => f.MultipleId));

            return View("CreatePublication", createPublication);
        }

        [HttpPost]
        public ActionResult Search(string character)
        {
            if (character == null) return RedirectToAction("Index", "Base");
            var filter = new PublicationFilterViewModel();
            filter.Publication = new Publication();
            filter.Publication.PublicationName = character;
            var view = publicationService.Filter(filter, 1, "FilterPage", PublicationControllerName, "PublicationFilterFormId");
            return View("Publications", view);
        }

        public IActionResult View(int id)
        {
            if (id != 0)
            {
                var publication = publicationService.Load(id) as Publication;
                return View(publication);
            }
            return RedirectToAction("Publications");
        }

        public ActionResult Update(PublicationViewModel model)
        {
            return View();
        }

        public async Task<IActionResult> Download(string uid, string name)
        {
            var type = fileManager.GetContentType(name);
            var path = fileManager.Download(uid);
            var memory = new MemoryStream();
            using (var stream = new FileStream(path, FileMode.Open))
            {
                await stream.CopyToAsync(memory);
            }
            memory.Position = 0;
            return File(memory, type, Path.GetFileName(path));
        }
    }
}