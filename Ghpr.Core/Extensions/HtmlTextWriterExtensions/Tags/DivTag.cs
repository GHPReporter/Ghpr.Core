using System;
using System.Web.UI;
using Ghpr.Core.Extensions.HtmlTextWriterExtensions.Styles;

namespace Ghpr.Core.Extensions.HtmlTextWriterExtensions.Tags
{
    public static class DivTag
    {
        public static HtmlTextWriter Div(this HtmlTextWriter writer, Action someAction)
        {
            return writer
                .Tag(HtmlTextWriterTag.Div, someAction);
        }

        public static HtmlTextWriter TogglableDiv(this HtmlTextWriter writer, string id, bool isDisplayed, Action someAction)
        {
            return writer
                .Id(id)
                .Class("togglable-div")
                .If(!isDisplayed, () => writer
                    .Display("none"))
                .Tag(HtmlTextWriterTag.Div, someAction);
        }

        public static HtmlTextWriter Container(this HtmlTextWriter writer, Action someAction)
        {
            return writer
                .Class("container")
                .Tag(HtmlTextWriterTag.Div, someAction);
        }

        public static HtmlTextWriter Columns(this HtmlTextWriter writer, Action someAction)
        {
            return writer
                .Class("columns")
                .Tag(HtmlTextWriterTag.Div, someAction);
        }

        public static HtmlTextWriter OneThirdColumn(this HtmlTextWriter writer, Action someAction)
        {
            return writer
                .Class("one-third column")
                .Tag(HtmlTextWriterTag.Div, someAction);
        }

        public static HtmlTextWriter TwoThirdsColumn(this HtmlTextWriter writer, Action someAction)
        {
            return writer
                .Class("two-thirds column")
                .Tag(HtmlTextWriterTag.Div, someAction);
        }

        public static HtmlTextWriter TabNav(this HtmlTextWriter writer, Action someAction)
        {
            return writer
                .Class("tabnav")
                .Tag(HtmlTextWriterTag.Div, someAction);
        }
    }
}
