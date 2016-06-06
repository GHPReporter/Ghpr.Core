using System;
using System.Web.UI;
using Ghpr.Core.Extensions.HtmlTextWriterExtensions.Tags;

namespace Ghpr.Core.Extensions.HtmlTextWriterExtensions.ReportSections
{
    public static class ReportTitle
    {
        public static HtmlTextWriter GhprTitle(this HtmlTextWriter writer, Action someAction)
        {
            return writer
                .Tag(HtmlTextWriterTag.Div, someAction);
        }
    }
}