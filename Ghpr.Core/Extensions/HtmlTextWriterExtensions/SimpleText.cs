using System;
using System.Web.UI;

namespace Ghpr.Core.Extensions.HtmlTextWriterExtensions
{
    public static class SimpleText
    {
        public static HtmlTextWriter WriteString(this HtmlTextWriter writer, string value = "")
        {
            writer.Write(value != "" ? value : Environment.NewLine);
            return writer;
        }

        public static HtmlTextWriter NewLine(this HtmlTextWriter writer)
        {
            writer.Write(Environment.NewLine);
            return writer;
        }

        public static HtmlTextWriter Text(this HtmlTextWriter writer, string value)
        {
            writer.Write(value);
            return writer;
        }
    }
}