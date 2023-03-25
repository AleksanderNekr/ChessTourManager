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
                                    ColumnWidth = printDialog.PrintableAreaWidth
                                };

        Table table = new()
                      {
                          CellSpacing     = 0,
                          BorderBrush     = Brushes.Gray,
                          BorderThickness = new Thickness(1),
                          FontStyle       = FontStyles.Normal,
                          FontWeight      = FontWeights.Normal,
                          TextAlignment   = TextAlignment.Left
                      };

        ConfigHeader(dataGrid, table);

        ConfigBody(dataGrid, table, document);

        printDialog.PrintDocument(((IDocumentPaginatorSource)document).DocumentPaginator, "Print DataGrid");
        return true;
    }

    private static void ConfigBody(DataGrid dataGrid, Table table, FlowDocument document)
    {
        TableRowGroup dataGroup = new();

        foreach (object? item in dataGrid.Items)
        {
            TableRow dataRow = new();

            foreach (DataGridColumn column in dataGrid.Columns)
            {
                string cellValue = GetCellValue(column, item);

                TableCell dataCell = new(new Paragraph(new Run(cellValue)))
                                     {
                                         BorderBrush     = Brushes.Gray,
                                         BorderThickness = new Thickness(1),
                                         Padding         = new Thickness(4)
                                     };
                dataRow.Cells.Add(dataCell);
            }

            dataGroup.Rows.Add(dataRow);
        }

        table.RowGroups.Add(dataGroup);
        document.Blocks.Add(table);
    }

    private static string GetCellValue(DataGridColumn column, object item)
    {
        FrameworkElement cellContent = column.GetCellContent(item)
                                    ?? throw new InvalidOperationException();

        return cellContent switch
               {
                   TextBlock textBlock => textBlock.Text,
                   CheckBox checkBox => checkBox.IsChecked == true
                                            ? "Да"
                                            : "Нет",
                   ComboBox comboBox     => comboBox.Text,
                   DatePicker datePicker => datePicker.Text,
                   TextBox textBox       => textBox.Text,
                   _                     => string.Empty
               };
    }

    private static void ConfigHeader(DataGrid dataGrid, Table table)
    {
        TableRowGroup headerGroup = new();
        TableRow      headerRow   = new();

        foreach (DataGridColumn column in dataGrid.Columns)
        {
            TableCell headerCell = new(new Paragraph(new Run(column.Header.ToString())))
                                   {
                                       Background      = Brushes.LightGray,
                                       BorderBrush     = Brushes.Gray,
                                       BorderThickness = new Thickness(1),
                                       Padding         = new Thickness(4),
                                       FontStyle       = FontStyles.Normal,
                                       FontWeight      = FontWeights.Bold
                                   };
            headerRow.Cells.Add(headerCell);
        }

        headerGroup.Rows.Add(headerRow);
        table.RowGroups.Add(headerGroup);
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
