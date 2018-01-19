using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Mvc;
using ResearchModule.Models;
using ResearchModule.Managers;

namespace ResearchModule.Controllers
{
    public class AuthorController : BaseController
    {
        public PartialViewResult SelectList()
        {
            var list = manager.GetByFunction<Author>(a => a.IsValid())
                .Select(a =>
                    new SelectListItem
                    {
                        Value = a.Id,
                        Text = string.Format("{0}.{1}. {2}", a.Surname.Substring(0, 1), a.LastName.Substring(0, 1), a.Name)
                    })
                .ToList();

            var selectList = new SelectList();
            selectList.SetName("Author");
            selectList.AddRange(list);
            return PartialView("Components/SelectList", selectList);
        }

        public PartialViewResult Add(long id)
        {
            return PartialView("../Add/Authors", id);
        }

        public PartialViewResult Search(string character)
        {
            if (character == null) return null;
            var text = character.ToLower();
            var authors = manager.GetByFunction<Author>(a => {
                if (a.IsValid())
                {
                    return a.LastName.ToLower().Contains(text) || a.Surname.ToLower().Contains(text) || a.Name.ToLower().Contains(text);
                }  
                else return false;
            });
            return PartialView("Search", authors.ToList());
        }
    }
}