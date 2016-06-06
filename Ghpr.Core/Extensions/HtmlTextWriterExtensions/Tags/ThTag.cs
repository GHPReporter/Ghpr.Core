using System;
using System.Web.UI;

namespace Ghpr.Core.Extensions.HtmlTextWriterExtensions.Tags
{
    public static class ThTag
    {
        public static HtmlTextWriter Th(this HtmlTextWriter writer, Action someAction)
        {
            return writer.Tag(HtmlTextWriterTag.Th, someAction);
        }

        public static HtmlTextWriter Th(this HtmlTextWriter writer, string value, bool sortable = true)
        {
            return writer
                .If(!sortable, () => writer.Class("no-sort"))
                .Tag(HtmlTextWriterTag.Th, value);
        }
    }
}
