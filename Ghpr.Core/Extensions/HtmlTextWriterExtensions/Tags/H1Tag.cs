using System.Web.UI;

namespace Ghpr.Core.Extensions.HtmlTextWriterExtensions.Tags
{
    public static class H1Tag
    {
        public static HtmlTextWriter H1(this HtmlTextWriter writer, string value)
        {
            return writer.Tag(HtmlTextWriterTag.H1, value);
        }
    }
}
