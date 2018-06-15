using ResearchModule.Models.Interfaces;
using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
using System.Linq;
using System.Threading.Tasks;

namespace ResearchModule.Models
{
    /// <summary>
    /// Публикации : Фильтры
    /// </summary>
    public class PF : IPublicationMultiple<PublicationFilters>, IID
    {
        [Key]
        public int Id { get; set; }

        [DatabaseGenerated(DatabaseGeneratedOption.None)]
        public Publication Publication { get; set; }

        [Required]
        [DatabaseGenerated(DatabaseGeneratedOption.None)]
        public int PublicationId { get; set; }

        [DatabaseGenerated(DatabaseGeneratedOption.None)]
        public PublicationFilters Multiple { get; set; }

        [Required]
        [DatabaseGenerated(DatabaseGeneratedOption.None)]
        public int MultipleId { get; set; }

        public PF() { }

        public PF(int publicationId, int multilpeId)
        {
            PublicationId = publicationId;
            MultipleId = multilpeId;
        }
    }
}
