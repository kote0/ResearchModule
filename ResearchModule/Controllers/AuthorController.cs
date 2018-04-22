using System.Linq;
using Microsoft.AspNetCore.Mvc;
using ResearchModule.Models;
using ResearchModule.Service;
using ResearchModule.Repository.Interfaces;
using ResearchModule.Extensions;
using Microsoft.AspNetCore.Authorization;

namespace ResearchModule.Controllers
{
    [Authorize]
    public class AuthorController : BaseController
    {
        private readonly IBaseRepository repository;
        public AuthorController(IBaseRepository repository)
        {
            this.repository = repository;
        }

        public PartialViewResult CreateForm(int id)
        {
            return PartialView(id);
        }

        public PartialViewResult Search(string character)
        {
            if (character == null) return null;
            var authors = repository.Get<Author>(a => a.Contains(character));

            return PartialView(authors.ToList());
        }
    }
}