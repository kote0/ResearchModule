using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace ResearchModule.Models
{
    public class PublicationElements
    {
        private List<PublicationElement> partitions { get; set; }

        private List<PublicationElement> forms { get; set; }

        public IEnumerable<PublicationElement> Forms
        {
            get
            {
                return forms.Where(p => !p.Obsolete).ToList();
            }
        }

        public IEnumerable<PublicationElement> Partitions
        {
            get
            {
                return partitions.Where(p => !p.Obsolete);
            }
        }

        public enum FormEnum
        {
            print,
            handwritten,
            electronic_source,
            audiovisual
        }

        public enum PartitionEnum
        {
            educational,
            scientific,
            patents
        }

        public PublicationElements()
        {
            partitions = new List<PublicationElement>();
            partitions.Add(
                Create((int)PartitionEnum.educational, "учебные издания", ""));
            partitions.Add(
                Create((int)PartitionEnum.scientific, "научные труды", ""));
            partitions.Add(
                Create((int)PartitionEnum.patents, "патенты, свидетельства и др", ""));

            forms = new List<PublicationElement>();
            forms.Add(
                Create((int)FormEnum.print, "печатная", "печ.", ""));
            forms.Add(
                Create((int)FormEnum.handwritten, "рукописаная", "рукоп.", ""));
            forms.Add(
                Create((int)FormEnum.electronic_source, "электронный ресурс", "электрон. ресур", ""));
            forms.Add(
                Create((int)FormEnum.audiovisual, "аудиовизуальная", "аудиовиз.", ""));
        }


        private PublicationElement Create(int id, string name, string shortName, string description, bool obsolete = false)
        {
            return new PublicationElement()
            {
                Id = id,
                Name = name,
                ShortName = shortName,
                Description = description,
                Obsolete = obsolete
            };
        }

        private PublicationElement Create(int id, string name, string description, bool obsolete = false)
        {
            return Create(id, name, "", description, obsolete);
        }
    }
}

