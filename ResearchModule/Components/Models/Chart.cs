using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace ResearchModule.Components.Models
{
    public class Chart
    {
        public string Label { get; set; }

        public string Data { get; set; }

        public string Type { get; set; }

        public string DisplayName { get; set; }

        public string Id { get; set; }

        public Chart()
        {
            if (string.IsNullOrEmpty(Type))
                Type = "'line'";
            if (string.IsNullOrEmpty(DisplayName))
                DisplayName = "'none'";
            if (string.IsNullOrEmpty(Id))
                Id = Guid.NewGuid().ToString().Replace('-', '_');
        }
    }
}
