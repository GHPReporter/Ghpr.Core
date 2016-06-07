using System.Web.UI;
using Ghpr.Core.Extensions.HtmlTextWriterExtensions.Tags;
using Ghpr.Core.Utils;

namespace Ghpr.Core.Extensions.HtmlTextWriterExtensions.ReportSections
{
    public static class MainResults
    {
        public static HtmlTextWriter GhprMainResults(this HtmlTextWriter writer, string id)
        {
            return writer
                .Class("columns")
                    .Id(id)
                    .Div(() => writer
                        .Class("one-third column")
                        .Div(() => writer
                            .Div(() => writer
                                .Class("border-bottom p-3 mb-3")
                                .H2("Time: ")
                                .Class("border border-0 p-3 mb-3")
                                .Div(() => writer
                                    .Ul(() => writer
                                        .Id(Ids.MainResults.StartDateTime).Li("Start datetime: ")
                                        .Id(Ids.MainResults.FinishDateTime).Li("Finish datetime: ")
                                        .Id(Ids.MainResults.Duration).Li("Duration: ")
                                    )
                                )
                            )
                            .Div(() => writer
                                .Class("border-bottom p-3 mb-3")
                                .H2("Summary: ")
                                .Class("border border-0 p-3 mb-3")
                                .Div(() => writer
                                    .Ul(() => writer
                                        .Id(Ids.MainResults.TotalAll).Li("Total: ")
                                        .Id(Ids.MainResults.TotalPassed).Li("Success: ")
                                        .Id(Ids.MainResults.TotalBroken).Li("Errors: ")
                                        .Id(Ids.MainResults.TotalFailed).Li("Failures: ")
                                        .Id(Ids.MainResults.TotalInconclusive).Li("Inconclusive: ")
                                        .Id(Ids.MainResults.TotalIgnored).Li("Ignored: ")
                                    )
                                )
                            )
                        )
                        .Class("two-thirds column")
                        .Div(() => writer
                            .Id(Ids.MainResults.PieChart)
                            .Div()
                        )
                    );
        }
    }
}