using System;
using System.IO;
using System.IO.Packaging;
using System.Windows;
using System.Windows.Controls;
using System.Windows.Documents;
using System.Windows.Media;
using System.Windows.Xps.Packaging;

namespace ChessTourManager.WPF.Helpers.FileHelpers;

public static class PrintMethods
{
    public static void ShowPrintDataGridPreview(DataGrid dataGrid)
    {
        PrintDialog printDialog = new();

        FlowDocument document = new()
                                {
                                    ColumnWidth           = printDialog.PrintableAreaWidth,
                                    IsColumnWidthFlexible = true,
                                    TextAlignment         = TextAlignment.Justify,
                                    IsHyphenationEnabled  = true
                                };

        Table table = new()
                      {
                          CellSpacing     = 0,
                          BorderBrush     = Brushes.Gray,
                          BorderThickness = new Thickness(1),
                          FontStyle       = FontStyles.Normal,
                          FontWeight      = FontWeights.Normal,
                          TextAlignment   = TextAlignment.Center,
                          FontFamily      = new FontFamily("Tahoma"),
                          FontSize        = 12,
                          Padding         = new Thickness(0),
                          Margin          = new Thickness(0)
                      };

        table.RowGroups.Add(ConfigHeader(dataGrid, table));
        table.RowGroups.Add(ConfigBody(dataGrid));
        document.Blocks.Add(table);
        document.TextAlignment = TextAlignment.Center;

        // Album landscape orientation.
        document.PageWidth  = printDialog.PrintableAreaHeight;
        document.PageHeight = printDialog.PrintableAreaWidth;

        FixedDocument fixedDoc = FlowToFixedDoc(document);

        PrintPreview preview = new() { DataContext = fixedDoc };
        preview.ShowDialog();
    }

    private static FixedDocument FlowToFixedDoc(FlowDocument flowDocument)
    {
        DocumentPaginator? paginator = ((IDocumentPaginatorSource)flowDocument).DocumentPaginator;
        Package            package   = Package.Open(new MemoryStream(), FileMode.Create, FileAccess.ReadWrite);
        Uri                packUri   = new("pack://temp.xps");

        PackageStore.RemovePackage(packUri);
        PackageStore.AddPackage(packUri, package);

        using XpsDocument xps = new(package, CompressionOption.NotCompressed, packUri.ToString());
        XpsDocument.CreateXpsDocumentWriter(xps).Write(paginator);
        FixedDocument doc = xps.GetFixedDocumentSequence().References[0].GetDocument(true) ?? new FixedDocument();

        return doc;
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

    private static TableRowGroup ConfigHeader(DataGrid dataGrid, Table table)
    {
        TableRowGroup headerGroup = new();
        TableRow      headerRow   = new();

        // Number column.
        AddHeader(table, headerRow, "№", 40);

        foreach (DataGridColumn column in dataGrid.Columns)
        {
            if (column.Header is null)
            {
                continue;
            }

            AddHeader(table, headerRow, column.Header.ToString() ?? " ", column.ActualWidth);
        }

        headerGroup.Rows.Add(headerRow);
        return headerGroup;
    }

    private static void AddHeader(Table table, TableRow headerRow, string name, double width)
    {
        table.Columns.Add(new TableColumn
                          {
                              Width = new GridLength(width)
                          });
        headerRow.Cells.Add(GetHeaderCell(name));
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
                       Padding         = new Thickness(0),
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
}
