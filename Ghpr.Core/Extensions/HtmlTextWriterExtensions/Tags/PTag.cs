using System.Web.UI;

namespace Ghpr.Core.Extensions.HtmlTextWriterExtensions.Tags
{
    public static class PTag
    {
        public static HtmlTextWriter P(this HtmlTextWriter writer, string value)
        {
            return writer.Tag(HtmlTextWriterTag.P, value);
        }
    }
}
