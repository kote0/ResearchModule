using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace ResearchModule.Models
{
    public class User
    {
        public long Id { get; set; }
        public long AuthorId { get; set; }
        public AccessLevel Access { get; set; }
        public bool IsDeleted { get; set; }
    }

    public enum AccessLevel
    {
        // полные права
        admin,
        // Доступ на добавление публикаций
        additionalPublication
    }
}
