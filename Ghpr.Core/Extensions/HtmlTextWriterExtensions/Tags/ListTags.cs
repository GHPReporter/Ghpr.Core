using System;
using System.Web.UI;
using Ghpr.Core.Extensions.HtmlTextWriterExtensions.Styles;

namespace Ghpr.Core.Extensions.HtmlTextWriterExtensions.Tags
{
    public static class ListTags
    {
        public static HtmlTextWriter OpenTreeItem(this HtmlTextWriter writer, string name)
        {
            writer.RenderBeginTag(HtmlTextWriterTag.Ul);
            writer.AddStyleAttribute(HtmlTextWriterStyle.FontWeight, "bold");
            writer.RenderBeginTag(HtmlTextWriterTag.Label);
            writer.Write(name);
            writer.RenderEndTag(); //LABEL
            return writer;
        }

        public static HtmlTextWriter CloseTreeItem(this HtmlTextWriter writer)
        {
            writer.RenderEndTag();//UL
            return writer;
        }

        public static HtmlTextWriter TreeItem(this HtmlTextWriter writer, string name, Action action)
        {
            writer.Ul(() => writer
                .Bold()
                .Label(name)
                .DoAction(action)
            );
            return writer;
        }
    }
}