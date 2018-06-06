using Microsoft.AspNetCore.Html;
using Microsoft.AspNetCore.Razor.TagHelpers;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace ResearchModule.Components.TagHelpers
{
    public class ReqTagHelper : TagHelper
    {
        private bool required = true;

        public bool Required
        {
            get { return required; }
            set { required = value; }
        }

        public override void Process(TagHelperContext context, TagHelperOutput output)
        {
            if (required)
                Validate.Create(output);
        }
    }

    public class ReqInputTagHelper : ReqTagHelper
    {
        public string Name { get; set; }

        public string Value { get; set; }

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
                output.Attributes.SetAttribute("value", Value ?? "");
                output.Attributes.SetAttribute("id", Name.Replace(".", "_"));
                output.Attributes.SetAttribute("class", "form-control");
                base.Process(context, output);
            }
        }
    }

    public class InputGroupBtnTagHelper : TagHelper
    {
        public IHtmlContent Input { get; set; }

        public IHtmlContent Button { get; set; }

        public override void Process(TagHelperContext context, TagHelperOutput output)
        {
            output.TagName = "div";
            output.Attributes.SetAttribute("class", "input-group");
            output.Content.SetHtmlContent(Input);
            output.Content.AppendFormat("<span class='input-group-btn'>{0}</span>", Button);
        }

    }


    public class SelectListTagHelper : TagHelper
    {
        public ResearchModule.Models.SelectList Items { get; set; }

        public string Title { get; set; }

        public string Name { get; set; }

        public string OnChange { get; set; }

        public bool Multiple { get; set; }

        public bool LiveSearch { get; set; }

        public override void Process(TagHelperContext context, TagHelperOutput output)
        {
            output.TagName = "select";
            var classNames = "form-control selectpicker";
            if (Items != null)
            {
                var name = Items.GetName() ?? Name;
                if (!string.IsNullOrEmpty(name))
                {
                    output.Attributes.SetAttribute("name", name);
                    output.Attributes.SetAttribute("id", name.Replace(".", "_"));
                    classNames += string.Format(" selectpicker_{0}", name);

                }
                if (Items.Elements == null || Items.Elements.Count == 0)
                {
                    Title = Title ?? "Ничего не выбрано";
                }

                foreach (var elem in Items.Elements)
                {
                    output.Content.AppendFormat("<option {2} value={1}>{0}</option>", elem.Text, elem.Value, elem.Selected ? "selected" : "");
                }
            }
            output.Attributes.SetAttribute("class", classNames);
            output.Attributes.SetAttribute("title", Title ?? "Ничего не выбрано");

            if (!string.IsNullOrEmpty(OnChange))
            {
                output.Attributes.SetAttribute("onChange", OnChange);
            }

            if (Multiple)
            {
                output.Attributes.SetAttribute("multiple", "");
            }
            
            if (LiveSearch)
            {
                output.Attributes.SetAttribute("data-live-search", "true");

            }

        }
    }

    public static class Validate
    {
        public static void Create(TagHelperOutput output)
        {
            output.Attributes.SetAttribute("data-val", true);
            output.Attributes.SetAttribute("data-val-required", "Заполните обязательное поле");
        }
        public static void Remove(TagHelperOutput output)
        {
            output.Attributes.SetAttribute("data-val", false);
            output.Attributes.SetAttribute("data-val-required", "");
        }

    }
}
