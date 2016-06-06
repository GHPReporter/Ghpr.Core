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
            PageScriptString = "";
            PageFooterCode = "";
            PageStylePaths = re.GetResoucrePaths(Resource.All, ".css");
            ScriptFilePaths = re.GetResoucrePaths(Resource.All, ".js");

            PageBodyCode = HtmlBuilder.Build(wr => wr
                .H1("Test page :)"));

        }
    }
}