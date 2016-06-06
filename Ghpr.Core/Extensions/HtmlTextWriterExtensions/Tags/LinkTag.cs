using System.Collections.Generic;
using System.Web.UI;

namespace Ghpr.Core.Extensions.HtmlTextWriterExtensions.Tags
{
    public static class LinkTag
    {
        public static HtmlTextWriter Stylesheets(this HtmlTextWriter writer, List<string> pathsToCss)
        {
            foreach (var path in pathsToCss)
            {
                writer.Tag(HtmlTextWriterTag.Link, new Dictionary<HtmlTextWriterAttribute, string>
                {
                    {HtmlTextWriterAttribute.Rel, @"stylesheet"},
                    {HtmlTextWriterAttribute.Type, @"text/css"},
                    {HtmlTextWriterAttribute.Href, path}
                });
            }
            return writer;
        }
    }
}
