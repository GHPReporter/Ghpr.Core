// ReSharper disable InconsistentNaming

using System;
using System.Web.UI;

namespace Ghpr.Core.Extensions.HtmlTextWriterExtensions.Tags
{
    public static class TBodyTag
    {
        public static HtmlTextWriter TBody(this HtmlTextWriter writer, Action someAction)
        {
            return writer.Tag(HtmlTextWriterTag.Tbody, someAction);
        }
    }
}
