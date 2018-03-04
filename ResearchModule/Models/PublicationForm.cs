using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Threading.Tasks;

namespace ResearchModule.Models
{
    public static class PublicationForm
    {
        public enum Forms
        {
            print,
            handwritten,
            electronic_source,
            audiovisual
        }

        public static readonly Dictionary<string, FormWork> FormDictionary;

        static PublicationForm()
        {
            FormDictionary = new Dictionary<string, FormWork>
            {
                { Forms.print.ToString(), new FormWork() { Name = "печатная", ShortName = "печ." } },
                { Forms.handwritten.ToString(), new FormWork() { Name = "рукописаная", ShortName = "рукоп." } },
                { Forms.electronic_source.ToString(), new FormWork() { Name = "электронный ресурс", ShortName = "электрон. ресурс" } },
                { Forms.audiovisual.ToString(), new FormWork() { Name = "аудиовизуальная", ShortName = "аудиовиз." } }
            };
        }

    }



    public class FormWork
    {
        public string Name { get; set; }
        public string ShortName { get; set; }
    }

}
