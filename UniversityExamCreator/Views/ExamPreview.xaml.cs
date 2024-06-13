using System;
using System.Windows;
using System.Windows.Controls;
using PdfSharp.Pdf;
using PdfSharp.Drawing;
using System.Windows.Documents;
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

            // Create an empty page
            PdfPage page = document.AddPage();

            // Get an XGraphics object for drawing
            XGraphics gfx = XGraphics.FromPdfPage(page);

            // Create a font
            XFont font = new XFont("Verdana", 20);

            // Draw the text
            gfx.DrawString("Klausur Vorschau", font, XBrushes.Black,
                new XRect(0, 0, page.Width, page.Height),
                XStringFormats.Center);

            // Save the document to a temporary file
            tempFilename = System.IO.Path.Combine(System.IO.Path.GetTempPath(), "KlausurVorschau.pdf");
            document.Save(tempFilename);

            // Open the temporary file with the default PDF viewer
            System.Diagnostics.Process.Start(new System.Diagnostics.ProcessStartInfo(tempFilename) { UseShellExecute = true });

            
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
