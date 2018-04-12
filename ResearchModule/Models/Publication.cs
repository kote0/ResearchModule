using ResearchModule.Components.Models;
using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.ComponentModel.DataAnnotations;

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

        [DisplayName("Вид публикации")]
        public virtual PublicationType PublicationType { get; set; }

        /// <summary>
        /// Раздел
        /// </summary>
        [Required]
        [DisplayName("Раздел")]
        public int PublicationPartition { get; set; }

        /// <summary>
        /// Форма работы
        /// </summary>
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

        /// <summary>
        /// Дата создания
        /// </summary>
        [Required]
        [DisplayName("Дата создания")]
        public DateTime CreateDate { get; set; }

        /// <summary>
        /// Дата изменения
        /// </summary>
        [Required]
        [DisplayName("Дата изменения")]
        public DateTime ModifyDate { get; set; }

        /// <summary>
        /// Файл
        /// </summary>
        public FileDetail PublicationFile { get; set; }

        /// <summary>
        /// Файл
        /// </summary>
        [Required]
        [DisplayName("Файл")]
        public int PublicationFileId { get; set; }

        /// <summary>
        /// Перевод(может отсутствовать)
        /// </summary>
        [DisplayName("Перевод названия публикации")]
        public string TranslateText { get; set; }

        [DisplayName("Язык")]
        public int? Language { get; set; }

        public ICollection<PA> PAs { get; set; }

        public virtual ICollection<PF> PFs { get; set; }

        /*[DisplayName("Автор публикации")]
        public User Author { get; set; }*/

        ///// <summary>
        ///// Uid Файл 
        ///// </summary>
        //[Required]
        //public string PublicationFileUid { get; set; }

        ///// <summary>
        ///// Название файла
        ///// </summary>
        //[Required]
        //[DisplayName("Название файла")]
        //public string PublicationFileName { get; set; }

    }

}
