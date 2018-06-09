using ResearchModule.Models.Interfaces;
using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
using System.Linq;
using System.Threading.Tasks;

namespace ResearchModule.Models
{
    /// <summary>
    /// Публикации : Авторы
    /// </summary>
    public class PA : IPublicationMultiple<Author>, IID
    {
        [Key]
        public int Id { get; set; }
        
        public virtual Publication Publication { get; set; }
        
        public virtual Author Multiple { get; set; }

        /// <summary>
        /// Вес - вклад автора в публикацию
        /// </summary>
        public double Weight { get; set; }

        [Required]
        public int PublicationId { get; set; }

        [Required]
        public int MultipleId { get; set; }

        [DisplayName("Соавтор")]
        public bool Coauthor { get; set; }

        public PA() { }

        public PA(int publicationId, int multilpeId)
        {
            PublicationId = publicationId;
            MultipleId = multilpeId;
        }
    }
}
