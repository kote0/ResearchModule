using Microsoft.AspNetCore.Razor.TagHelpers;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace ResearchModule.Components.TagHelpers
{

    public class ReqInputTagHelper : TagHelper
    {
        public string Name { get; set; }

        public override void Process(TagHelperContext context, TagHelperOutput output)
        {
            if (string.IsNullOrEmpty(Name))
            {
                output.SuppressOutput();
            }
            else
            {
                output.TagName = "input";
                output.Attributes.SetAttribute("type", "text");
                output.Attributes.SetAttribute("name", Name);
                output.Attributes.SetAttribute("id", Name.Replace(".", "_"));
                output.Attributes.SetAttribute("data-val", true);
                output.Attributes.SetAttribute("data-val-required", "Заполните обязательное поле");
                output.Attributes.SetAttribute("class", "form-control");
            }
        }
    }
}
