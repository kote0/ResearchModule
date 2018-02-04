using Microsoft.AspNetCore.Mvc.Rendering;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace ResearchModule.Models
{
    public class SelectList
    {
        private string Name { get; set; }

        public List<SelectListItem> Elements { get; set; }

        public SelectList()
        {
            Elements = new List<SelectListItem>();
        }
        public void Add(SelectListItem selectListItem)
        {
            if (selectListItem != null)
            {
                Elements.Add(selectListItem);
            }
        }

        public void AddRange(List<SelectListItem> list)
        {
            if (list != null && list.Count > 0)
            {
                Elements.AddRange(list);
            }
        }

        public void SetName(string name)
        {
            if (!string.IsNullOrEmpty(name))
            {
                Name = name;
            }
        }

        public string GetName()
        {
            return Name;
        }
    }

    public class SelectListItem
    {
        public long Value { get; set; }

        public string Text { get; set; }

        public bool Selected { get; set; }

        public SelectListItem()
        {
            Selected = false;
        }
    }
}
