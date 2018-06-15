using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace ResearchModule.Components.Models
{
    /// <summary>
    /// Модель для создания графиков
    /// </summary>
    public class Chart
    {
        public string Label { get; set; }

        public string Data { get; set; }

        public ChartData ChartData { get; set; }

        public string Type { get; set; }

        public string DisplayName { get; set; }

        public string Id { get; set; }

        public Chart()
        {
            if (string.IsNullOrEmpty(Id))
                Id = Guid.NewGuid().ToString().Replace('-', '_');

            if (string.IsNullOrEmpty(DisplayName))
                DisplayName = "'none'";

            if (string.IsNullOrEmpty(Type))
                Type = "'line'";
        }
        
    }

    /// <summary>
    /// Модель для хранения значений в Chart
    /// </summary>
    public class ChartData
    {
        public string Data { get; set; }

        public string Type { get; set; }
    }
    
}
