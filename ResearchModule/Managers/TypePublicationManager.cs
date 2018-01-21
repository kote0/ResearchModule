using Microsoft.AspNetCore.Mvc.Rendering;
using ResearchModule.Models;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace ResearchModule.Managers
{
    //TODO: Удалить TypePublicationManager
    public class TypePublicationManager : BaseManager
    {
        public List<string> AllTypePublication()
        {
            var typePublications = GetByFunction<TypePublication>(p => p.IsValid()).Select(m => m.TypePublicationName);
            return typePublications?.ToList();
        }
        
    }
}
