using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.RazorPages;
using ResearchModule.Data;
using ResearchModule.Models;
using Microsoft.EntityFrameworkCore;
using ResearchModule.Managers;

namespace ResearchModule.Pages
{
    public class IndexModel : PageModel
    {
        private SectionManager Manager { get; set; }

        public IndexModel(DBContext db)
        {
            Manager = new SectionManager(db);
        }
        public List<Section> Section { get; private set; }

        public void OnGet()
        {
            Section = Manager.GetAll();
        }
    }
}
