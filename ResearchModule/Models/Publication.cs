using ResearchModule.Managers;
using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
using System.Linq;
using System.Threading.Tasks;

namespace ResearchModule.Models
{
    public partial class Publication
    {
        private IBaseManager manager = new BaseManager(); 

        

        public long Id { get; set; }

        [Required]
        public string PublicationName { get; set; }
        [Required]
        public long? TypePublicationId { get; set; }
        [Required]
        public long? SectionId { get; set; }
        [Required]
        public long? FormWorkId { get; set; }
        public bool? IsTranslate { get; set; }
        public string TranslateText { get; set; }
        public string Language { get; set; }

        public TypePublication TypePublication { get; set; }
        public FormWork FormWork { get; set; }
        public Section Section { get; set; }

        public bool IsValid()
        {
            if (!string.IsNullOrEmpty(PublicationName) && TypePublicationId.HasValue && SectionId.HasValue && FormWorkId.HasValue)
                return true;
            return false;
        }

        public IEnumerable<Author> AuthorsOfPublication()
        {
            var PA = manager.GetByFunction<PA>(p => p.PId == Id)
                .Select(a=> manager.GetByFunction<Author>(au => au.Id == a.AId).FirstOrDefault())
                .Where(o => o.IsValid());
            return PA;
        }

        public void GetOutherProperty()
        {
            if (SectionId.HasValue && TypePublicationId.HasValue && FormWorkId.HasValue)
            {
                this.Section = manager.Get<Section>(SectionId.Value);
                this.TypePublication = manager.Get<TypePublication>(TypePublicationId.Value);
                this.FormWork = manager.Get<FormWork>(FormWorkId.Value);
            }
        }
    }
}
