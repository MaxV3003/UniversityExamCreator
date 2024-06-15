using System;
using System.Windows;
using System.Windows.Controls;
using PdfSharp.Pdf;
using PdfSharp.Drawing;
using PdfSharp.Drawing.Layout;
using System.Collections.Generic;
using System.Windows.Navigation;
using UniversityExamCreator.Models;
using System.Text;

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
        double yPoint = 80;
        private void GeneratePDFButton_Click(object sender, RoutedEventArgs e)
        {
            // Create a new PDF document
            PdfDocument document = new PdfDocument
            {
                Info = { Title = "Klausur Vorschau" }
            };

            // Initialize page and graphics
            PdfPage page = document.AddPage();
            XGraphics gfx = XGraphics.FromPdfPage(page);

            // Fonts
            XFont titleFont = new XFont("Verdana", 20);
            XFont taskFont = new XFont("Verdana", 12);

            // Page settings
            //double yPoint = 80;
            const double pageHeight = 842;
            const double pageWidth = 595;
            const double margin = 40;
            const double innerWidth = pageWidth - 2 * margin;
            const double spacing = 30;

            // Draw the exam name
            gfx.DrawString(Examconfig.ExamName, titleFont, XBrushes.Black, new XRect(0, yPoint, page.Width, page.Height), XStringFormats.TopCenter);
            yPoint += 40;

            // Example list of tasks (replace with your actual tasks)
            var tasks = new List<TaskFrame>
            {
                new TaskFrame { Title = "Task 1", Description = "This is the first task description. This description is quite long and should wrap to the next line when it exceeds the width of the page. Let's see how it looks.This is the first task description. This description is quite long and should wrap to the next line when it exceeds the width of the page. Let's see how it looks.This is the first task description. This description is quite long and should wrap to the next line when it exceeds the width of the page. Let's see how it looks." },
                new TaskFrame { Title = "Task 2", Description = "This is the second task description." },
                new TaskFrame { Title = "Task 3", Description = "This is the third task description." }
                // Add more tasks as needed
            };

            // Loop through the tasks and draw them on the PDF
            foreach (var task in tasks)
            {
                Console.WriteLine("Y-Point: "+ yPoint);
                // Measure heights
                double titleHeight = MeasureTextHeight(task.Title, titleFont, innerWidth);

                Console.WriteLine("TitelHeight: " + titleHeight );
                double descriptionHeight = MeasureTextHeight(task.Description, taskFont, innerWidth);

                Console.WriteLine("DescriptionHeight: "+ descriptionHeight);
                double requiredSpace = titleHeight + descriptionHeight + spacing;

                // Check if there is enough space for the next task frame
                if (yPoint + requiredSpace > pageHeight - margin)
                {
                    // Add a new page and reset yPoint
                    page = document.AddPage();
                    gfx = XGraphics.FromPdfPage(page);
                    yPoint = margin; // Reset to top margin of the new page
                }

                // Draw the task title
                gfx.DrawString(task.Title, titleFont, XBrushes.Black, new XRect(margin, yPoint, innerWidth, page.Height), XStringFormats.TopLeft);
                yPoint += titleHeight;

                // Draw the task description using XTextFormatter
                var tf = new XTextFormatter(gfx);

                var rect = new XRect(margin, yPoint, innerWidth, page.Height - yPoint - margin);
                tf.DrawString(task.Description, taskFont, XBrushes.Black, rect, XStringFormats.TopLeft);
                Console.WriteLine("rect: " + rect.Height.ToString());
                // Update the yPoint based on the height of the drawn text
                yPoint += descriptionHeight + spacing;
                Console.WriteLine("");
            }

            // Save the document to a temporary file
            tempFilename = System.IO.Path.Combine(System.IO.Path.GetTempPath(), "KlausurVorschau.pdf");
            document.Save(tempFilename);

            // Open the temporary file with the default PDF viewer
            System.Diagnostics.Process.Start(new System.Diagnostics.ProcessStartInfo(tempFilename) { UseShellExecute = true });
        }

        private double MeasureTextHeight(string text, XFont font, double width)
        {
            using (var tempDocument = new PdfDocument())
            {
                var tempPage = tempDocument.AddPage();
                using (var tempGfx = XGraphics.FromPdfPage(tempPage))
                {
                    var tf = new XTextFormatter(tempGfx);
                    var rect = new XRect(0, 0, width, double.MaxValue);

                    // Split text into correctly formatted lines
                    var lines = SplitTextIntoLines(text, font, width, tempGfx, double.MaxValue, 0);

                    double height = 0;

                    // Measure the height of each line
                    foreach (var line in lines)
                    {
                        height += tempGfx.MeasureString(line, font).Height;
                    }

                    return height;
                }
            }
        }

        private List<string> SplitTextIntoLines(string text, XFont font, double innerWidth, XGraphics gfx, double pageHeight, double margin)
        {
            var lines = new List<string>();
            var currentLine = new StringBuilder();
            double currentY = 0;

            // Split text into words
            var words = text.Split(new[] { ' ' }, StringSplitOptions.RemoveEmptyEntries);

            foreach (var word in words)
            {
                // Measure current line width with new word added
                var proposedSize = gfx.MeasureString(currentLine.ToString() + " " + word, font);

                // Check if adding the new word exceeds innerWidth
                if (proposedSize.Width > innerWidth)
                {
                    // Add current line to lines and start new line with current word
                    lines.Add(currentLine.ToString());
                    currentLine.Clear();
                    currentLine.Append(word);
                    currentY += font.GetHeight();

                    // Check if new line exceeds page height
                    if (currentY + font.GetHeight() > pageHeight - margin)
                    {
                        // Add remaining text as one line and return
                        lines.Add(currentLine.ToString());
                        return lines;
                    }
                }
                else
                {
                    // Add word to current line
                    if (currentLine.Length > 0)
                    {
                        currentLine.Append(" ");
                    }
                    currentLine.Append(word);
                }
            }

            // Add last line
            if (currentLine.Length > 0)
            {
                lines.Add(currentLine.ToString());
            }

            return lines;
        }


        public class TaskFrame
        {
            public string Title { get; set; }
            public string Description { get; set; }
        }

        private void SavePDF(string tempFilename)
        {
            // Create a SaveFileDialog to ask the user where to save the PDF
            var dlg = new Microsoft.Win32.SaveFileDialog
            {
                FileName = "Neue_Klausur",
                DefaultExt = ".pdf",
                Filter = "PDF documents (.pdf)|*.pdf"
            };

            // Show save file dialog box
            var result = dlg.ShowDialog();

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
