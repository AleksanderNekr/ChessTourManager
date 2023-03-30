using System;
using System.Printing;
using System.Windows;
using System.Windows.Controls;
using System.Windows.Documents;
using System.Windows.Media;

namespace ChessTourManager.WPF.Helpers.FileHelpers;

public static class PrintTableMethods
{
    public static bool TryPrintDataGrid(DataGrid dataGrid)
    {
        PrintDialog printDialog = new();

        if (printDialog.ShowDialog() == false)
        {
            return false;
        }

        ConfigPage(printDialog);

        FlowDocument document = new()
                                {
                                    ColumnWidth           = printDialog.PrintableAreaWidth,
                                    IsColumnWidthFlexible = true
                                };

        Table table = new()
                      {
                          CellSpacing     = 0,
                          BorderBrush     = Brushes.Gray,
                          BorderThickness = new Thickness(1),
                          FontStyle       = FontStyles.Normal,
                          FontWeight      = FontWeights.Normal,
                          TextAlignment   = TextAlignment.Center,
                          FontFamily      = new FontFamily("Tahoma")
                      };


        table.RowGroups.Add(ConfigHeader(dataGrid));
        table.RowGroups.Add(ConfigBody(dataGrid));
        document.Blocks.Add(table);

        printDialog.PrintDocument(((IDocumentPaginatorSource)document).DocumentPaginator, "Print data");
        return true;
    }

    private static void PrintRow(DataGrid dataGrid, object item, TableRow dataRow)
    {
        // Row number column specified width.
        dataRow.Cells.Add(GetDataCell((dataGrid.Items.IndexOf(item) + 1).ToString()));

        foreach (DataGridColumn column in dataGrid.Columns)
        {
            string cellValue = GetCellValue(column, item);

            if (cellValue == string.Empty)
            {
                continue;
            }

            dataRow.Cells.Add(GetDataCell(cellValue));
        }
    }

    private static TableCell GetDataCell(string cellValue)
    {
        return new TableCell(new Paragraph(new Run(cellValue)))
               {
                   BorderBrush     = Brushes.Gray,
                   BorderThickness = new Thickness(1),
                   Padding         = new Thickness(1),
                   TextAlignment   = TextAlignment.Center,
                   FontStyle       = FontStyles.Normal,
                   FontWeight      = FontWeights.Normal,
                   FontFamily      = new FontFamily("Tahoma")
               };
    }

    private static string GetCellValue(DataGridColumn column, object item)
    {
        if (column.SortMemberPath == string.Empty)
        {
            return string.Empty;
        }

        var s = GetPropertyValuesMethods.GetPropertyValue(item, column.SortMemberPath)?.ToString();
        return s ?? " ";
    }

    private static TableRowGroup ConfigHeader(DataGrid dataGrid)
    {
        TableRowGroup headerGroup = new();
        TableRow      headerRow   = new();

        // Number column.
        headerRow.Cells.Add(GetHeaderCell("№"));

        foreach (DataGridColumn column in dataGrid.Columns)
        {
            if (column.Header is null)
            {
                continue;
            }

            headerRow.Cells.Add(GetHeaderCell(column.Header.ToString() ?? string.Empty));
        }

        headerGroup.Rows.Add(headerRow);
        return headerGroup;
    }

    private static TableCell GetHeaderCell(string header)
    {
        var run  = new Run(header);
        var para = new Paragraph(run);
        var cell = new TableCell(para)
                   {
                       Background      = Brushes.LightGray,
                       BorderBrush     = Brushes.Gray,
                       BorderThickness = new Thickness(1),
                       Padding         = new Thickness(1),
                       FontStyle       = FontStyles.Normal,
                       FontWeight      = FontWeights.Bold
                   };

        return cell;
    }

    private static TableRowGroup ConfigBody(DataGrid dataGrid)
    {
        TableRowGroup dataGroup = new();

        foreach (object? item in dataGrid.Items)
        {
            TableRow dataRow = new();

            PrintRow(dataGrid, item, dataRow);

            dataGroup.Rows.Add(dataRow);
        }

        return dataGroup;
    }

    private static void ConfigPage(PrintDialog printDialog)
    {
        printDialog.PrintTicket.PageOrientation   = PageOrientation.Portrait;
        printDialog.PrintTicket.PageBorderless    = PageBorderless.None;
        printDialog.PrintTicket.PageMediaSize     = new PageMediaSize(PageMediaSizeName.ISOA4);
        printDialog.PrintTicket.PageResolution    = new PageResolution(300, 300);
        printDialog.PrintTicket.PageScalingFactor = 100;
    }
}
