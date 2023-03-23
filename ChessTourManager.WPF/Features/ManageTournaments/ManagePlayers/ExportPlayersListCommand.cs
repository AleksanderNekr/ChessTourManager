using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Text;
using System.Windows;
using System.Windows.Controls;
using ChessTourManager.DataAccess.Entities;
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
            throw;
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

        // Append data
        foreach (Player player in dataGrid.Items)
        {
            sb.Append(player.PlayerLastName);
            sb.Append(",");
            sb.Append(player.PlayerFirstName);
            sb.Append(",");
            sb.Append(player.Gender);
            sb.Append(",");
            sb.Append(player.PlayerAttribute);
            sb.Append(",");
            sb.Append(player.Team?.TeamName);
            sb.Append(",");
            sb.Append(player.Group?.GroupName);
            sb.Append(",");
            sb.Append(player.PlayerBirthYear);
            sb.Append(",");
            sb.Append(player.IsActive);
            sb.Append(",");
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
            var item = (Player)data[i];
            for (var j = 0; j < dataGrid.Columns.Count; j++)
            {
                object? value = item
                               .GetType()
                               .GetProperty(dataGrid.Columns[j].SortMemberPath)?
                               .GetValue(item, null);
                worksheet.Cells[i + 2, j + 1].Value = value?.ToString();
            }

            worksheet.Cells[i + 2, 5].Value = item.Team?.TeamName;
            worksheet.Cells[i + 2, 6].Value = item.Group?.GroupName;
            worksheet.Cells[i + 2, 8].Value = item.IsActive.ToString();
        }

        // Save the workbook
        package.SaveAs(new FileInfo(fileName));
    }
}
