using System.Web.UI;
using Ghpr.Core.Extensions.HtmlTextWriterExtensions.Tags;

namespace Ghpr.Core.Extensions.HtmlTextWriterExtensions.Buttons
{
    public static class Buttons
    {
        public static HtmlTextWriter DangerButton(this HtmlTextWriter writer, string text, string href)
        {
            writer
                .Class("btn btn-danger")
                .Href(href)
                .Type("button")
                .A(text);
            return writer;
        }

        public static HtmlTextWriter ViewButton(this HtmlTextWriter writer, string text, string href)
        {
            writer
                .Class("btn btn-sm")
                .Href(href)
                .Type("button")
                .OpenTag(HtmlTextWriterTag.A)
                .Class("octicon octicon-eye")
                .Tag(HtmlTextWriterTag.Span)
                .Text("  " + text)
                .CloseTag();//A
            return writer;
        }

        public static HtmlTextWriter ShowHideButton(this HtmlTextWriter writer, string text, string href, string octicon = "octicon octicon-flame")
        {
            writer
                .Class("btn btn-sm")
                .Href(href)
                .Type("button")
                .OnClick("$($(this).attr(\'href\')).toggle();")
                .OpenTag(HtmlTextWriterTag.A)
                .Class(octicon)
                .Tag(HtmlTextWriterTag.Span)
                .Text("  " + text)
                .CloseTag();//A
            return writer;
        }
    }
}
