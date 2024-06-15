using System;
using System.Windows;
using System.Windows.Controls;
using PdfSharp.Pdf;
using PdfSharp.Drawing;
using PdfSharp.Drawing.Layout;
using System.Collections.Generic;
using System.Windows.Navigation;
using UniversityExamCreator.Models;

namespace UniversityExamCreator.Views
{
    public partial class ExamPreview : Page
    {
        private string tempFilename;
        Examconfig Examconfig { get; set; }

        internal ExamPreview(Examconfig examconfig)
        {
            Examconfig = examconfig;
            InitializeComponent();
        }

        private void Button_Click(object sender, RoutedEventArgs e)
        {
            NavigationService.Navigate(new ExamCreate(Examconfig));
        }

        private void GeneratePDFButton_Click(object sender, RoutedEventArgs e)
        {
            // Create a new PDF document
            PdfDocument document = new PdfDocument();
            document.Info.Title = "Klausur Vorschau";

            // Create the first empty page
            PdfPage page = document.AddPage();

            // Get an XGraphics object for drawing
            XGraphics gfx = XGraphics.FromPdfPage(page);

            // Create fonts
            XFont titleFont = new XFont("Verdana", 20);
            XFont taskFont = new XFont("Verdana", 12);

            // Set the initial vertical position for the first task
            double yPoint = 80;
            const double pageHeight = 842; // Height of an A4 page in points
            const double pageWidth = 595;  // Width of an A4 page in points
            const double margin = 40; // Margin from the top/bottom of the page
            const double innerWidth = pageWidth - 2 * margin;
            const double spacing = 30; // Spacing between task frames

            // Draw the exam name
            gfx.DrawString(Examconfig.ExamName, titleFont, XBrushes.Black,
                new XRect(0, yPoint, page.Width, page.Height),
                XStringFormats.TopCenter);

            // Increment vertical position
            yPoint += 40;

            // Example list of tasks (replace with your actual tasks)
            var tasks = new List<TaskFrame>
    {
        new TaskFrame { Title = "Task 1", Description = "This is the first task description. This description is quite long and should wrap to the next line when it exceeds the width of the page. Let's see how it looks.This is the first task description. This description is quite long and should wrap to the next line when it exceeds the width of the page. Let's see how it looks.This is the first task description. This description is quite long and should wrap to the next line when it exceeds the width of the page. Let's see how it looks.This is the first task description. This description is quite long and should wrap to the next line when it exceeds the width of the page. Let's see how it looks." },
        new TaskFrame { Title = "Task 2", Description = "This is the second task description." },
        new TaskFrame { Title = "Task 3", Description = "This is the third task description." }
        // Add more tasks as needed
    };

            // Loop through the tasks and draw them on the PDF
            foreach (var task in tasks)
            {
                // Check if there is enough space for the next task frame
                double titleHeight = GetTextHeight(task.Title, titleFont, innerWidth);
                double descriptionHeight = GetTextHeight(task.Description, taskFont, innerWidth);
                double requiredSpace = titleHeight + descriptionHeight + spacing;

                if (yPoint + requiredSpace > pageHeight - margin)
                {
                    // Add a new page and reset yPoint
                    page = document.AddPage();
                    gfx = XGraphics.FromPdfPage(page);
                    yPoint = margin; // Reset to top margin of the new page
                }

                // Draw the task title
                gfx.DrawString(task.Title, titleFont, XBrushes.Black,
                    new XRect(margin, yPoint, innerWidth, page.Height),
                    XStringFormats.TopLeft);

                // Increment vertical position
                yPoint += titleHeight;

                // Draw the task description using XTextFormatter
                var tf = new XTextFormatter(gfx);
                var rect = new XRect(margin, yPoint, innerWidth, page.Height - yPoint - margin);
                tf.DrawString(task.Description, taskFont, XBrushes.Black, rect, XStringFormats.TopLeft);

                // Update the yPoint based on the height of the drawn text
                descriptionHeight = GetTextHeightUsingFormatter(task.Description, taskFont, innerWidth);
                yPoint += descriptionHeight + spacing;
            }

            // Save the document to a temporary file
            string tempFilename = System.IO.Path.Combine(System.IO.Path.GetTempPath(), "KlausurVorschau.pdf");
            document.Save(tempFilename);

            // Open the temporary file with the default PDF viewer
            System.Diagnostics.Process.Start(new System.Diagnostics.ProcessStartInfo(tempFilename) { UseShellExecute = true });
        }

        private double GetTextHeight(string text, XFont font, double width)
        {
            using (var tempDocument = new PdfDocument())
            {
                var tempPage = tempDocument.AddPage();
                using (var tempGfx = XGraphics.FromPdfPage(tempPage))
                {
                    var tf = new XTextFormatter(tempGfx);
                    var rect = new XRect(0, 0, width, double.MaxValue);
                    tf.DrawString(text, font, XBrushes.Black, rect, XStringFormats.TopLeft);

                    // Measure the height of the drawn text
                    var size = tempGfx.MeasureString(text, font);
                    return size.Height;
                }
            }
        }

        private double GetTextHeightUsingFormatter(string text, XFont font, double width)
        {
            using (var tempDocument = new PdfDocument())
            {
                var tempPage = tempDocument.AddPage();
                using (var tempGfx = XGraphics.FromPdfPage(tempPage))
                {
                    var tf = new XTextFormatter(tempGfx);
                    var rect = new XRect(0, 0, width, double.MaxValue);
                    tf.DrawString(text, font, XBrushes.Black, rect, XStringFormats.TopLeft);

                    // Measure the height of the drawn text
                    var size = tempGfx.MeasureString(text, font);
                    return size.Height;
                }
            }
        }

        // Define the TaskFrame class
        public class TaskFrame
        {
            public string Title { get; set; }
            public string Description { get; set; }
        }


        private void SavePDF(string tempFilename)
        {
            // Create a SaveFileDialog to ask the user where to save the PDF
            Microsoft.Win32.SaveFileDialog dlg = new Microsoft.Win32.SaveFileDialog();
            dlg.FileName = "Neue_Klausur"; // Default file name
            dlg.DefaultExt = ".pdf"; // Default file extension
            dlg.Filter = "PDF documents (.pdf)|*.pdf"; // Filter files by extension

            // Show save file dialog box
            Nullable<bool> result = dlg.ShowDialog();

            // Process save file dialog box results
            if (result == true)
            {
                // Save document
                string filename = dlg.FileName;
                System.IO.File.Copy(tempFilename, filename, true);
                MessageBox.Show($"PDF document has been saved as '{filename}'.", "PDF Saved", MessageBoxButton.OK, MessageBoxImage.Information);

                // Navigate back to the home page
                NavigationService.Navigate(new ToolsPage());
            }
        }

        private void SavePDFButton_Click(object sender, RoutedEventArgs e)
        {
            if (!string.IsNullOrEmpty(tempFilename) && System.IO.File.Exists(tempFilename))
            {
                SavePDF(tempFilename);
            }
            else
            {
                MessageBox.Show("Please generate the PDF first.", "Error", MessageBoxButton.OK, MessageBoxImage.Error);
            }
        }
    }
}
