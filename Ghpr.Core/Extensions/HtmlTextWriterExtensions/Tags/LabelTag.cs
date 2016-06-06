using System.Web.UI;

namespace Ghpr.Core.Extensions.HtmlTextWriterExtensions.Tags
{
    public static class LabelTag
    {
        public static HtmlTextWriter Label(this HtmlTextWriter writer, string value)
        {
            return writer.Tag(HtmlTextWriterTag.Label, value);
        }
    }
}
