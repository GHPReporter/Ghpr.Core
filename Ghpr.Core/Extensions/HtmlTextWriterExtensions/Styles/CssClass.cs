using System.Web.UI;

namespace Ghpr.Core.Extensions.HtmlTextWriterExtensions.Styles
{
    public static class CssClass
    {
        public static HtmlTextWriter Shadow(this HtmlTextWriter writer, string value)
        {
            return writer
                .WithAttr("box-shadow", value)
                .WithAttr("-moz-box-shadow", value)
                .WithAttr("-webkit-box-shadow", value);
        }

        public static HtmlTextWriter Style(this HtmlTextWriter writer, string styleAttr, string value)
        {
            writer.AddStyleAttribute(styleAttr, value);
            return writer;
        }

        public static HtmlTextWriter Style(this HtmlTextWriter writer, HtmlTextWriterStyle styleAttr, string value)
        {
            writer.AddStyleAttribute(styleAttr, value);
            return writer;
        }

        public static HtmlTextWriter Width(this HtmlTextWriter writer, string value)
        {
            writer.AddStyleAttribute(HtmlTextWriterStyle.Width, value);
            return writer;
        }

        public static HtmlTextWriter MarginRight(this HtmlTextWriter writer, string value)
        {
            writer.AddStyleAttribute(HtmlTextWriterStyle.MarginRight, value);
            return writer;
        }

        public static HtmlTextWriter Bold(this HtmlTextWriter writer)
        {
            writer.AddStyleAttribute(HtmlTextWriterStyle.FontWeight, "bold");
            return writer;
        }

        public static HtmlTextWriter Color(this HtmlTextWriter writer, string value)
        {
            writer.AddStyleAttribute("color", value);
            return writer;
        }

        public static HtmlTextWriter BackgroundColor(this HtmlTextWriter writer, string value)
        {
            writer.AddStyleAttribute("background-color", value);
            return writer;
        }

        public static HtmlTextWriter Float(this HtmlTextWriter writer, string value)
        {
            writer.AddStyleAttribute("float", value);
            return writer;
        }

        public static HtmlTextWriter TextAlign(this HtmlTextWriter writer, string value)
        {
            writer.AddStyleAttribute("text-align", value);
            return writer;
        }

        public static HtmlTextWriter Display(this HtmlTextWriter writer, string value)
        {
            writer.AddStyleAttribute(HtmlTextWriterStyle.Display, value);
            return writer;
        }
    }
}