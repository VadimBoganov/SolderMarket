using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.Rendering;
using Microsoft.AspNetCore.Mvc.Routing;
using Microsoft.AspNetCore.Mvc.ViewFeatures;
using Microsoft.AspNetCore.Razor.TagHelpers;
using solder.Models;

namespace solder.TagHeplers
{
    public class SortSolderTagHelper : TagHelper
    {
        public SortState Property {get;set;}
        public SortState Current {get;set;}
        public string Action {get;set;}
        public bool Up {get;set;}

        [ViewContext]
        [HtmlAttributeNotBound]
        public ViewContext ViewContext {get;set;}
        private IUrlHelperFactory _urlHelperFactory;

        public SortSolderTagHelper(IUrlHelperFactory urlHelperFactory)
        {
            _urlHelperFactory = urlHelperFactory;
        }

        public override void Process(TagHelperContext context, TagHelperOutput output)
        {
            IUrlHelper urlHelper = _urlHelperFactory.GetUrlHelper(ViewContext);
            output.TagName = "a";
            string url = urlHelper.Action(Action, new {sortOrder = Property});
            output.Attributes.SetAttribute("href", url);

            if(Current == Property)
            {
                TagBuilder tag = new TagBuilder("i");
                tag.AddCssClass("glyphicon");

                if(Up == true) tag.AddCssClass("glyphicon-chevron-up");
                else tag.AddCssClass("glyphicon-chevron-down");

                output.PreContent.AppendHtml(tag);
            }
        }


    }
}