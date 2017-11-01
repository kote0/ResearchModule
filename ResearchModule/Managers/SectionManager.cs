using Microsoft.EntityFrameworkCore;
using ResearchModule.Data;
using ResearchModule.Models;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace ResearchModule.Managers
{
    public class SectionManager
    {
        private readonly DBContext _db;

        public SectionManager(DBContext db)
        {
            _db = db;
        }
        public Section Create()
        {
            throw new NotImplementedException();
        }

        public void Delete(long id)
        {
            var record = _db.Section.Find(id);

            if (record != null)
            {
                _db.Section.Remove(record);
                _db.SaveChanges();
            }
        }

        public Section Get(long id)
        {
            return _db.Section.Find(id);
        }

        public void Update(Section record)
        {
            if (record == null) return;
            _db.Attach(record).State = EntityState.Modified;
        }
        public List<Section> GetAll()
        {
            return _db.Section.ToList();
        }
    }
}
