using Microsoft.AspNetCore.Http;
using ResearchModule.Components.Models;
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
        public long Id { get; set; }

        /// <summary>
        /// Название
        /// </summary>
        [Required]
        public string   PublicationName { get; set; }

        /// <summary>
        /// Вид
        /// </summary>
        [Required]
        public long PublicationType { get; set; }

        /// <summary>
        /// Раздел
        /// </summary>
        [Required]
        public long     PublicationPartition { get; set; }

        /// <summary>
        /// Форма
        /// </summary>
        [Required]
        public long     PublicationForm { get; set; }

        /// <summary>
        /// Выходные данные 
        /// Издательство
        /// </summary>
        [Required]
        public string   OutputData { get; set; }

        /// <summary>
        /// Объем
        /// </summary>
        [Required]
        public long     Volume { get; set; }

        /// <summary>
        /// Дата создания/Изменения
        /// </summary>
        [Required]
        public DateTime CreateDate { get; set; }

        /// <summary>
        /// Uid Файл 
        /// </summary>
        [Required]
        public string PublicationFileUid { get; set; }

        /// <summary>
        /// Название файла
        /// </summary>
        [Required]
        public string PublicationFileName { get; set; }

        /// <summary>
        /// Перевод(может отсутствовать)
        /// </summary>
        public string   TranslateText { get; set; }


        //public long Language { get; set; }

        /*
         Дополнить после ввода пользователей
         public Author Author { get; set; }
        */



    }

}
