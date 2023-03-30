using System.Collections.Generic;
using System.Data;
using System.Linq;
using System.Printing;
using System.Windows;
using System.Windows.Controls;
using System.Windows.Data;
using System.Windows.Documents;
using System.Windows.Markup;
using System.Windows.Media;

namespace ChessTourManager.WPF.Helpers.FileHelpers;

public static class PrintMethods
{
    public static bool TryPrintFrameworkElement(FrameworkElement frameworkElement)
    {
        //        PrintDialog printDialog = new();
        //
        //        if (printDialog.ShowDialog() == false)
        //        {
        //            return false;
        //        }
        return (bool)ShowPrintPreview(GetFixedDocument(frameworkElement, new PrintDialog()));
        //        Size pageSize = new(printDialog.PrintableAreaWidth, printDialog.PrintableAreaHeight);
        //
        //        frameworkElement.Measure(pageSize);
        //        frameworkElement.Arrange(new Rect(5, 5, pageSize.Width, pageSize.Height));
        //
        //        printDialog.PrintVisual(frameworkElement, "Printed data");
    }

    public static FixedDocument GetFixedDocument(FrameworkElement element, PrintDialog printDialog)
    {
        PrintCapabilities capabilities = printDialog.PrintQueue.GetPrintCapabilities(printDialog.PrintTicket);
        Size              pageSize     = new(printDialog.PrintableAreaWidth, printDialog.PrintableAreaHeight);
        Size visibleSize = new(capabilities.PageImageableArea.ExtentWidth,
                               capabilities.PageImageableArea.ExtentHeight);
        FixedDocument fixedDoc = new();
        //If the toPrint visual is not displayed on screen we neeed to measure and arrange it
        element.Measure(new Size(double.PositiveInfinity, double.PositiveInfinity));
        element.Arrange(new Rect(new Point(0, 0), element.DesiredSize));
        //
        Size size = element.DesiredSize;
        //Will assume for simplicity the control fits horizontally on the page
        double yOffset = 0;
        while (yOffset < size.Height)
        {
            VisualBrush vb = new(element)
                             {
                                 Stretch      = Stretch.None,
                                 AlignmentX   = AlignmentX.Center,
                                 AlignmentY   = AlignmentY.Top,
                                 ViewboxUnits = BrushMappingMode.Absolute,
                                 TileMode     = TileMode.None,
                                 // Centered on the page.
                                 Viewbox = new Rect(0, yOffset, size.Width, size.Height)
                             };
            PageContent pageContent = new();
            FixedPage   page        = new();
            ((IAddChild)pageContent).AddChild(page);
            fixedDoc.Pages.Add(pageContent);
            page.Width  = pageSize.Width;
            page.Height = pageSize.Height;
            Canvas canvas = new();
            FixedPage.SetLeft(canvas, capabilities.PageImageableArea.OriginWidth);
            FixedPage.SetTop(canvas, capabilities.PageImageableArea.OriginHeight);
            canvas.Width      = visibleSize.Width;
            canvas.Height     = visibleSize.Height;
            canvas.Background = vb;
            page.Children.Add(canvas);
            yOffset += visibleSize.Height;
        }

        return fixedDoc;
    }

    private static bool? ShowPrintPreview(IDocumentPaginatorSource fixedDoc)
    {
        Window wnd = new();
        DocumentViewer viewer = new()
                                {
                                    Document = fixedDoc
                                };

        wnd.Content = viewer;
        return wnd.ShowDialog();
    }
}
