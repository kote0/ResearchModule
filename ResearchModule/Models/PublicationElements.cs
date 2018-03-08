using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace ResearchModule.Models
{
    public class PublicationElements
    {
        public List<PublicationElement> Forms { get; set; }
        public List<PublicationElement> Partitions { get; set; }

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
            Partitions = new List<PublicationElement>();
            
            Partitions.Add(
                Create((int)PartitionEnum.educational, "учебные издания", ""));
            Partitions.Add(
                Create((int)PartitionEnum.scientific, "научные труды", ""));
            Partitions.Add(
                Create((int)PartitionEnum.patents, "патенты, свидетельства и др", ""));

            Forms = new List<PublicationElement>();
            Forms.Add(
                Create((int)FormEnum.print, "печатная", "печ.", ""));
            Forms.Add(
                Create((int)FormEnum.handwritten, "рукописаная", "рукоп.", ""));
            Forms.Add(
                Create((int)FormEnum.electronic_source, "электронный ресурс", "электрон. ресур", ""));
            Forms.Add(
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

