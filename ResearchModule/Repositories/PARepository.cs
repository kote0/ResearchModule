using ResearchModule.Models;
using ResearchModule.Repository.Abstarcts;
using ResearchModule.Repository.Interfaces;

namespace ResearchModule.Repository
{
    /// <summary>
    /// Таблица, хранящая в себе авторов и их публикации
    /// Связь М:М
    /// </summary>
    public class PARepository : PMAbstract<PA, Author>
    {
        public PARepository(IBaseRepository manager) : base(manager)
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
