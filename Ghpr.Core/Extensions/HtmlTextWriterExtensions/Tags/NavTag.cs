using System;
using System.Web.UI;

namespace Ghpr.Core.Extensions.HtmlTextWriterExtensions.Tags
{
    public static class NavTag
    {
        public static HtmlTextWriter Nav(this HtmlTextWriter writer, Action someAction)
        {
            return writer
                .Tag("nav", someAction);
        }

        public static HtmlTextWriter Menu(this HtmlTextWriter writer, Action someAction)
        {
            return writer
                .Class("menu")
                .Nav(someAction);
        }

        public static HtmlTextWriter Menu(this HtmlTextWriter writer, string menuHeading, Action someAction)
        {
            return writer
                .Class("menu")
                .Nav(() => writer
                    .Class("menu-heading")
                    .Span(menuHeading)
                    .DoAction(someAction)
                );
        }

        public static HtmlTextWriter Menu(this HtmlTextWriter writer, params string[][] menuItems)
        {
            return writer
                .Class("menu")
                .Nav(() => writer
                    .ForEach(menuItems, menuItem => writer
                        .MenuItem(menuItem[0], menuItem[1] ?? "", menuItem[2] ?? "")
                    )
                );
        }

        public static HtmlTextWriter Menu(this HtmlTextWriter writer, string menuHeading, params string[][] menuItems)
        {
            return writer
                .Class("menu")
                .Nav(() => writer
                    .Class("menu-heading")
                    .Span(menuHeading)
                    .ForEach(menuItems, menuItem => writer
                        .MenuItem(menuItem[0], menuItem[1] ?? "", menuItem[2] ?? "")
                    )
                );
        }

        public static HtmlTextWriter TabNavTabs(this HtmlTextWriter writer, Action someAction)
        {
            return writer
                .Class("tabnav-tabs")
                .Tag("nav", someAction);
        }
    }
}
