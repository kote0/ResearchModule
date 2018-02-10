using Microsoft.AspNetCore.Http;
using ResearchModule.Components.Models;
using ResearchModule.Managers;
using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
using System.Linq;
using System.Threading.Tasks;

namespace ResearchModule.Models
{
    public partial class Publication
    {
        public long Id { get; set; }

        [Required]
        [DisplayName("Название публикации")]
        public string   PublicationName { get; set; }
        
        [Required]
        [DisplayName("Вид публикации")]
        public long PublicationType { get; set; }
        
        [Required]
        [DisplayName("Раздел")]
        public long     PublicationPartition { get; set; }
        
        [Required]
        [DisplayName("Форма работы")]
        public long     PublicationForm { get; set; }

        /// <summary>
        /// Выходные данные 
        /// Издательство
        /// </summary>
        [Required]
        [DisplayName("Издательство")]
        public string   OutputData { get; set; }

        [Required]
        [DisplayName("Объем")]
        public long     Volume { get; set; }

        [Required]
        [DisplayName("Дата создания/изменения")]
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
        [DisplayName("Перевод названия публикации")]
        public string   TranslateText { get; set; }


        //public long Language { get; set; }

        /*
         Дополнить после ввода пользователей
         public Author Author { get; set; }
        */



    }

}
