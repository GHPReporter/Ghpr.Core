using System;
using System.Web.UI;

namespace Ghpr.Core.Extensions.HtmlTextWriterExtensions.Tags
{
    public static class LiTag
    {
        public static HtmlTextWriter Li(this HtmlTextWriter writer, string value)
        {
            return writer.Tag(HtmlTextWriterTag.Li, value);
        }

        public static HtmlTextWriter Li(this HtmlTextWriter writer, Action someAction)
        {
            return writer.Tag(HtmlTextWriterTag.Li, someAction);
        }
    }
}
