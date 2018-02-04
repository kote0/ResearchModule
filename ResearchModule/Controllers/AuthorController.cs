using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Mvc;
using ResearchModule.Models;

namespace ResearchModule.Controllers
{
    public class AuthorController : BaseController
    {
        public PartialViewResult CreateForm(long id)
        {
            return PartialView(id);
        }

        public PartialViewResult Search(string character)
        {
            if (character == null) return null;
            var text = character.ToLower();
            var authors = manager.GetByFunction<Author>(a => {
                if (a.IsValid())
                {
                    return a.Lastname.ToLower().Contains(text) || a.Surname.ToLower().Contains(text) || a.Name.ToLower().Contains(text);
                }
                else return false;
            });
            return PartialView(authors.ToList());
        }
    }
}