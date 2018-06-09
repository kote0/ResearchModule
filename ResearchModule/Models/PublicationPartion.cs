using ResearchModule.Models.Interfaces;
using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Linq;
using System.Threading.Tasks;

namespace ResearchModule.Models
{
    public class PublicationForm : IName
    {
        public int Id { get; set; }
        public string Name { get; set; }
        public string ShortName { get; set; }
    }

    public class PublicationPartion : IName
    {
        public int Id { get; set; }

        [DisplayName("Название раздела")]
        public string Name { get; set; }
    }

    public static class PublicationElems
    {
        public static PublicationPartion educational 
            = new PublicationPartion() { Id = 1, Name = "учебные издания" };
        public static PublicationPartion scientific 
            = new PublicationPartion() { Id = 2, Name = "научные труды" };
        public static PublicationPartion patents 
            = new PublicationPartion() { Id = 3, Name = "патенты, свидетельства и др" };

        public static IEnumerable<PublicationPartion> GetPartions()
        {
            var list = new List<PublicationPartion>();
            list.Add(educational);
            list.Add(scientific);
            list.Add(patents);
            return list;
        }

        public static PublicationPartion GetPartion(int id)
        {
            return GetPartions().First(i => i.Id == id);
        }

        public static string GetPartionName(int id)
        {
            return GetPartion(id).Name;
        }

        public static PublicationForm print =
            new PublicationForm() { Id = 1, Name = "печатная", ShortName = "печ." };
        public static PublicationForm handwritten =
            new PublicationForm() { Id = 2, Name = "рукописаная", ShortName = "рукоп." };
        public static PublicationForm electronicSource =
            new PublicationForm() { Id = 3, Name = "электронный ресурс", ShortName = "электрон. ресур" };
        public static PublicationForm audiovisual =
            new PublicationForm() { Id = 4, Name = "аудиовизуальная", ShortName = "аудиовиз." };

        public static IEnumerable<PublicationForm> GetForms()
        {
            var list = new List<PublicationForm>();
            list.Add(print);
            list.Add(handwritten);
            list.Add(electronicSource);
            list.Add(audiovisual);
            return list;
        }

        public static PublicationForm GetForm(int id)
        {
            return GetForms().First(i => i.Id == id);
        }

        public static string GetFormName(int id)
        {
            return GetForm(id).Name;
        }

        public static bool IsPrintForm(int id)
        {
            if (id == electronicSource.Id || id == audiovisual.Id)
            {
                return false;
            }
            return true;
        }
        
    }
}
