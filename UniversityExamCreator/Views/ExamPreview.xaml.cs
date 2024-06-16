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
        double yPoint = 80;

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
            PdfDocument document = new PdfDocument
            {
                Info = { Title = "Klausur Vorschau" }
            };

            // Initialize page and graphics
            PdfPage page = document.AddPage();
            XGraphics gfx = XGraphics.FromPdfPage(page);

            // Fonts
            XFont examTitelFont = new XFont("Verdana", 25);
            XFont titleFont = new XFont("Verdana", 14);
            XFont taskFont = new XFont("Verdana", 9);
            XFont mcFont = new XFont("Verdana", 9);

            // Page settings
            const double pageHeight = 842;
            const double pageWidth = 595;
            const double margin = 40;
            const double innerWidth = pageWidth - 2 * margin;
            const double taskSpacing = 20; // Decreased spacing between tasks
            const double mcSpacing = 15; // Increased spacing between MC answers
            const double checkboxSize = 12;

            // Draw the exam name
            gfx.DrawString(Examconfig.ExamName, examTitelFont, XBrushes.Black, new XRect(0, yPoint, page.Width, page.Height), XStringFormats.TopCenter);
            yPoint += 40;

            var tasks = new List<Task>
    {
        new Task("Einf", "Datastruct", "OF", "Easy", 3, "Task 1")
        {
            TaskContent = "This is the first task description. This description is quite long and should wrap to the next line when it exceeds the width of the page. Let's see how it looks. This is the first task description. This description is quite long and should wrap to the next line when it exceeds the width of the page. Let's see how it looks. This is the first task description. This description is quite long and should wrap to the next line when it exceeds the width of the page. Let's see how it looks.",
            TaskAnswer = new Answer("Task1", "This is the answer for the first task.")
        },
        new Task("Alg", "Algorithms", "MC", "Medium", 5, "Task 2")
        {
            TaskContent = "This is the second task description. It involves understanding algorithms.",
            TaskAnswer = new Answer("Task2", "The answer involves explaining the algorithm steps."),
            MCAnswers = new List<MCAnswer>
            {
                new MCAnswer("Task2", "Option 1", 1, 0),
                new MCAnswer("Task2", "Option 2", 2, 1),
                new MCAnswer("Task2", "Option 3", 3, 0),
                new MCAnswer("Task2", "Option 1", 1, 0),
                new MCAnswer("Task2", "Option 2", 2, 1),
                new MCAnswer("Task2", "Option 3", 3, 0),
                new MCAnswer("Task2", "Option 1", 1, 0),
                new MCAnswer("Task2", "Option 2", 2, 1),
                new MCAnswer("Task2", "Option 3", 3, 0)
            }
        },
        new Task("DB", "Database", "OF", "Hard", 10, "Task 3")
        {
            TaskContent = "This is the third task description. It involves complex database queries.",
            TaskAnswer = new Answer("Task3", "The answer includes the SQL query required to retrieve the data.")
        },
        new Task("SE", "Software Engineering", "MC", "Medium", 7, "Task 4")
        {
            TaskContent = "This is the fourth task description. It involves software engineering principles.",
            TaskAnswer = new Answer("Task4", "The answer includes explaining the software engineering principle."),
            MCAnswers = new List<MCAnswer>
            {
                new MCAnswer("Task4", "Option 1", 1, 1),
                new MCAnswer("Task4", "Option 2", 2, 0),
                new MCAnswer("Task4", "Option 3", 3, 0)
            }
        }
    };

            foreach (var task in tasks)
            {
                double titleHeight = MeasureTextHeight(task.TaskName, titleFont, innerWidth);
                double descriptionHeight = MeasureTextHeight(task.TaskContent, taskFont, innerWidth);
                double mcAnswersHeight = 0;

                if (task.TaskType == "MC" && task.MCAnswers != null)
                {
                    foreach (var mcAnswer in task.MCAnswers)
                    {
                        mcAnswersHeight += MeasureTextHeight(mcAnswer.Content, mcFont, innerWidth - checkboxSize - 5) + mcSpacing;
                    }
                }

                double requiredSpace = titleHeight + descriptionHeight + mcAnswersHeight + taskSpacing;

                // Check if there is enough space for the next task frame
                if (yPoint + requiredSpace > pageHeight - margin)
                {
                    // Add a new page and reset yPoint
                    page = document.AddPage();
                    gfx = XGraphics.FromPdfPage(page);
                    yPoint = margin; // Reset to top margin of the new page
                }

                // Draw the task title
                gfx.DrawString(task.TaskName, titleFont, XBrushes.Black, new XRect(margin, yPoint, innerWidth, page.Height), XStringFormats.TopLeft);
                yPoint += titleHeight;

                // Draw the task description using XTextFormatter
                var tf = new XTextFormatter(gfx);
                var rect = new XRect(margin, yPoint, innerWidth, page.Height - yPoint - margin);
                tf.DrawString(task.TaskContent, taskFont, XBrushes.Black, rect, XStringFormats.TopLeft);
                yPoint += descriptionHeight + taskSpacing;

                // Draw MC Answers if any
                if (task.TaskType == "MC" && task.MCAnswers != null)
                {
                    foreach (var mcAnswer in task.MCAnswers)
                    {
                        double mcAnswerHeight = MeasureTextHeight(mcAnswer.Content, mcFont, innerWidth - checkboxSize - 5);

                        // Draw the checkbox
                        gfx.DrawRectangle(XPens.Black, XBrushes.White, new XRect(margin, yPoint, checkboxSize, checkboxSize));

                        // Draw the MC answer text
                        gfx.DrawString(mcAnswer.Content, mcFont, XBrushes.Black, new XRect(margin + checkboxSize + 5, yPoint, innerWidth - checkboxSize - 5, page.Height), XStringFormats.TopLeft);

                        yPoint += mcAnswerHeight + mcSpacing;
                    }
                    yPoint += taskSpacing;
                }
            }

            // Save the document to a temporary file
            tempFilename = System.IO.Path.Combine(System.IO.Path.GetTempPath(), "KlausurVorschau.pdf");
            document.Save(tempFilename);
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
                string filename = dlg.FileName;
                System.IO.File.Copy(tempFilename, filename, true);
                MessageBox.Show($"PDF document has been saved as '{filename}'.", "PDF Saved", MessageBoxButton.OK, MessageBoxImage.Information);
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
