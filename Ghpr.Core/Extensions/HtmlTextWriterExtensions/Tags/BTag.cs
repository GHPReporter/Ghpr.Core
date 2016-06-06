using System.Web.UI;

namespace Ghpr.Core.Extensions.HtmlTextWriterExtensions.Tags
{
    public static class BTag
    {
        public static HtmlTextWriter B(this HtmlTextWriter writer, string value)
        {
            return writer.Tag(HtmlTextWriterTag.B, value);
        }
    }
}
