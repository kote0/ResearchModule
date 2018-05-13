using ResearchModule.Components.Models;
using ResearchModule.Components.Models.Interfaces;
using ResearchModule.Models.Interfaces;
using ResearchModule.Repository.Interfaces;
using System;
using System.Collections.Generic;
using System.Linq;

namespace ResearchModule.Repository
{
    public class PMultipleRepository<TPM, T> 
        where TPM : class, IPublicationMultiple<T>
        where T : class, IID
    {
        private readonly IBaseRepository repository;
        private Result result;

        public PMultipleRepository(IBaseRepository repository)
        {
            this.repository = repository;
            result = new Result();
        }

        public IEnumerable<TPM> Create(IEnumerable<T> list, int publicationId)
        {
            var items = new List<TPM>();
            foreach (var element in list)
            {
                items.Add(Create(element, publicationId));
            }
            return items.Count != 0 ? items : null;
        }

        public TPM Create(T multiple, int publicationId)
        {
            var pm = Activator.CreateInstance<TPM>();
            pm.PublicationId = publicationId;

            AddProperty(ref pm, multiple);

            if (multiple.Id == 0)
                result.Set(repository.Add(multiple).Error);

            pm.MultipleId = multiple.Id;

            result.Set(repository.Add(pm).Error);
            return pm;
        }

        public IResult Delete(TPM pm)
        {
            return repository.Delete(pm);
        }

        public IEnumerable<T> Find(int publicationId)
        {
            var multiples = repository.GetQuery<TPM>(pm => pm.PublicationId == publicationId)
                .ToList();

            List<T> objs = new List<T>();
            foreach (var item in multiples)
            {
                var obj = repository.GetQuery<T>(a => a.Id == item.MultipleId)
                    .FirstOrDefault();

                if (obj != null)
                {
                    AddProperty(ref obj, item);
                    objs.Add(obj);
                }
            }
            return objs;
        }

        public virtual void AddProperty(ref TPM pm, T multiple)
        {
        }

        public virtual void AddProperty(ref T multiple, TPM pm)
        {
        }

    }
}
