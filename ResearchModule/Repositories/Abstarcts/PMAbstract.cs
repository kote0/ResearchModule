using ResearchModule.Components.Models;
using ResearchModule.Models.Interfaces;
using ResearchModule.Repository.Interfaces;
using System;
using System.Collections.Generic;
using System.Linq;

namespace ResearchModule.Repository.Abstarcts
{
    public abstract class PMAbstract<TPM, T>  
        where TPM : class, IPublicationMultiple<T>
        where T : class, IID
    {
        private readonly IBaseRepository repository;
        private Result result;

        public PMAbstract(IBaseRepository repository)
        {
            this.repository = repository;
            result = new Result();
        }

        public Result Create(IEnumerable<T> list, int publicationId)
        {
            foreach (var element in list)
            {
                Create(element, publicationId);
            }
            return result;
        }

        public Result Create(T multiple, int publicationId)
        {
            var pm = Activator.CreateInstance<TPM>();
            pm.PublicationId = publicationId;
            AddProperty(ref pm, multiple);
            if (multiple.Id == 0)
                result.Set(repository.Create(multiple).Error);
            pm.MultipleId = multiple.Id;
            result.Set(repository.Create(pm).Error);
            return result;
        }

        public Result Update(IEnumerable<T> list, int publicationId)
        {
            foreach (var element in list)
            {
                Update(element, publicationId);
            }
            return result;
        }

        public Result Update(T multiple, int publicationId)
        {
            var pm = Activator.CreateInstance<TPM>();
            pm.PublicationId = publicationId;
            AddProperty(ref pm, multiple);
            if (multiple.Id == 0)
            {
                return Create(multiple, publicationId);
            }
            else
            {
                pm.MultipleId = multiple.Id;
                result.Set(repository.Update(pm).Error);
                return result;
            }
        }

        public Result Delete(TPM pm)
        {
            result.Set(repository.Delete(pm).Error);
            return result;
        }

        public IEnumerable<T> Find(int publicationId)
        {
            var multiples = repository.Get<TPM>(pm => pm.PublicationId == publicationId);

            List<T> objs = new List<T>();
            foreach (var item in multiples)
            {
                var obj = repository.Get<T>(a => a.Id == item.MultipleId).FirstOrDefault();
                if (obj != null)
                {
                    AddProperty(ref obj, item);
                    objs.Add(obj);
                }
            }
            return objs;
        }

        public abstract void AddProperty(ref TPM pm, T multiple);

        public abstract void AddProperty(ref T multiple, TPM pm);

    }
}
