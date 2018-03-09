using ResearchModule.Managers;
using ResearchModule.Models;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace ResearchModule.Service
{
    public class PublicationService
    {
        private readonly BaseManager manager;
        private readonly PAManager paManager;
        private readonly PublicationElements publicationElements;

        public PublicationService(PAManager paManager, BaseManager manager, PublicationElements publicationElements)
        {
            this.manager = manager;
            this.paManager = paManager;
            this.publicationElements = publicationElements;
        }

        public async Task<IEnumerable<Author>> GetAuthors(int id)
        {
            return await paManager.FindAuthorsByPublication(id);
        }

        public string GetFormName(int id)
        {
            var formWork = publicationElements.Forms.FirstOrDefault(o => id.Equals(o.Id));
            return formWork.Name;
        }

        public string GetPartitionName(int id)
        {
            return publicationElements.Partitions.FirstOrDefault(o => o.Id == id).Name;
        }

        public string GetTypeName(int id)
        {
            return manager.Get<PublicationType>(id).Name;
        }
    }
}
