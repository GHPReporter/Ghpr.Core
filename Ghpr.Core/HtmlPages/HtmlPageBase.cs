using System.Collections.Generic;
using System.IO;
using System.Web.UI;
using Ghpr.Core.Extensions;
using Ghpr.Core.Extensions.HtmlTextWriterExtensions;
using Ghpr.Core.Extensions.HtmlTextWriterExtensions.Styles;
using Ghpr.Core.Extensions.HtmlTextWriterExtensions.Tags;
using Ghpr.Core.Interfaces;

namespace Ghpr.Core.HtmlPages
{
    public class HtmlPageBase : IHtmlPage
    {
        public string FullPage =>  
            HtmlBuilder.Build(wr => wr
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
                    .Class("border-bottom p-3 mb-3")
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

        public string PageTitle { get; set; }
        public string PageBodyCode { get; set; }
        public string PageScriptString { get; set; }
        public string PageFooterCode { get; set; }
        public List<string> PageStylePaths { get; set; }
        public List<string> ScriptFilePaths { get; set; }

        public HtmlPageBase(string pageTitle, string pageBody = "", string pageScript = "", string pageFooter = "")
        {
            PageTitle = pageTitle;
            PageBodyCode = pageBody;
            PageScriptString = pageScript;
            PageFooterCode = pageFooter;
            PageStylePaths = new List<string>();
            ScriptFilePaths = new List<string>();
        }

        public void SavePage(string path, string name = "")
        {
            Directory.CreateDirectory(path);
            File.WriteAllText(Path.Combine(path, name.Equals("") ? "index.html" : name), FullPage);
        }
    }
}
