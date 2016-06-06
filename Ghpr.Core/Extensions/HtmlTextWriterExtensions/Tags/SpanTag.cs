using System;
using System.Web.UI;

namespace Ghpr.Core.Extensions.HtmlTextWriterExtensions.Tags
{
    public static class SpanTag
    {
        public static HtmlTextWriter Span(this HtmlTextWriter writer)
        {
            return writer.OpenTag(HtmlTextWriterTag.Span).CloseTag();
        }

        public static HtmlTextWriter Span(this HtmlTextWriter writer, string value)
        {
            writer
                .OpenTag(HtmlTextWriterTag.Span)
                .Text(value)
                .CloseTag();
            return writer;
        }

        public static HtmlTextWriter Span(this HtmlTextWriter writer, Action someAction)
        {
            return writer.Tag(HtmlTextWriterTag.Span, someAction);
        }

        public static HtmlTextWriter TooltippedSpan(this HtmlTextWriter writer, string tooltipText, string text)
        {
            writer
                .Class("tooltipped tooltipped-n")
                .WithAttr("aria-label", tooltipText)
                .Span(text);
            return writer;
        }

        public static HtmlTextWriter TooltippedSpan(this HtmlTextWriter writer, string tooltipText, Action action)
        {
            writer
                .Class("tooltipped tooltipped-n")
                .WithAttr("aria-label", tooltipText)
                .Span(action.Invoke);
            return writer;
        }
    }
}
