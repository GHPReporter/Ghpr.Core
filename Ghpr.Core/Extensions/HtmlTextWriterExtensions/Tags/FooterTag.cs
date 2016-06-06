using System;
using System.Web.UI;
using Ghpr.Core.Extensions.HtmlTextWriterExtensions.Styles;

namespace Ghpr.Core.Extensions.HtmlTextWriterExtensions.Tags
{
    public static class FooterTag
    {
        public static HtmlTextWriter FooterWithText(this HtmlTextWriter writer, string value)
        {
            return writer.Tag("footer", value);
        }

        public static HtmlTextWriter Footer(this HtmlTextWriter writer, Action action)
        {
            writer.OpenTag("footer");
            action.Invoke();
            return writer.CloseTag();
        }

        public static HtmlTextWriter Footer(this HtmlTextWriter writer, string footerText = "")
        {
            return writer
                .Footer(() => writer
                    .Style(HtmlTextWriterStyle.TextAlign, "center")
                    .Class("border border-0 p-3 mb-3")
                    .Tag(HtmlTextWriterTag.Div, () => writer
                        .Style(HtmlTextWriterStyle.Position, "relative")
                        .Div(() => writer
                            .Text(footerText.Equals("")
                                ? "Copyright 2015-2016 " + '\u00a9' + " GhprWeb"
                                : footerText)
                        )
                    )
                );
        }
    }
}
