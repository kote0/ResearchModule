using Microsoft.AspNetCore.Mvc.Rendering;
using ResearchModule.Models;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace ResearchModule.Managers
{
    public class TypePublicationManager : BaseManager<TypePublication>
    {
        public List<string> AllTypePublication()
        {
            var typePublications = _db.Set<TypePublication>().Select(m => m.TypePublicationName);
            return typePublications?.ToList();
        }
        public StringBuilder DropbdownList(List<TypePublication> types)
        {
            if (types == null) return null;
            StringBuilder text = new StringBuilder();
            text.Append("<ul class='dropdown-menu'>");
            foreach (var type in types)
            {
                text.AppendFormat("<li class='dropdown-group-item'>{0}</li>", type.TypePublicationName);
            }
            text.Append("</ul>");
            return text;
        }
        
    }
}
