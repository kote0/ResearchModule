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
        private readonly BaseManager manager = new BaseManager();
        private readonly PAManager mngPA = new PAManager();

        public string eee()
        {
            return "yesss!";
        }

        public IEnumerable<Author> GetAuthorsByPublication(long id)
        {
            return mngPA.FindAuthorsByPublication(id);
        }

        public string GetFormName(long id)
        {
            return ResearchModule.Models.PublicationForm.Forms.FirstOrDefault(o => o.Key == id).Value.Name;
        }

        public string GetPartitionName(long id)
        {
            return ResearchModule.Models.PublicationPartition.Partition.FirstOrDefault(o => o.Key == id).Value;
        }

        public string GetTypeName(long id)
        {
            return manager.Get<PublicationType>(id).Name;
        }
    }
}
