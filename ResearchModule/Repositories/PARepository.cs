using ResearchModule.Models;
using ResearchModule.Repository.Interfaces;

namespace ResearchModule.Repository
{
    /// <summary>
    /// Таблица, хранящая в себе авторов и их публикации
    /// Связь М:М
    /// </summary>
    public class PAuthorRepository : PMultipleRepository<PA, Author>
    {
        public PAuthorRepository(IBaseRepository manager) : base(manager)
        {
        }

        public override void AddProperty(ref Author multiple, PA pm)
        {
            multiple.Weight = pm.Weight;
            multiple.Selected = true;
        }

        public override void AddProperty(ref PA pm, Author multiple)
        {
            pm.Weight = multiple.Weight;
        }
    }

}
