using System;
using System.Collections.Generic;
using System.Web.UI;

namespace Ghpr.Core.Extensions.HtmlTextWriterExtensions.Tags
{
    public static class TableTag
    {
        public static HtmlTextWriter Table(this HtmlTextWriter writer, Action someAction)
        {
            return writer.Tag(HtmlTextWriterTag.Table, someAction);
        }

        public static HtmlTextWriter SortableTable(this HtmlTextWriter writer, Action someAction)
        {
            var tableId = Guid.NewGuid().ToString();
            return writer
                .Id(tableId)
                .Tag(HtmlTextWriterTag.Table, someAction)
                .Script($"new Tablesort(document.getElementById('{tableId}'));");
        }

        public static HtmlTextWriter SortableTable(this HtmlTextWriter writer, List<string> headers, List<List<string>> rows)
        {
            return writer
                .SortableTable(() => writer
                    .THead(() => writer
                        .Tr(() => writer
                            .ForEach(headers, header => writer
                                .Th(header)
                            )
                        )
                    )
                    .TBody(() => writer
                        .ForEach(rows, row => writer
                            .Tr(() => writer
                                .ForEach(row, dataItem => writer
                                    .Td(dataItem)
                                )
                            )
                        )
                    )
                );
        }

        public static HtmlTextWriter SortableTable(this HtmlTextWriter writer, string[] headers, params string[][] rows)
        {
            return writer
                .SortableTable(() => writer
                    .THead(() => writer
                        .Tr(() => writer
                            .ForEach(headers, header => writer
                                .Th(header)
                            )
                        )
                    )
                    .TBody(() => writer
                        .ForEach(rows, row => writer
                            .Tr(() => writer
                                .ForEach(row, dataItem => writer
                                    .Td(dataItem)
                                )
                            )
                        )
                    )
                );
        }
    }
}
