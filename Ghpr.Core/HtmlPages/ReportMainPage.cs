using System.Collections.Generic;
using Ghpr.Core.EmbeddedResources;
using Ghpr.Core.Enums;
using Ghpr.Core.Extensions;
using Ghpr.Core.Extensions.HtmlTextWriterExtensions.Tags;

namespace Ghpr.Core.HtmlPages
{
    public class ReportMainPage : HtmlPageBase
    {
        public ReportMainPage(string srcPath = "") : base("GHP Report Main Page")
        {
            var re = new ResourceExtractor("", srcPath);
            PageBodyCode = HtmlBuilder.Build(wr => wr
                .H1("Test page :)"));

            PageScriptString = "";
            PageFooterCode = "";
            PageStylePaths = new List<string>();
            ScriptFilePaths = re.GetResoucrePaths(Resource.All);
        }
    }
}