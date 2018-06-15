using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace ResearchModule.Components.Models.Interfaces
{
    /// <summary>
    /// Результат выполнения
    /// </summary>
    public interface IResult
    {
        /// <summary>
        /// Успешно
        /// </summary>
        bool Succeeded { get; }

        /// <summary>
        /// Провалено
        /// </summary>
        bool Failed { get; }

        object Model { get; set; }

        /// <summary>
        /// Список ошибкок
        /// </summary>
        List<string> Error { get;  }

        /// <summary>
        /// Установка ошибки
        /// </summary>
        /// <param name="str"></param>
        /// <param name="obj"></param>
        /// <returns></returns>
        IResult Set(string str, params object[] obj);

        /// <summary>
        /// Установка ошибки
        /// </summary>
        /// <param name="str"></param>
        /// <returns></returns>
        IResult Set(string str);

        /// <summary>
        /// Установка ошибок
        /// </summary>
        /// <param name="list"></param>
        /// <returns></returns>
        IResult Set(IEnumerable<string> list);

        /// <summary>
        /// Установка ошибок
        /// </summary>
        IResult Set(IResult result);
    }
}
