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

        [Required(ErrorMessage = "Заполните обязательное поле")]
        [DisplayName("Название публикации")]
        public string PublicationName { get; set; }

        [Required(ErrorMessage = "Заполните обязательное поле")]
        [DisplayName("Вид публикации")]
        public int PublicationTypeId { get; set; }

        [DisplayName("Вид публикации")]
        public virtual PublicationType PublicationType { get; set; }

        /// <summary>
        /// Раздел
        /// </summary>
        [Required(ErrorMessage = "Заполните обязательное поле")]
        [DisplayName("Раздел")]
        public int PublicationPartition { get; set; }

        /// <summary>
        /// Форма работы
        /// </summary>
        [Required(ErrorMessage = "Заполните обязательное поле")]
        [DisplayName("Форма работы")]
        public int PublicationForm { get; set; }

        /// <summary>
        /// Выходные данные / Издательство
        /// </summary>
        [Required(ErrorMessage = "Заполните обязательное поле")]
        [DisplayName("Издательство")]
        public string OutputData { get; set; }

        [Required(ErrorMessage = "Заполните обязательное поле")]
        [DisplayName("Объем")]
        public long Volume { get; set; }

        /// <summary>
        /// Дата создания
        /// </summary>
        [DisplayName("Дата создания")]
        public DateTime CreateDate { get; set; }

        /// <summary>
        /// Дата изменения
        /// </summary>
        [DisplayName("Дата изменения")]
        public DateTime ModifyDate { get; set; }

        /// <summary>
        /// Файл
        /// </summary>
        public FileDetail PublicationFile { get; set; }

        /// <summary>
        /// Файл
        /// </summary>
        [Required(ErrorMessage = "Заполните обязательное поле")]
        [DisplayName("Файл")]
        public int PublicationFileId { get; set; }

        /// <summary>
        /// Перевод(может отсутствовать)
        /// </summary>
        [DisplayName("Перевод названия публикации")]
        public string TranslateText { get; set; }

        [DisplayName("Язык")]
        public int? Language { get; set; }

        public List<PA> PAs { get; set; }

        [DisplayName("Фильтры")]
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
