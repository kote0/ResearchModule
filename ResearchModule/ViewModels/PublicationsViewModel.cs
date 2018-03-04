using ResearchModule.Components.Models;
using ResearchModule.Managers;
using ResearchModule.Models;
using ResearchModule.Service;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;


namespace ResearchModule.ViewModels
{
    public class PublicationsViewModel
    {
        public IQueryable<Publication> Publications { get; }

        public IEnumerable<Author> Authors { get; }

        private BaseManager manager;
        private readonly PublicationService publicationService;

        public PublicationsViewModel(BaseManager manager, PublicationService publicationService)
        {
            this.manager = manager;
            this.publicationService = publicationService;
            /*manager = new BaseManager();
            Publications = manager.GetAll<Publication>();
            var t = Publications.Select(a => {
                var te = a.Id;
                var ts = PublicationService.GetAuthorsByPublication(te);
            }
            new Pair<long, string>
            {
               
            });
            
            
            foreach (var author in PublicationService.GetAuthorsByPublication(item.Id))
            {
                author.ToStringFormat();
            }*/
        }
        public PublicationsViewModel(IQueryable<Publication> Publications)
        {
            Publications.Select(a => new Pair<long, IEnumerable<Author>>
            {
                First = a.Id,
                Second = publicationService.GetAuthorsByPublication(a.Id)
            });

        }
    }
}



/*public static class Pagination
{
    public static IEnumerable<T> Page<T>(this IEnumerable<T> source, int page, int pageSize)
    {
        return source.Skip((page - 1) * pageSize).Take(pageSize);
    }
}*/

