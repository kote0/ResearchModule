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

        public Chart()
        {
        }

        public Chart(string label, string data)
        {
            Label = label;
            Data = data;
        }
    }
}
