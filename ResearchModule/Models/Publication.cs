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
        public int Id { get; set; }

        [Required]
        [DisplayName("Название публикации")]
        public string PublicationName { get; set; }

        [Required]
        [DisplayName("Вид публикации")]
        public int PublicationTypeId { get; set; }

        
        public virtual PublicationType PublicationType { get; set; }

        [Required]
        [DisplayName("Раздел")]
        public int PublicationPartition { get; set; }

        [Required]
        [DisplayName("Форма работы")]
        public int PublicationForm { get; set; }

        /// <summary>
        /// Выходные данные / Издательство
        /// </summary>
        [Required]
        [DisplayName("Издательство")]
        public string OutputData { get; set; }

        [Required]
        [DisplayName("Объем")]
        public long Volume { get; set; }

        [Required]
        [DisplayName("Дата создания")]
        public DateTime CreateDate { get; set; }

        [Required]
        [DisplayName("Дата изменения")]
        public DateTime ModifyDate { get; set; }

        /// <summary>
        /// Uid Файл 
        /// </summary>
        [Required]
        public string PublicationFileUid { get; set; }

        /// <summary>
        /// Название файла
        /// </summary>
        [Required]
        [DisplayName("Название файла")]
        public string PublicationFileName { get; set; }

        /// <summary>
        /// Перевод(может отсутствовать)
        /// </summary>
        [DisplayName("Перевод названия публикации")]
        public string TranslateText { get; set; }

        [DisplayName("Язык")]
        public int? Language { get; set; }

        public ICollection<PA> PAs { get; set; }

        public ICollection<PF> PFs { get; set; }

    }

}
