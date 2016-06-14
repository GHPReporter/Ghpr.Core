using System;
using System.IO;
using System.Web.UI;

namespace Ghpr.Core.Extensions.HtmlTextWriterExtensions
{
    public static class HtmlBuilder
    {
        public static string Build(Action<HtmlTextWriter> action)
        {
            var strWr = new StringWriter();
            using (var writer = new HtmlTextWriter(strWr))
            {
                action(writer);
            }
            return strWr.ToString();
        }
    }
}
