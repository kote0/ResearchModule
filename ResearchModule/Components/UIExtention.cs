using Microsoft.AspNetCore.Html;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.Rendering;
using Microsoft.AspNetCore.Mvc.ViewComponents;
using Microsoft.AspNetCore.Mvc.ViewFeatures;
using Microsoft.AspNetCore.Routing;
using ResearchModule.Components.Models;
using ResearchModule.Managers;
using ResearchModule.Models;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace ResearchModule.Components
{
    public static class UIExtention
    {
        public const string Components  = "Components/";

        public static IHtmlContent TestElement(this IHtmlHelper html, string id)
        {
            return html.Partial("Index", id);
        }

        public static IHtmlContent Button(this IHtmlHelper html, string id, string value, IHtmlContent icon = null, object routeValues = null)
        {
            //new Dictionary<string, object> { { "enctype", "multipart/form-data" }, { "data-ajax", "false"} })
            return Button(html, id, value, icon, new RouteValueDictionary(routeValues));
        }

        public static IHtmlContent Button(this IHtmlHelper html, string id, string value, IHtmlContent icon = null, RouteValueDictionary routeValues = null)
        {
            var tagBuilder = new TagBuilder("button");
            tagBuilder.GenerateId(id, id);
            tagBuilder.AddCssClass("btn-default");
            tagBuilder.AddCssClass("btn");
            tagBuilder.InnerHtml.AppendHtml(icon);
            tagBuilder.InnerHtml.AppendHtml(value);

            if (routeValues != null) {
                tagBuilder.MergeAttributes(routeValues, true);
            }
            tagBuilder.RenderSelfClosingTag();
            
            return tagBuilder;
        }

        public static IHtmlContent Icon(this IHtmlHelper html, string iconName)
        {
            var tagBuilder = new TagBuilder("span");
            var icon = string.Format("glyphicon-{0}", iconName);
            tagBuilder.AddCssClass(icon);
            tagBuilder.AddCssClass("glyphicon");
            tagBuilder.RenderSelfClosingTag();

            return tagBuilder;
        }

        #region Card

        public static Card Card(this IHtmlHelper html, string id)
        {
            return new Card(html, id);
        }
        
        #endregion
    }

    public class AuthorSelectViewComponent : ViewComponent
    {
        BaseManager mng = new BaseManager();
        

        public async Task<IViewComponentResult> InvokeAsync()
        {
            //--- TODO: Вынести в private метод
            var list = mng.GetByFunction<Author>(a => a.IsValid()) //сделать асинхронным
                .Select(a =>
                    new ResearchModule.Models.SelectListItem
                    {
                        Value = a.Id,
                        Text = string.Format("{0}.{1}. {2}", a.Surname.Substring(0, 1), a.LastName.Substring(0, 1), a.Name)
                    })
                .ToList();

           /* var selectList = new ResearchModule.Models.SelectList();
            selectList.SetName("Author");
            selectList.AddRange(list);*/
            //--- 
            return View("../SelectList", selectListCreate(list, "Author"));
        }

        // перенести в единый класс для компонентов
        // изучить, как работать с ViewComponent
        private ResearchModule.Models.SelectList selectListCreate(List<ResearchModule.Models.SelectListItem> list, string name)
        {
            var selectList = new ResearchModule.Models.SelectList();
            selectList.SetName(name);
            selectList.AddRange(list);
            return selectList;
        }
    }



    public class SectionSelectViewComponent : ViewComponent
    {
        BaseManager mng = new BaseManager();

        public async Task<IViewComponentResult> InvokeAsync()
        {
            var list = mng.GetByFunction<Section>(a => a.IsValid())
                .Select(a =>
                    new ResearchModule.Models.SelectListItem
                    {
                        Value = a.Id,
                        Text =  a.SectionName
                    })
                .ToList();

            var selectList = new ResearchModule.Models.SelectList();
            selectList.SetName("Section");
            selectList.AddRange(list);

            return View("../SelectList", selectList);
        }
    }

    public class TypePublicationSelectViewComponent : ViewComponent
    {
        BaseManager mng = new BaseManager();

        public async Task<IViewComponentResult> InvokeAsync()
        {
            var list = mng.GetByFunction<TypePublication>(a => a.IsValid())
                .Select(a =>
                    new ResearchModule.Models.SelectListItem
                    {
                        Value = a.Id,
                        Text = a.TypePublicationName
                    })
                .ToList();

            var selectList = new ResearchModule.Models.SelectList();
            selectList.SetName("TypePublicationId");
            selectList.AddRange(list);

            return View("../SelectList", selectList);
        }
    }
    public class FormWorkSelectViewComponent : ViewComponent
    {
        BaseManager mng = new BaseManager();

        public async Task<IViewComponentResult> InvokeAsync()
        {
            var list = mng.GetByFunction<FormWork>(a => a.IsValid())
                .Select(a =>
                    new ResearchModule.Models.SelectListItem
                    {
                        Value = a.Id,
                        Text = string.Format("{0}({1})", a.FormName, a.ShortName)
                    })
                .ToList();

            var selectList = new ResearchModule.Models.SelectList();
            selectList.SetName("FormWorkId");
            selectList.AddRange(list);

            return View("../SelectList", selectList);
        }
    }

}
