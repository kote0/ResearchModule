using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace ResearchModule.Models.Interfaces
{
    public interface IPublicationMultiple<T>
    {
        /// <summary>
        /// Публикация
        /// </summary>
        Publication Publication { get; set; }

        /// <summary>
        /// Дополнение
        /// </summary>
        T Multiple { get; set; }

        /// <summary>
        /// Id Публикации
        /// </summary>
        int PublicationId { get; set; }

        /// <summary>
        /// Id Дополнения
        /// </summary>
        int MultipleId { get; set; }
    }
}
