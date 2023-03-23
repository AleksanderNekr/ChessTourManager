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
    public static bool TryExportGrid(DataGrid dataGrid)
    {
        const string fileExtensions = "XLSX files (*.xlsx)|*.xlsx|XLS files (*.xls)|*.xls|"
                                    + "CSV files (*.csv)|*.csv";
        SaveFileDialog saveDialog = new()
                                    {
                                        FileName = "players",
                                        Filter   = fileExtensions
                                    };
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
                ExportToExcel(dataGrid, fileName);
                break;
            case ".csv":
                ExportToCsv(dataGrid, fileName);
                break;
        }

        return true;
    }

    private static void WriteHeader(StringBuilder sb1, DataGrid dataGrid)
    {
        foreach (DataGridColumn? column in dataGrid.Columns)
        {
            sb1.Append(column.Header);
            sb1.Append(",");
        }

        StartNewLine(sb1);
    }

    private static void StartNewLine(StringBuilder stringBuilder)
    {
        // Remove the last 2 comma.
        stringBuilder.Length -= 2;

        stringBuilder.AppendLine();
    }

    private static void ExportToCsv(DataGrid dataGrid, string fileName)
    {
        StringBuilder sb = new();

        WriteHeader(sb, dataGrid);

        List<object> data = dataGrid.ItemsSource.Cast<object>().ToList();

        for (var i = 0; i < data.Count; i++)
        {
            object item = data[i];
            foreach (DataGridColumn col in dataGrid.Columns)
            {
                object? value = GetPropertyValue(item, col.SortMemberPath);
                sb.Append(value);
                sb.Append(",");
            }

            StartNewLine(sb);
        }

        // Write data to file
        File.WriteAllText(fileName, sb.ToString());
    }

    private static void ExportToExcel(DataGrid dataGrid, string fileName)
    {
        List<object> data = dataGrid.ItemsSource.Cast<object>().ToList();

        ExcelPackage.LicenseContext = LicenseContext.NonCommercial;
        var package = new ExcelPackage();

        ExcelWorksheet? worksheet = package.Workbook.Worksheets.Add("Sheet1");

        for (var i = 0; i < dataGrid.Columns.Count; i++)
        {
            worksheet.Cells[1, i + 1].Value = dataGrid.Columns[i].Header;
        }

        for (var i = 0; i < data.Count; i++)
        {
            object item = data[i];
            for (var j = 0; j < dataGrid.Columns.Count; j++)
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
