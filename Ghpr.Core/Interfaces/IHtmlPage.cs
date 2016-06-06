using System.Collections.Generic;

namespace Ghpr.Core.Interfaces
{
    public interface IHtmlPage
    {
        string FullPage { get; }

        string PageTitle { get; set; }
        string PageBodyCode { get; set; }
        string PageScriptString { get; set; }
        string PageFooterCode { get; set; }
        List<string> PageStylePaths { get; set; }
        List<string> ScriptFilePaths { get; set; }
        
        void SavePage(string path, string name = "");
    }
}