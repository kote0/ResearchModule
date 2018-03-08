using Microsoft.AspNetCore.Razor.TagHelpers;
using ResearchModule.Components.Models;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace ResearchModule.Components.TagHelpers
{
    public class PagingTagHelper : TagHelper
    {
        public PageInfo PageInfo { get; set; }

        public Func<int, string> PageUrl { get; set; }

        public override void Process(TagHelperContext context, TagHelperOutput output)
        {
            StringBuilder result = new StringBuilder();
            for (int i = 1; i < PageInfo.TotalPages; i++)
            {
                output.TagName = "a";
                output.Attributes.SetAttribute("href", PageUrl(i));
                output.Content.SetHtmlContent(i.ToString());
                string classes = "btn btn-default";
                if (i == PageInfo.PageNumber)
                {
                    classes += "selected btn-primary";
                }
                output.Attributes.SetAttribute("class", classes);
            }            
        }
    }
}
