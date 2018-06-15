using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace ResearchModule.Components.Page
{
    public interface IPageCreator
    {
        object CreatePagination(int first, string action, string controller, string dataId = null);

        object CreatePagination(object list, int first, string action, string controller, string dataId = null);
    }
}
