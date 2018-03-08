using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Threading.Tasks;

namespace ResearchModule.Models
{
    public partial class PA
    {
        public int Id { get; set; }

        [Required]
        public int PublicationId { get; set; }
        public virtual Publication Publication { get; set; }

        [Required]
        public int AuthorId { get; set; }
        public virtual Author Author { get; set; }

        /// <summary>
        /// Вес - вклад автора в публикацию
        /// </summary>
        public double Weight { get; set; }

        public PA() { }
        public PA(int publicationId, int authorId)
        {
            PublicationId = publicationId;
            AuthorId = authorId;
        }
    }
}
