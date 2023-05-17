using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Text;
using System.Windows;
using System.Windows.Controls;
using Microsoft.Win32;
using OfficeOpenXml;

namespace ChessTourManager.WPF.Helpers.FileHelpers;

public static class ExportTableMethods
{
    private const string Separator = ";";

    public static bool TryExportGrid(DataGrid dataGrid, int skipFirstColumnsCount = 0, int skipLastColumnsCount = 0)
    {
        CheckSkipColumns(dataGrid.Columns.Count, skipFirstColumnsCount, skipLastColumnsCount);

        SaveFileDialog saveDialog = ConfigSaveFileDialog(dataGrid);
        if (saveDialog.ShowDialog() == false)
        {
            return false;
        }

        string fileName  = saveDialog.FileName;
        string extension = Path.GetExtension(fileName);

        switch (extension)
        {
            case ".xlsx":
            case ".xls":
                ExportToExcel(dataGrid, fileName, skipFirstColumnsCount, skipLastColumnsCount);
                break;
            case ".csv":
                ExportToCsv(dataGrid, fileName, skipFirstColumnsCount, skipLastColumnsCount);
                break;
        }

        return true;
    }

    private static SaveFileDialog ConfigSaveFileDialog(DataGrid dataGrid)
    {
        const string fileExtensions = "XLSX files (*.xlsx)|*.xlsx|XLS files (*.xls)|*.xls|"
                                    + "CSV files (*.csv)|*.csv";
        Type   itemType = dataGrid.Items[0].GetType();
        string filename = itemType.Name + "s";

        SaveFileDialog saveDialog = new()
                                    {
                                        FileName = filename,
                                        Filter   = fileExtensions
                                    };
        return saveDialog;
    }

    private static void CheckSkipColumns(int dataGridColumnsCount, int skipFirstColumnsCount,
                                         int skipLastColumnsCount)
    {
        if (skipFirstColumnsCount < 0)
        {
            throw new ArgumentOutOfRangeException(nameof(skipFirstColumnsCount),
                                                  "skipFirstColumnsCount must be >= 0");
        }

        if (skipLastColumnsCount < 0)
        {
            throw new ArgumentOutOfRangeException(nameof(skipLastColumnsCount),
                                                  "skipLastColumnsCount must be >= 0");
        }

        if (dataGridColumnsCount <= (skipFirstColumnsCount + skipLastColumnsCount))
        {
            throw new ArgumentOutOfRangeException(nameof(skipFirstColumnsCount),
                                                  "skipFirstColumnsCount + skipLastColumnsCount"
                                                + " must be < dataGridColumnsCount");
        }
    }

    private static void WriteHeaderCsv(StringBuilder sb, DataGrid dataGrid, int skipFirstColumnsCount,
                                       int           skipLastColumnsCount)
    {
        for (int i = skipFirstColumnsCount; i < (dataGrid.Columns.Count - skipLastColumnsCount); i++)
        {
            DataGridColumn? column = dataGrid.Columns[i];
            if (column.Visibility != Visibility.Visible)
            {
                continue;
            }

            sb.Append(column.Header);
            sb.Append(Separator);
        }

        StartNewLine(sb);
    }

    private static void StartNewLine(StringBuilder stringBuilder)
    {
        // Remove the last comma.
        stringBuilder.Length -= 1;

        stringBuilder.AppendLine();
    }

    private static void ExportToCsv(DataGrid dataGrid, string fileName, int skipFirstColumnsCount,
                                    int      skipLastColumnsCount)
    {
        StringBuilder sb = new();

        WriteHeaderCsv(sb, dataGrid, skipFirstColumnsCount, skipLastColumnsCount);

        IEnumerable<object> objects = dataGrid.ItemsSource.Cast<object>();

        WriteBodyCsv(dataGrid, objects.ToList(), sb, skipFirstColumnsCount, skipLastColumnsCount);

        File.WriteAllText(fileName, sb.ToString());
    }

    private static void WriteBodyCsv(DataGrid              dataGrid,
                                     IReadOnlyList<object> data,
                                     StringBuilder         sb,
                                     int                   skipFirstColumnsCount = 0,
                                     int                   skipLastColumnsCount  = 0)
    {
        for (var i = 0; i < data.Count; i++)
        {
            object item = data[i];
            for (int j = skipFirstColumnsCount; j < (dataGrid.Columns.Count - skipLastColumnsCount); j++)
            {
                if (dataGrid.Columns[j].Visibility != Visibility.Visible)
                {
                    continue;
                }

                object? value = GetPropertyValuesMethods.GetPropertyValue(item, dataGrid.Columns[j].SortMemberPath);
                sb.Append(value);
                sb.Append(Separator);
            }

            StartNewLine(sb);
        }
    }

    private static void ExportToExcel(DataGrid dataGrid,
                                      string   fileName,
                                      int      skipFirstColumnsCount,
                                      int      skipLastColumnsCount)
    {
        IEnumerable<object> objects = dataGrid.ItemsSource.Cast<object>();

        ExcelPackage.LicenseContext = LicenseContext.NonCommercial;
        var package = new ExcelPackage();

        ExcelWorksheets? worksheets = package.Workbook.Worksheets;

        ExcelWorksheet? worksheet = worksheets.Add("Sheet1");

        ConfigureHeaderExcel(dataGrid, skipFirstColumnsCount, skipLastColumnsCount, worksheet);

        ConfigureBodyExcel(dataGrid, skipFirstColumnsCount, skipLastColumnsCount, objects.ToList(), worksheet);

        package.SaveAs(new FileInfo(fileName));
    }

    private static void ConfigureBodyExcel(DataGrid              dataGrid,
                                           int                   skipFirstColumnsCount,
                                           int                   skipLastColumnsCount,
                                           IReadOnlyList<object> objects,
                                           ExcelWorksheet        worksheet)
    {
        for (var i = 0; i < objects.Count; i++)
        {
            object item      = objects[i];
            var    skipCount = 0;
            for (int j = skipFirstColumnsCount; j < (dataGrid.Columns.Count - skipLastColumnsCount); j++)
            {
                if (dataGrid.Columns[j].Visibility != Visibility.Visible)
                {
                    skipCount++;
                    continue;
                }

                object? value = GetPropertyValuesMethods.GetPropertyValue(item, dataGrid.Columns[j].SortMemberPath);
                worksheet.Cells[i + 2, j + 1 - skipCount].Value = value?.ToString();
            }
        }
    }

    private static void ConfigureHeaderExcel(DataGrid       dataGrid,
                                             int            skipFirstColumnsCount,
                                             int            skipLastColumnsCount,
                                             ExcelWorksheet worksheet)
    {
        var skipCount = 0;
        for (int i = skipFirstColumnsCount; i < (dataGrid.Columns.Count - skipLastColumnsCount); i++)
        {
            if (dataGrid.Columns[i].Visibility != Visibility.Visible)
            {
                skipCount++;
                continue;
            }

            worksheet.Cells[1, i + 1 - skipCount].Value = dataGrid.Columns[i].Header;
        }
    }
}
