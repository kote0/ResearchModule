using ResearchModule.Components.Models;
using ResearchModule.Models;
using ResearchModule.Repository.Interfaces;
using ResearchModule.Service;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace ResearchModule.Components.Page
{
    public class UserPage : IPageCreator
    {
        private readonly IBaseRepository repository;

        public UserPage(IBaseRepository repository)
        {
            this.repository = repository;

        }

        public object CreatePagination(int first, string action, string controller, string dataId = null)
        {
            var list = repository.Include<User, Author>(a => a.Author).Where(u=>!u.IsDeleted);
            var count = list.LongCount<User>();
            var page = new PageInfo(first, count);
            page.SetUrl(action, controller);
            if (dataId != null)
                page.DataId = dataId;
            page.Model = list.Page(first).ToList();
            return page;
        }

        public object CreatePagination(object list, int first, string action, string controller, string dataId = null)
        {
            throw new NotImplementedException();
        }
    }
}
