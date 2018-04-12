using Microsoft.AspNetCore.Http;
using ResearchModule.Components.Models.Interfaces;
using ResearchModule.Models;
using ResearchModule.ViewModels;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace ResearchModule.Service.Interfaces
{
    public interface IPublicationService : IService
    {
        PublicationsViewModel Page(int first);
        PublicationsViewModel Search(string character);
        PublicationsViewModel CreateView(IEnumerable<Publication> list, int first = 1);
        Publication LoadWithFile(int id);
        Publication Load(int id);
        IResult Create(Publication publication, PublicationType type, IFormFile file, IEnumerable<Author> createdAuthors, IEnumerable<Author> selectedAuthors);
        IResult Update(Publication publication, PublicationType type, IFormFile file, IEnumerable<Author> createdAuthors, IEnumerable<Author> selectedAuthors);
    }
}
