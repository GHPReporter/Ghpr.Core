using System.Collections.Generic;
using System.IO;
using System.Web.UI;
using Ghpr.Core.Extensions;
using Ghpr.Core.Extensions.HtmlTextWriterExtensions;
using Ghpr.Core.Extensions.HtmlTextWriterExtensions.Styles;
using Ghpr.Core.Extensions.HtmlTextWriterExtensions.Tags;

namespace Ghpr.Core.Html
{
    public class HtmlPage
    {
        public string FullPage { get; private set; }

        public string PageTitle;
        public string PageBodyCode = "";
        public string PageScriptString = "";
        public string PageFooterCode = "";
        public List<string> PageStylePaths = new List<string>();
        public List<string> ScriptFilePaths = new List<string>();

        public HtmlPage(string pageTitle)
        {
            PageTitle = pageTitle;
        }

        private void GeneratePageString()
        {
            FullPage = HtmlBuilder.Build(wr => wr
                .WriteString("<!DOCTYPE html>")
                .NewLine()
                .Tag(HtmlTextWriterTag.Head, () => wr
                    .Tag(HtmlTextWriterTag.Meta, new Dictionary<string, string>
                    {
                        {"http-equiv", "X-UA-Compatible"},
                        {"content", @"IE=edge"},
                        {"charset", "utf-8"}
                    })
                    .Title(PageTitle)
                    .Scripts(ScriptFilePaths)
                    .TagIf(!PageScriptString.Equals(""), HtmlTextWriterTag.Script, PageScriptString)
                    .Type(@"text/css")
                    .Tag(HtmlTextWriterTag.Style)
                    .Stylesheets(PageStylePaths)

                )
                .Tag(HtmlTextWriterTag.Body, () => wr
                    .Class("border-bottom p-3 mb-3 bg-gray")
                    .Div(() => wr
                        .Container(() => wr
                            .TextAlign("center")
                            .Div(() => wr
                                .H1(PageTitle)
                            )
                        )
                    )
                    .Container(() => wr
                        .Write(PageBodyCode)
                    )
                )
                .NewLine()
                .Footer()
                .NewLine()
                .WriteString("</html>")
                .NewLine());
        }

        public void SavePage(string path, string name = "")
        {
            GeneratePageString();
            Directory.CreateDirectory(path);
            File.WriteAllText(Path.Combine(path, name.Equals("") ? "index.html" : name), FullPage);
        }
    }
}
