using System;
using System.Web.UI;

namespace Ghpr.Core.Extensions.HtmlTextWriterExtensions.Tags
{
    public static class TdTag
    {
        public static HtmlTextWriter Td(this HtmlTextWriter writer, Action someAction)
        {
            return writer.Tag(HtmlTextWriterTag.Td, someAction);
        }

        public static HtmlTextWriter Td(this HtmlTextWriter writer, string value)
        {
            return writer.Tag(HtmlTextWriterTag.Td, value);
        }
    }
}
