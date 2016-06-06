using System.Web.UI;

namespace Ghpr.Core.Extensions.HtmlTextWriterExtensions.Tags
{
    public static class TitleTag
    {
        public static HtmlTextWriter Title(this HtmlTextWriter writer, string value)
        {
            return writer.Tag(HtmlTextWriterTag.Title, value);
        }
    }
}