using Microsoft.AspNetCore.Razor.TagHelpers;

namespace ClassSchedule.TagHelpers
{
    [HtmlTargetElement("button", Attributes = "type=submit")]
    public class SubmitButtonTagHelper : TagHelper
    {
        public override void Process(TagHelperContext context, TagHelperOutput output)
        {
            output.Attributes.AppendCssClass("btn btn-dark");
        }
    }
}
