using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Threading.Tasks;

namespace ResearchModule.Models
{
    public static class PublicationPartition //Section
    {
        public static readonly Dictionary<long, string> Partition = new Dictionary<long, string>
        {
            { 1, "учебные издания" },
            { 2, "научные труды" },
            { 3, "патенты, свидетельства и др" }          
        };
    }
}
