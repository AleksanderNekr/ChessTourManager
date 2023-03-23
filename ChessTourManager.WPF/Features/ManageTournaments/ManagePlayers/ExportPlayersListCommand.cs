using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Reflection;
using System.Text;
using System.Windows;
using System.Windows.Controls;
using ChessTourManager.WPF.Helpers;
using Microsoft.Win32;
using OfficeOpenXml;

namespace ChessTourManager.WPF.Features.ManageTournaments.ManagePlayers;

public class ExportPlayersListCommand : CommandBase
{
    public override void Execute(object? parameter)
    {
        if (parameter is not DataGrid dataGrid)
        {
            return;
        }

        try
        {
            const string fileExtensions = "XLSX files (*.xlsx)|*.xlsx|XLS files (*.xls)|*.xls|"
                                        + "CSV files (*.csv)|*.csv";
            SaveFileDialog saveDialog = new()
                                        {
                                            FileName = "players",
                                            Filter   = fileExtensions
                                        };
            if (saveDialog.ShowDialog() != true)
            {
                return;
            }

            string fileName  = saveDialog.FileName;
            string extension = Path.GetExtension(fileName);

            // If excel file
            if (extension == ".xlsx" || extension == ".xls")
            {
                ExportToExcel(dataGrid, fileName);
            }
            else if (extension == ".csv")
            {
                ExportToCSV(dataGrid, fileName);
            }
        }
        catch (Exception e)
        {
            MessageBox.Show(e.Message, "Экспорт списка участников", MessageBoxButton.OK, MessageBoxImage.Error);
            return;
        }

        MessageBox.Show("Данные успешно экспортированы", "Экспорт списка участников",
                        MessageBoxButton.OK, MessageBoxImage.Information);
    }

    private void ExportToCSV(DataGrid dataGrid, string fileName)
    {
        var sb = new StringBuilder();

        // Append column headers
        foreach (DataGridColumn? column in dataGrid.Columns)
        {
            sb.Append(column.Header);
            sb.Append(",");
        }
        sb.AppendLine();

        List<object> data = dataGrid.ItemsSource.Cast<object>().ToList();

        // Write the data
        for (var i = 0; i < data.Count; i++)
        {
            object item = data[i];
            foreach (DataGridColumn col in dataGrid.Columns)
            {
                object? value = GetPropertyValue(item, col.SortMemberPath);
                sb.Append(value);
                sb.Append(",");
            }
            sb.AppendLine();
        }

        // Write data to file
        File.WriteAllText(fileName, sb.ToString());
    }

    private void ExportToExcel(DataGrid dataGrid, string fileName)
    {
        // Get the data from the DataGrid
        List<object> data = dataGrid.ItemsSource.Cast<object>().ToList();

        ExcelPackage.LicenseContext = LicenseContext.NonCommercial;
        // Create a new Excel workbook
        var package = new ExcelPackage();

        // Add a worksheet
        ExcelWorksheet? worksheet = package.Workbook.Worksheets.Add("Sheet1");

        // Write the headers
        for (var i = 0; i < dataGrid.Columns.Count; i++)
        {
            worksheet.Cells[1, i + 1].Value = dataGrid.Columns[i].Header;
        }

        // Write the data
        for (var i = 0; i < data.Count; i++)
        {
            var item = data[i];
            for (var j = 0; j < dataGrid.Columns.Count; j++)
            {
                object? value = GetPropertyValue(item, dataGrid.Columns[j].SortMemberPath);
                worksheet.Cells[i + 2, j + 1].Value = value?.ToString();
            }
        }

        // Save the workbook
        package.SaveAs(new FileInfo(fileName));
    }

    // Recursive function to get the value of a property with a path that may have multiple levels
    private object? GetPropertyValue(object obj, string propertyPath)
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
