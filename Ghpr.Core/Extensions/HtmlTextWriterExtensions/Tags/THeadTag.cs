// ReSharper disable InconsistentNaming

using System;
using System.Web.UI;

namespace Ghpr.Core.Extensions.HtmlTextWriterExtensions.Tags
{
    public static class THeadTag
    {
        public static HtmlTextWriter THead(this HtmlTextWriter writer, Action someAction)
        {
            return writer.Tag(HtmlTextWriterTag.Thead, someAction);
        }
    }
}
