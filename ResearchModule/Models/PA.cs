using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Threading.Tasks;

namespace ResearchModule.Models
{
    public partial class PA
    {
        public long Id { get; set; }
        [Required]
        public long PublicationId { get; set; }
        [Required]
        public long AuthorId { get; set; }

        /// <summary>
        /// Вес - вклад автора в публикацию
        /// </summary>
        public double Weight { get; set; }

        public PA() { }
        public PA(long publicationId, long authorId)
        {
            PublicationId = publicationId;
            AuthorId = authorId;
        }
    }
}
