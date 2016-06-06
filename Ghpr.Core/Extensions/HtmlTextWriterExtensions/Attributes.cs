using System.Web.UI;

namespace Ghpr.Core.Extensions.HtmlTextWriterExtensions
{
    public static class Attributes
    {
        public static HtmlTextWriter WithAttr(this HtmlTextWriter writer, HtmlTextWriterAttribute attr, string value)
        {
            writer.AddAttribute(attr, value);
            return writer;
        }

        public static HtmlTextWriter Id(this HtmlTextWriter writer, string value)
        {
            writer.AddAttribute(HtmlTextWriterAttribute.Id, value);
            return writer;
        }

        public static HtmlTextWriter Href(this HtmlTextWriter writer, string value)
        {
            writer.AddAttribute(HtmlTextWriterAttribute.Href, value);
            return writer;
        }

        public static HtmlTextWriter OnClick(this HtmlTextWriter writer, string value)
        {
            writer.AddAttribute(HtmlTextWriterAttribute.Onclick, value);
            return writer;
        }

        public static HtmlTextWriter Class(this HtmlTextWriter writer, string value)
        {
            writer.AddAttribute(HtmlTextWriterAttribute.Class, value);
            return writer;
        }

        public static HtmlTextWriter Type(this HtmlTextWriter writer, string value)
        {
            writer.AddAttribute(HtmlTextWriterAttribute.Type, value);
            return writer;
        }

        public static HtmlTextWriter Src(this HtmlTextWriter writer, string value)
        {
            writer.AddAttribute(HtmlTextWriterAttribute.Src, value);
            return writer;
        }

        public static HtmlTextWriter WithAttr(this HtmlTextWriter writer, string attr, string value)
        {
            writer.AddAttribute(attr, value);
            return writer;
        }
    }
}