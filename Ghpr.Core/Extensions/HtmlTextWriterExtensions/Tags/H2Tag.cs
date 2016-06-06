using System.Web.UI;

namespace Ghpr.Core.Extensions.HtmlTextWriterExtensions.Tags
{
    public static class H2Tag
    {
        public static HtmlTextWriter H2(this HtmlTextWriter writer, string value)
        {
            return writer.Tag(HtmlTextWriterTag.H2, value);
        }
    }
}
