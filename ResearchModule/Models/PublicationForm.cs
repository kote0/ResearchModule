using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Threading.Tasks;

namespace ResearchModule.Models
{
    public static class PublicationForm
    {
        public static readonly Dictionary<long, FormWork> Forms = new Dictionary<long, FormWork>
        {
            { 1, new FormWork() { Name = "печатная", ShortName = "печ." } },
            { 2, new FormWork() { Name = "электронный ресурс", ShortName = "электрон. ресурс" } },
            { 3, new FormWork() { Name = "рукописаная", ShortName = "рукоп." } },
            { 4, new FormWork() { Name = "аудиовизуальная", ShortName = "аудиовиз." } }
        };

       
    }

    public class FormWork
    {
        public string Name { get; set; }
        public string ShortName { get; set; }
    }
}
