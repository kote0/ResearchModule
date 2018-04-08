using ResearchModule.Managers;
using ResearchModule.Managers.Interfaces;
using ResearchModule.Models;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace ResearchModule.Service
{
    public class PublicationService
    {
        private readonly IBaseManager Manager;
        private readonly PAManager paManager;
        private readonly PublicationElements publicationElements;

        public PublicationService(PAManager paManager, IBaseManager manager, PublicationElements publicationElements)
        {
            this.Manager = manager;
            this.paManager = paManager;
            this.publicationElements = publicationElements;
        }

        public async Task<IEnumerable<Author>> GetAuthors(int id)
        {
            return await paManager.FindAuthors(id);
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
            var item = Manager.Get<PublicationType>(id);

            return item.Name ?? String.Empty;
        }
    }
}
