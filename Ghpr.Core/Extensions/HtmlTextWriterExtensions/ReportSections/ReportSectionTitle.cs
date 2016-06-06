using System.Web.UI;
using Ghpr.Core.Extensions.HtmlTextWriterExtensions.Tags;

namespace Ghpr.Core.Extensions.HtmlTextWriterExtensions.ReportSections
{
    public static class ReportSectionTitle
    {
        public static HtmlTextWriter GhprSectionTitle(this HtmlTextWriter writer, string title)
        {
            return writer
                .Class("border-bottom p-3 mb-3")
                .Div(() => writer
                    .H1(title)
                );
        }
    }
}