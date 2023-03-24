using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Reflection;
using System.Text;
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
        string filename = dataGrid.Items[0].GetType().Name + "s";

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

        if (dataGridColumnsCount <= skipFirstColumnsCount + skipLastColumnsCount)
        {
            throw new ArgumentOutOfRangeException(nameof(skipFirstColumnsCount),
                                                  "skipFirstColumnsCount + skipLastColumnsCount"
                                                + " must be < dataGridColumnsCount");
        }
    }

    private static void WriteHeader(StringBuilder sb, DataGrid dataGrid, int skipFirstColumnsCount,
                                    int           skipLastColumnsCount)
    {
        for (int i = skipFirstColumnsCount; i < dataGrid.Columns.Count - skipLastColumnsCount; i++)
        {
            DataGridColumn? column = dataGrid.Columns[i];
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

        WriteHeader(sb, dataGrid, skipFirstColumnsCount, skipLastColumnsCount);

        List<object> objects = dataGrid.ItemsSource.Cast<object>().ToList();

        WriteBody(dataGrid, objects, sb, skipFirstColumnsCount, skipLastColumnsCount);

        File.WriteAllText(fileName, sb.ToString());
    }

    private static void WriteBody(DataGrid dataGrid,                  List<object> data, StringBuilder sb,
                                  int      skipFirstColumnsCount = 0, int          skipLastColumnsCount = 0)
    {
        for (var i = 0; i < data.Count; i++)
        {
            object item = data[i];
            for (int j = skipFirstColumnsCount; j < dataGrid.Columns.Count - skipLastColumnsCount; j++)
            {
                object? value = GetPropertyValue(item, dataGrid.Columns[j].SortMemberPath);
                sb.Append(value);
                sb.Append(Separator);
            }

            StartNewLine(sb);
        }
    }

    private static void ExportToExcel(DataGrid dataGrid, string fileName, int skipFirstColumnsCount,
                                      int      skipLastColumnsCount)
    {
        List<object> objects = dataGrid.ItemsSource.Cast<object>().ToList();

        ExcelPackage.LicenseContext = LicenseContext.NonCommercial;
        var package = new ExcelPackage();

        ExcelWorksheet? worksheet = package.Workbook.Worksheets.Add("Sheet1");

        for (int i = skipFirstColumnsCount; i < dataGrid.Columns.Count - skipLastColumnsCount; i++)
        {
            worksheet.Cells[1, i + 1].Value = dataGrid.Columns[i].Header;
        }

        for (var i = 0; i < objects.Count; i++)
        {
            object item = objects[i];
            for (int j = skipFirstColumnsCount; j < dataGrid.Columns.Count - skipLastColumnsCount; j++)
            {
                object? value = GetPropertyValue(item, dataGrid.Columns[j].SortMemberPath);
                worksheet.Cells[i + 2, j + 1].Value = value?.ToString();
            }
        }

        package.SaveAs(new FileInfo(fileName));
    }

    /// Function to get the value of a property with a path that may have multiple levels.
    private static object? GetPropertyValue(object obj, string propertyPath)
    {
        string[] propertyNames = propertyPath.Split('.');
        object?  propertyValue = obj;

        foreach (string propertyName in propertyNames)
        {
            PropertyInfo? property = propertyValue.GetType().GetProperty(propertyName);
            if (property == null)
            {
                return null;
            }

            propertyValue = property.GetValue(propertyValue, null);
            if (propertyValue == null)
            {
                return null;
            }
        }

        return propertyValue;
    }
}
