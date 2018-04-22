﻿using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using ResearchModule.Components.Models;
using ResearchModule.Components.Models.Interfaces;
using ResearchModule.Models;
using ResearchModule.Service;
using ResearchModule.ViewModels;
using System.Collections.Generic;

namespace ResearchModule.Controllers
{
    //RedirectToAction("Test", "FullTextSearch", new { area = "EleWise.ELMA.BPM.Web.Common" });
    [Authorize]
    public class PublicationController : BaseController
    {
        
        private readonly PublicationService publicationService;


        public PublicationController(PublicationService publicationService) 
        {
            this.publicationService = publicationService;
        }
        
        public IActionResult Filter(PublicationFilterViewModel viewModel)
        {
            var publications = publicationService.Filter(viewModel);

            return View("Publications", publications);
        }

        [HttpGet]
        public ActionResult Create()
        {
            return View();
        }

        [HttpGet]
        public ActionResult Publications(int first = 1)
        {
            return View(publicationService.Page(first));
        }

        public ActionResult Edit(int id)
        {
            var item = publicationService.LoadWithFile(id);
            return View(item);
        }

        [HttpPost]
        public ActionResult Search(string character)
        {
            if (character == null) return null;
            var model = publicationService.Search(character);
            return PartialView("Publications", model);
        }

        
        [HttpPost]
        public ActionResult Create(IEnumerable<Author> Author, [Bind("Id", Prefix = "Search")]IEnumerable<Author> Search,
            Publication Publication, PublicationType PublicationType, IFormFile FormFile)
        {
            IResult result = null;
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

        public ActionResult Update(PublicationViewModel model)
        {
            return View();
        }
    }
}