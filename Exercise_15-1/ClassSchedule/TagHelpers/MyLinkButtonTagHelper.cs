using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.Rendering;
using Microsoft.AspNetCore.Mvc.Routing;
using Microsoft.AspNetCore.Mvc.ViewFeatures;
using Microsoft.AspNetCore.Razor.TagHelpers;

namespace ClassSchedule.TagHelpers
{
    [HtmlTargetElement("my-link-button")]
    public class MyLinkButtonTagHelper : TagHelper
    {
        public string? Action { get; set; }
        public string? Controller { get; set; }
        public int Id { get; set; }

        [ViewContext]
        [HtmlAttributeNotBound]
        public ViewContext ViewContext { get; set; } = null!;

        private LinkGenerator linkGenerator;
        public MyLinkButtonTagHelper(LinkGenerator generator)
        {
            linkGenerator = generator;
        }

        public override void Process(TagHelperContext context, TagHelperOutput output)
        {
            string action = Action ?? ViewContext.RouteData.Values["action"]?.ToString() ?? "";
            string controller = Controller ?? ViewContext.RouteData.Values["controller"]?.ToString() ?? "";
            object id = new { id = Id };

            string url = linkGenerator.GetPathByAction(action, controller, id) ?? "#";

            string? currentId = ViewContext.RouteData.Values["id"]?.ToString();
            string css = (currentId == Id.ToString()) ?
                "btn btn-dark" : "btn btn-outline-dark";

            output.BuildLink(url, css);
        }
    }
}
