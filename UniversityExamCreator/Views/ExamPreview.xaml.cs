using System.Windows;
using PdfSharp.Pdf;
using PdfSharp.Drawing;
using PdfSharp.Drawing.Layout;
using System.Collections.Generic;
using System.Windows.Navigation;
using UniversityExamCreator.Models;
using System.Text;
using System;
using System.Collections.ObjectModel;
using System.Windows.Controls;
using System.Windows.Input;
using System.Windows.Media;
using System.Data.SQLite;

namespace UniversityExamCreator.Views
{
    public partial class ExamPreview : System.Windows.Controls.Page //System.Windows.Controls. sonst entfernen
    {

        // Configs
        private string tempFilename;
        Examconfig Examconfig { get; set; }
        public List<Task> Tasks { get; set; }
        private ObservableCollection<Task> SelectedTasks { get; set; }

        // PDF-Document & Drawing 
        PdfSharp.Pdf.PdfDocument document;
        PdfSharp.Drawing.XGraphics gfx;

        // Test-Area
        List<String> Fonts { get; set; }

        List<AdditionalInformation> additionalInformation;

        // Font-Vars
        int defaultExamTitleSize;
        int defaultTitleSize;
        int defaultTaskSize;
        int defaultMCSize;
        string defaultFont;
        XFont examTitelFont;
        XFont titleFont;
        XFont taskFont;
        XFont mcFont;
        XFont specialFont; // a static Font only for the CoverSheet

        // Page settings
        const double pageHeight = 842;
        const double pageWidth = 600;
        const double margin = 60;
        const double innerWidth = pageWidth - 2 * margin;
        const double taskSpacing = 20;
        const double mcSpacing = 10;
        const double checkboxSize = 10;
        double yPoint = 100;

        // Task-Switch-Content
        private string dbConnectionString;
        //private ObservableCollection<Task> TasksSwitch;
        private Task draggedItem;

        internal ExamPreview(Examconfig examconfig, List<Task> tasks, List<Task> selectedTasks)
        {
            Examconfig = examconfig;
            Tasks = tasks;
            InitializeComponent();

            SelectedTasks = new ObservableCollection<Task>();

            foreach (Task task in selectedTasks) 
            {
                SelectedTasks.Add(task);
            }

            dataGrid.ItemsSource = SelectedTasks;

            // Initialize the available fonts.
            Fonts = new List<string>
            {
                "Verdana",
                "Times New Roman",
                "Arial",
            };

            // Fill FontCombobox.
            FontComboBox.ItemsSource = Fonts;

            // Initialize the Fontsizes for the Comboboxes.
            for (int i = 9; i <= 32; i += 1)
            {
                ExamTitleFontSize.Items.Add(i);
                TitleFontSize.Items.Add(i);
                TextFontSize.Items.Add(i);
            }

            // Set the Default-Size-Values of the Font-Vars.
            defaultExamTitleSize = 25;
            defaultTitleSize = 14;
            defaultTaskSize = 9;
            defaultMCSize = 9;

            // Set the Default-Font-Typ of the Exam.
            defaultFont = "Verdana";
            setFont(defaultFont);

            // Show the Default Value in the Combobox before the user is modifing them. 
            FontComboBox.SelectedItem = defaultFont;
            ExamTitleFontSize.SelectedItem = defaultExamTitleSize;
            TitleFontSize.SelectedItem = defaultTitleSize;
            TextFontSize.SelectedItem = defaultTaskSize;

            // Additional Content
            additionalInformation = additionalInformationCreator();
            ListViewInformation.ItemsSource = additionalInformation;

            // Task-Switch-Content 
            PathFinder pathFinder = new PathFinder("Databases", "database.db");
            dbConnectionString = "Data Source=" + pathFinder.GetPath() + ";Version=3;";
            dataGrid.ItemsSource = SelectedTasks;

            Tasks = taskCreator();
        }

        /// <summary>
        /// Selcetionchanger if there is an other Item selected.
        /// </summary>
        private void FontComboBox_SelectionChanged(object sender, SelectionChangedEventArgs e)
        {
            string selectedFont = FontComboBox.SelectedItem as string;

            if (selectedFont != null)
            {
                setFont(selectedFont);
            }
        }

        /// <summary>
        /// If ExamTitleFontCombobox-Value has changed.
        /// </summary>
        private void ExamTitleFontSize_SelectionChanged(object sender, SelectionChangedEventArgs e)
        {
            int? selectedExamTitleFontSize = ExamTitleFontSize.SelectedItem as int?;

            if (selectedExamTitleFontSize.HasValue)
            {
                setFontSize(selectedExamTitleFontSize.Value, "examTitelFont");
            }
        }

        /// <summary>
        /// If TitleFontCombobox-Value has changed.
        /// </summary>
        private void TitleFontSize_SelectionChanged(object sender, SelectionChangedEventArgs e)
        {
            int? selectedTitleFontSize = TitleFontSize.SelectedItem as int?;

            if (selectedTitleFontSize.HasValue)
            {
                setFontSize(selectedTitleFontSize.Value, "titleFont");
            }
        }

        /// <summary>
        /// If TaskFontCombobox-Value has changed.
        /// </summary>
        private void TextFontSize_SelectionChanged(object sender, SelectionChangedEventArgs e)
        {
            int? selectedTextFontSize = TextFontSize.SelectedItem as int?;

            if (selectedTextFontSize.HasValue)
            {
                setFontSize(selectedTextFontSize.Value, "taskFont");
            }
        }

        /// <summary>
        /// Set the Font-Typ the Exam is written in.
        /// </summary>
        private void setFont(string font)
        {
            defaultFont = font;
            examTitelFont = new XFont(font, 25);
            titleFont = new XFont(font, 14);
            taskFont = new XFont(font, 9);
            mcFont = new XFont(font, 9);
            specialFont = new XFont(font, 9);
        }

        /// <summary>
        /// Set the selected FontSize-Value.
        /// </summary>
        private void setFontSize(int size, string font)
        {
            switch (font)
            {
                case "examTitelFont":
                    defaultExamTitleSize = size;
                    examTitelFont = new XFont(defaultFont, size);
                    break;
                case "titleFont":
                    defaultTitleSize = size;
                    titleFont = new XFont(defaultFont, size);
                    break;
                case "taskFont":
                    defaultTaskSize = size;
                    taskFont = new XFont(defaultFont, size);
                    mcFont = new XFont(defaultFont, size);
                    break;
            }
        }

        /*---------------------------------------------------------------------------------------------------*/
        //                                  PDF-Erzeugungs-Abschnitt                                         //
        /*---------------------------------------------------------------------------------------------------*/

        /// <summary>
        /// Navigate to previous page.
        /// </summary>
        private void BackButton_Click(object sender, RoutedEventArgs e)
        {
            NavigationService.Navigate(new ExamCreate(Examconfig));
        }

        /// <summary>
        /// Starts the PDF-creation-process. 
        /// </summary>
        private void GeneratePDFButton_Click(object sender, RoutedEventArgs e)
        {
            // Create new PDF-Document, Page & Graphics
            PdfSharp.Pdf.PdfDocument newDocument = new PdfDocument()
            {
                Info = { Title = "Klausur Vorschau" }
            };

            // Page initializisation.
            document = newDocument;
            yPoint = 100;
            PdfPage page = document.AddPage();
            gfx = XGraphics.FromPdfPage(page);
            DateTime? selectedDate = ExamDate.SelectedDate;

            // The process starts if the values are set. 
            if (selectedDate.HasValue && (TextBoxExaminer.Text != string.Empty))
            {
                // Draw the PDF
                drawCoverSheet(page);
                yPoint = pageHeight;
                drawTasks(page);

                // Save the document to a temporary file
                tempFilename = System.IO.Path.Combine(System.IO.Path.GetTempPath(), "KlausurVorschau.pdf");
                document.Save(tempFilename);
                System.Diagnostics.Process.Start(new System.Diagnostics.ProcessStartInfo(tempFilename) { UseShellExecute = true });
            }

            // Check for missing values. 
            if (!selectedDate.HasValue && (TextBoxExaminer.Text.Equals(string.Empty)))
            {
                MessageBox.Show("Bitte ein Datum und einen Prüfer eingeben.", "Fehlende Prüfungsangaben", MessageBoxButton.OK, MessageBoxImage.Warning);
            }
            if (selectedDate.HasValue && (TextBoxExaminer.Text.Equals(string.Empty)))
            {
                MessageBox.Show("Bitte einen Prüfer eingeben.", "Fehlende Prüfungsangaben", MessageBoxButton.OK, MessageBoxImage.Warning);
            }
            if (!selectedDate.HasValue && (!TextBoxExaminer.Text.Equals(string.Empty)))
            {
                MessageBox.Show("Bitte ein Datum eingeben.", "Fehlende Prüfungsangaben", MessageBoxButton.OK, MessageBoxImage.Warning);
            }
        }

        /// <summary>
        /// Call-Function for the cover-sheet operations. 
        /// </summary>
        private void drawCoverSheet(PdfPage page)
        {
            // All methods are hard-coded for consitency. 
            drawHeader();
            drawMetaTable();
            drawTasksTable(page);
            //es könnte sein, dass hier die falsche Page des Dokuments übergeben wird
            drawAdditionalInformation(page);
        }

        /// <summary>
        /// Starts the PDF-creation-process for the Answer-PDF.
        /// </summary>
        private void GenerateAnswerPDF_Click(object sender, RoutedEventArgs e)
        {
            // Create new PDF-Document, Page & Graphics
            PdfSharp.Pdf.PdfDocument newDocument = new PdfDocument()
            {
                Info = { Title = "Klausur Vorschau" }
            };

            // Page initializisation.
            document = newDocument;
            yPoint = 100;
            PdfPage page = document.AddPage();
            gfx = XGraphics.FromPdfPage(page);
            DateTime? selectedDate = ExamDate.SelectedDate;

            // The process starts if the values are set. 
            if (selectedDate.HasValue && (TextBoxExaminer.Text != string.Empty))
            {
                // Draw the PDF
                drawCoverSheet(page);
                yPoint = pageHeight;
                drawTaskAnswers(page);

                // Save the document to a temporary file
                tempFilename = System.IO.Path.Combine(System.IO.Path.GetTempPath(), "AntwortKlausurVorschau.pdf");
                document.Save(tempFilename);
                System.Diagnostics.Process.Start(new System.Diagnostics.ProcessStartInfo(tempFilename) { UseShellExecute = true });
            }

            // Check for missing values. 
            if (!selectedDate.HasValue && (TextBoxExaminer.Text.Equals(string.Empty)))
            {
                MessageBox.Show("Bitte ein Datum und einen Prüfer eingeben.", "Fehlende Prüfungsangaben", MessageBoxButton.OK, MessageBoxImage.Warning);
            }
            if (selectedDate.HasValue && (TextBoxExaminer.Text.Equals(string.Empty)))
            {
                MessageBox.Show("Bitte einen Prüfer eingeben.", "Fehlende Prüfungsangaben", MessageBoxButton.OK, MessageBoxImage.Warning);
            }
            if (!selectedDate.HasValue && (!TextBoxExaminer.Text.Equals(string.Empty)))
            {
                MessageBox.Show("Bitte ein Datum eingeben.", "Fehlende Prüfungsangaben", MessageBoxButton.OK, MessageBoxImage.Warning);
            }
        }

        /// <summary>
        /// Draw the header of the cover-sheet.
        /// </summary>
        private void drawHeader()
        {
            // Laod Logo
            PathFinder pathFinder = new PathFinder("Models", "OVGU-FIN_farbig.jpg");
            string fullPath = pathFinder.GetPath();
            XImage logo = XImage.FromFile(fullPath);
            gfx.DrawImage(logo, 150, 0, 300, 100);

            // Initialize Exam-Header and first Line
            var examHeader = new XTextFormatter(gfx);
            string leftSideText = "Prüfer: ";
            DateTime? selectedDate = ExamDate.SelectedDate;

            // Draw the first Line 
            DateTime date = selectedDate.Value;
            leftSideText += TextBoxExaminer.Text;
            string rightSideText = "Magdeburg, " + date.ToString("dd.MM.yyyy");
            XSize rightTextSize = gfx.MeasureString(rightSideText, taskFont);
            double xRightPosition = margin + innerWidth - rightTextSize.Width;
            draw(leftSideText, taskFont, new XPoint(margin, yPoint));
            draw(rightSideText, taskFont, new XPoint(xRightPosition, yPoint));
            yPoint += taskSpacing;

            // Draw the exam name with wrapping
            var examNameRect = new XRect(margin, yPoint, innerWidth, pageHeight - yPoint - margin);
            DrawWrappedText(Examconfig.ExamName, examTitelFont, examNameRect, out double textHeight);
            yPoint += textHeight; // Problem: The Textheight isn't right after Measurement. If the Text is to long it will overlap the MetaTable
        }

        /// <summary>
        /// Draw the meta-table for the student authentication.
        /// </summary>
        private void drawMetaTable()
        {
            double x = margin;
            double y = yPoint;
            double cellHeight = 40;
            double cellWidth = 160;

            string[,] data = {
                    { " Name, Vorname", " Studiengang", " Martrikelnummer" },
                    { " Blätteranzahl", " Unterschrift", " erreichte Note " }
            };

            for (int row = 0; row < data.GetLength(0); row++)
            {
                for (int col = 0; col < data.GetLength(1); col++)
                {
                    gfx.DrawRectangle(XPens.Black, x + col * cellWidth, y + (row + 1) * cellHeight, cellWidth, cellHeight);
                    draw(data[row, col], specialFont, new XRect(x + col * cellWidth, y + (row + 1) * cellHeight, cellWidth, cellHeight), "XTopLeft");
                }
                yPoint += cellHeight;
            }
            yPoint += 2 * cellHeight;
        }

        /// <summary>
        /// Draw the table which contains all tasks as an overview.
        /// </summary>
        private void drawTasksTable(PdfPage page)
        {
            double cellHeight = 20;
            double defaultCellWidth = specialFont.Size * 7;
            string[,] data = new string[Tasks.Count + 1, 4]; // +1 for Header

            // Header
            string[] headers = { " # ", " Aufgabe ", " max. Punkte ", " erreicht " };
            for (int i = 0; i < headers.Length; i++)
            {
                data[0, i] = headers[i];
            }

            // Datarows
            int a = 1; // start at second entry
            foreach (var task in Tasks)
            {
                data[a, 0] = (a).ToString();
                data[a, 1] = task.TaskName;
                data[a, 2] = task.Points.ToString();
                data[a, 3] = "";
                a++;
            }

            // Calculate Tablesettings
            double secondColumnWidth = CalculateMaxCellWidthInSecondColumn(data, specialFont, defaultCellWidth);
            double totalTableWidth = defaultCellWidth * 3 + secondColumnWidth;

            // Draw Information
            string noticeText = "Diese Tabelle bitte nicht ausfüllen!";
            draw(noticeText, specialFont, new XRect(margin, yPoint, innerWidth, specialFont.Height), "Center");
            yPoint += specialFont.Height + 2;

            // Center the table and draw it
            double x = (page.Width - totalTableWidth) / 2;
            double y = yPoint;

            for (int row = 0; row < data.GetLength(0); row++)
            {
                double currentX = x;
                
                // Update page-amount if necessary 
                /*if (yPoint + cellHeight > pageHeight - margin)
                {
                    page = document.AddPage();
                    gfx = XGraphics.FromPdfPage(page);
                    y = margin;
                }*/

                for (int col = 0; col < data.GetLength(1); col++)
                {
                    double currentCellWidth = (col == 1) ? secondColumnWidth : defaultCellWidth;

                    if (row == 0)
                    {
                        // Draw Heading 
                        gfx.DrawRectangle(XPens.Black, XBrushes.LightGray, currentX, y, currentCellWidth, cellHeight);
                        draw(data[row, col], specialFont, new XRect(currentX, y, currentCellWidth, cellHeight), "Center");
                    }
                    else
                    {
                        // Draw Datarows
                        gfx.DrawRectangle(XPens.Black, currentX, y, currentCellWidth, cellHeight);
                        draw(data[row, col], specialFont, new XRect(currentX, y, currentCellWidth, cellHeight), "Center");
                    }

                    // Update x-Coordinate
                    currentX += currentCellWidth;
                }

                // Update y-Coordinate
                y += cellHeight;
            }

            // Spacing after the table
            yPoint = y + 2 * cellHeight;
        }

        /// <summary>
        /// Draw additional information for the students. 
        /// </summary>
        private void drawAdditionalInformation(PdfPage page)
        {
            double titelHigth = MeasureTextHeight("Hinweise", titleFont, innerWidth);
            List<String> checkedInfos = new List<String>();

            foreach (var checkItem in additionalInformation)
            {
                if (checkItem.IsChecked == true)
                {
                    checkedInfos.Add(checkItem.Content);
                }
            }

            // Draw title
            draw("Zusätzliche Hinweise", titleFont, new XRect(margin, yPoint, innerWidth, page.Height), "TopCenter");
            yPoint += titelHigth + taskSpacing / 2;

            // Draw the additional Information
            foreach (var info in checkedInfos)
            {
                string bulletPoint = "\u2022 "; // Unicode for Dots
                string bulletText = bulletPoint + info;
                double infoHigth = MeasureTextHeight(bulletText, specialFont, innerWidth / 2);

                // Update page-amount if necessary 
                if (yPoint + infoHigth > pageHeight - margin)
                {
                    page = document.AddPage();
                    gfx = XGraphics.FromPdfPage(page);
                    yPoint = margin;
                }

                // Add justification
                var tf = new XTextFormatter(gfx)
                {
                    Alignment = XParagraphAlignment.Justify
                };

                // Center the text
                double xPosition = (page.Width - innerWidth / 2) / 2;
                var rect = new XRect(xPosition, yPoint, innerWidth / 2, page.Height - yPoint - margin);

                // Draw bulletpoints
                tf.DrawString(bulletText, specialFont, XBrushes.Black, rect, XStringFormats.TopLeft);
                yPoint += infoHigth + 2;
            }
        }

        /// <summary>
        /// HelpFunction for drawing the examtitle.
        /// </summary>
        private void DrawWrappedText(string text, XFont font, XRect rect, out double textHeight)
        {
            var tf = new XTextFormatter(gfx);
            tf.Alignment = XParagraphAlignment.Center;

            // Measure text height to fit the rect
            var size = gfx.MeasureString(text, font, XStringFormats.TopLeft);
            textHeight = size.Height;
            tf.DrawString(text, font, XBrushes.Black, rect, XStringFormats.TopLeft);

        }

        /// <summary>
        /// Calculates the width for the second column of the task-table.
        /// </summary>
        /// <returns> Double </returns>
        private double CalculateMaxCellWidthInSecondColumn(string[,] data, XFont font, double cellWidth)
        {
            double maxWidth = 0;

            // Ignore Header-Line
            for (int row = 1; row < data.GetLength(0); row++)
            {
                double currentWidth = getCellWidth(data[row, 1], font, cellWidth);
                if (currentWidth > maxWidth)
                {
                    maxWidth = currentWidth;
                }
            }

            // Padding
            return maxWidth + 10;
        }

        /// <summary>
        /// Calculates the width of the current table-cell.
        /// </summary>
        /// <returns> Double </returns>
        private double getCellWidth(string text, XFont font, double cellWidth)
        {
            // Create a Datacontext for a cell
            using (var gfx = XGraphics.CreateMeasureContext(
                new XSize(1000, 1000),               // Size of measuring range
                XGraphicsUnit.Point,                 // Measurement-Unit
                XPageDirection.Downwards))           // Side Alignment
            {
                XSize size = gfx.MeasureString(text, font);

                if (size.Width < cellWidth)
                {
                    return cellWidth;
                }
                else
                {
                    return size.Width;
                }
            }
        }

        /// <summary>
        /// Draw every task which was selected for the exam.
        /// </summary>
        private void drawTasks(PdfPage page)
        {
            int headercounter = 1;
            List<Task> tasks = Tasks;

            // Draw the tasks
            foreach (var task in tasks)
            {
                double titleHeight = MeasureTextHeight(task.TaskName, titleFont, innerWidth);
                double descriptionHeight = MeasureTextHeight(task.TaskContent, taskFont, innerWidth);
                descriptionHeight += task.EmptyLineCount * 10;
                double mcAnswersHeight = 0;

                if (task.TaskType == "Multiple Choice" && task.MCAnswers != null)
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
                    CreateHeaderSite(page);
                }

                // Draw the task title and adjust the Tasknumber
                string finalheader = string.Empty;
                finalheader += headercounter;
                headercounter++;
                finalheader += ". " + task.TaskName;
                draw(finalheader, titleFont, new XRect(margin, yPoint, innerWidth, page.Height), "TopLeft");
                yPoint += titleHeight + 5;

                // Draw the task description 
                var tf = new XTextFormatter(gfx);
                tf.Alignment = XParagraphAlignment.Justify;
                draw(task.TaskContent, taskFont, new XRect(margin, yPoint, innerWidth, page.Height - yPoint - margin), "TopLeft", tf);
                yPoint += descriptionHeight + taskSpacing; //hier muss noch die Height angepasst werden, wenn der requiredspace > als eine ganze Seite ist 

                // Draw MC Answers if any
                if (task.TaskType == "Multiple Choice" && task.MCAnswers != null)
                {
                    foreach (MCAnswer mcAnswer in task.MCAnswers)
                    {
                        double mcAnswerHeight = MeasureTextHeight(mcAnswer.Content, mcFont, innerWidth - checkboxSize - 5);

                        gfx.DrawRectangle(XPens.Black, XBrushes.White, new XRect(margin, yPoint, checkboxSize, checkboxSize));
                        gfx.DrawString(mcAnswer.Content, mcFont, XBrushes.Black, new XRect(margin + checkboxSize + 5, yPoint, innerWidth - checkboxSize - 5, page.Height), XStringFormats.TopLeft);
                        draw(mcAnswer.Content, mcFont, new XRect(margin + checkboxSize + 5, yPoint, innerWidth - checkboxSize - 5, page.Height), "TopLeft");

                        yPoint += mcAnswerHeight + mcSpacing;
                    }

                    // Add Spacing 
                    yPoint += taskSpacing;
                }
            }
        }

        /// <summary>
        /// Draw every taskscontent and taskanswer for the answerexam.
        /// </summary>
        private void drawTaskAnswers(PdfPage page)
        {
            int headercounter = 1;
            List<Task> tasks = Tasks;

            foreach (var task in tasks)
            {
                double titleHeight = MeasureTextHeight(task.TaskName, titleFont, innerWidth);
                double descriptionHeight = MeasureTextHeight(task.TaskContent, taskFont, innerWidth);
                double answerHeight = 0;

                if (!task.TaskAnswer.Content.Equals(null)) 
                {
                    answerHeight = MeasureTextHeight(task.TaskAnswer.Content, taskFont, innerWidth);
                }

                // Add some space between TaskDescription and -answer.
                double finaldescriptionHeight = (2*taskSpacing + answerHeight + descriptionHeight);
                double mcAnswersHeight = 0;

                if (task.TaskType == "Multiple Choice" && task.MCAnswers != null)
                {
                    foreach (var mcAnswer in task.MCAnswers)
                    {
                        if (mcAnswer.AnswerFlag.Equals(1))
                        {
                            mcAnswersHeight += MeasureTextHeight(mcAnswer.Content, mcFont, innerWidth - checkboxSize - 5) + mcSpacing;
                        }
                    }
                }

                double requiredSpace = titleHeight + finaldescriptionHeight + mcAnswersHeight + taskSpacing;

                // Check if there is enough space for the next task frame
                if (yPoint + requiredSpace > pageHeight - margin)
                {
                    CreateHeaderSite(page);
                }

                // Draw the task title and adjust the Tasknumber
                string finalheader = string.Empty;
                finalheader += headercounter;
                headercounter++;
                finalheader += ". " + task.TaskName;
                draw(finalheader, titleFont, new XRect(margin, yPoint, innerWidth, page.Height), "TopLeft");
                yPoint += titleHeight+5;

                // Draw the task description 
                var tf = new XTextFormatter(gfx);
                tf.Alignment = XParagraphAlignment.Justify;
                draw(task.TaskContent, taskFont, new XRect(margin, yPoint, innerWidth, page.Height - yPoint - margin), "TopLeft", tf);
                yPoint += descriptionHeight + taskSpacing; //hier muss noch die Height angepasst werden, wenn der requiredspace > als eine ganze Seite ist 

                // Draw the Taskanswer
                draw(task.TaskAnswer.Content, taskFont, new XRect(margin, yPoint, innerWidth, page.Height - yPoint - margin), "TopLeft", tf);
                yPoint += answerHeight + taskSpacing;
                /*
                 * if(yPoint > pageHeight -margin){
                 *      double y = yPoint;
                 *      CreateHeaderSite(page);
                 *      yPoint = y; 
                 *      while(ypoint > pageHeight - margin)
                 *      {
                 *          yPoint -= pageHeight - margin;
                 *          y = yPoint;
                 *          CreateHeaderSite(page);
                 *          yPoint = y;
                 *      }
                 * }
                 */

                // Draw MC Answers if any one is right 
                if (task.TaskType == "Multiple Choice" && task.MCAnswers != null)
                {
                    foreach (var mcAnswer in task.MCAnswers)
                    {
                        if (mcAnswer.AnswerFlag.Equals(1))
                        {
                            double mcAnswerHeight = MeasureTextHeight(mcAnswer.Content, mcFont, innerWidth - checkboxSize - 5);

                            gfx.DrawRectangle(XPens.Black, XBrushes.White, new XRect(margin, yPoint, checkboxSize, checkboxSize));
                            gfx.DrawString(mcAnswer.Content, mcFont, XBrushes.Black, new XRect(margin + checkboxSize + 5, yPoint, innerWidth - checkboxSize - 5, page.Height), XStringFormats.TopLeft);
                            draw(mcAnswer.Content, mcFont, new XRect(margin + checkboxSize + 5, yPoint, innerWidth - checkboxSize - 5, page.Height), "TopLeft");

                            yPoint += mcAnswerHeight + mcSpacing;
                        }
                    }
                    // Add Spacing 
                    yPoint += taskSpacing;
                }
            }
        }

        /// <summary>
        /// Build up a PDF-page with a header. 
        /// </summary>
        private void CreateHeaderSite(PdfPage page)
        {
            double headerhight = MeasureTextHeight(Examconfig.ExamName, taskFont, innerWidth);
            XPen pen = new XPen(XColors.Black, 1);

            // Add a new page and reset yPoint
            page = document.AddPage();
            gfx = XGraphics.FromPdfPage(page);
            yPoint = margin;

            // Creating a Header.
            gfx.DrawString(Examconfig.ModuleID + " - " + DateTime.Now.Year.ToString(), taskFont, XBrushes.Black, margin, yPoint);
            gfx.DrawString("Name: ", taskFont, XBrushes.Black, innerWidth - margin, yPoint);
            draw(pen, margin, yPoint + headerhight, pageWidth - margin, yPoint + headerhight);
            yPoint += headerhight + 10;
        }

        /// <summary>
        /// Copy all tasks from the configuration to a list to work with them. 
        /// </summary>
        /// <returns> List of task-items. </returns>
        private List<Task> taskCreator()
        {
            // Task-Stuff
            var tasks = new List<Task>();

            // Task-Switch-Content
            foreach (Task task in SelectedTasks)
            {
                tasks.Add(task);
            }

            return tasks;
        }

        /// <summary>
        /// Initialize all the exam-hints. 
        /// </summary>
        /// <returns> List of hints for the database. </returns>
        private List<AdditionalInformation> additionalInformationCreator()
        {
            var information = new List<AdditionalInformation>
            {
                new AdditionalInformation("Vollständigkeit", false, "Überprüfe die Klausur auf Vollständigkeit"),
                new AdditionalInformation("Ausfüllen", false, "Fülle das Deckblatt aus!"),
                new AdditionalInformation("Beschriftung", false, "Beschriften Sie alle Blätter mit Ihrem Namen."),
                new AdditionalInformation("Anzahl Blätter", false, "Tragen Sie die Anzahl beschriebener Blätter auf dem Deckblatt ein."),
                new AdditionalInformation("Materialien & Ausweis", false, "Legen Sie bitte alle für die Klausur benötigten Dinge, Stifte, Verpflegung und insbesondere Lichtbildausweis auf Ihren Tisch."),
                new AdditionalInformation("Handy", false, "Schalten Sie Ihr Mobiltelefon aus!"),
                new AdditionalInformation("Wörterbuch", false, "Erlaubte Hilfsmittel: Taschenrechner, Wörterbuch"),
                new AdditionalInformation("Täuschungsversuch", false, "Täuschungsversuche führen zum Nichtbestehen der Klausur!"),
                new AdditionalInformation("Schreibfelder", false, "Verwenden Sie den Ihnen zur Verfügung stehenden Platz und nutzen Sie die Zusatzblätter."),
                new AdditionalInformation("Schriftbild", false, "Schreiben Sie deutlich und benutzen keine Bleistifte oder Farbstifte!"),

            };
            SaveHintsToDatabase(information);
            return information;
        }

        /// <summary>
        /// Save the hints in the database. 
        /// </summary>
        private void SaveHintsToDatabase(List<AdditionalInformation> hints)
        {
            PathFinder pathFinder = new PathFinder("Databases", "database.db");
            string databasePath = pathFinder.GetPath();
            string connectionString = $"Data Source={databasePath};Version=3;";
            DataService dataService = new DataService(connectionString);

            using (var connection = new SQLiteConnection(connectionString))
            {
                connection.Open();

                foreach (var hint in hints)
                {
                    // Prüfe, ob der Hinweis bereits existiert
                    string checkQuery = "SELECT COUNT(*) FROM hint WHERE name = @Name AND content = @Content";
                    using (var checkCmd = new SQLiteCommand(checkQuery, connection))
                    {
                        checkCmd.Parameters.AddWithValue("@Name", hint.Name);
                        checkCmd.Parameters.AddWithValue("@Content", hint.Content);

                        var count = Convert.ToInt32(checkCmd.ExecuteScalar());

                        // Wenn der Hinweis noch nicht existiert, füge ihn ein
                        if (count == 0)
                        {
                            dataService.InsertHint(hint.Name, hint.Content);
                        }
                    }
                }
            }
        }

        /// <summary>
        /// Measures the required space for a text on the Pdf-page.
        /// </summary>
        /// <returns> Rquired space for the given text. </returns>
        private double MeasureTextHeight(string text, XFont font, double width)
        {
            // Add a temporary page for measuring 
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

        /// <summary>
        /// Split the text in lines if the text hits the borders.
        /// </summary>
        /// <returns> List of Lines that fits in the space between the text-borders. </returns>
        private List<string> SplitTextIntoLines(string text, XFont font, double innerWidth, XGraphics gfx, double pageHeight, double margin)
        {
            var lines = new List<string>();
            var currentLine = new StringBuilder();
            double currentY = 0;

            // Split text into words
            var words = text.Split(new[] { ' ' }, StringSplitOptions.RemoveEmptyEntries);

            foreach (var word in words)
            {
                var proposedSize = gfx.MeasureString(currentLine.ToString() + " " + word, font);

                // Check if adding the new word exceeds innerWidth
                if (proposedSize.Width > innerWidth)
                {
                    lines.Add(currentLine.ToString());
                    currentLine.Clear();
                    currentLine.Append(word);
                    currentY += font.GetHeight();

                    // Check if new line exceeds page height
                    if (currentY + font.GetHeight() > pageHeight - margin)
                    {
                        lines.Add(currentLine.ToString());  //potenzielle Fehlerquelle, falls Aufgabe zu lang ist 
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

        /// <summary>
        /// The user could select in which directory the PDF should be saved.
        /// </summary>
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
                System.Windows.MessageBox.Show($"PDF document has been saved as '{filename}'.", "PDF Saved", MessageBoxButton.OK, MessageBoxImage.Information);
                NavigationService.Navigate(new ToolsPage());
            }
        }

        /// <summary>
        /// Save the PDF in the chosen directory.
        /// </summary>
        private void SavePDFButton_Click(object sender, RoutedEventArgs e)
        {
            if (!string.IsNullOrEmpty(tempFilename) && System.IO.File.Exists(tempFilename))
            {
                SavePDF(tempFilename);
            }
            else
            {
                System.Windows.MessageBox.Show("Please generate the PDF first.", "Error", MessageBoxButton.OK, MessageBoxImage.Error);
            }
        }

        /// <summary>
        /// Shows the Additional-Information content.
        /// </summary>
        private void AdditionalInformationInfoButton(object sender, RoutedEventArgs e)
        {
            System.Windows.Controls.Button button = sender as System.Windows.Controls.Button;

            if (button != null) 
            {
                AdditionalInformation info = button.Tag as AdditionalInformation;

                if (info != null)
                {
                    MessageBox.Show($"{info.Content}", info.Name);
                }
                else
                {
                    MessageBox.Show("Tag ist kein AdditionalInformation-Objekt.");
                }
            }
            else
            {
                MessageBox.Show("Sender ist kein WPF Button.");
            }
        }

        /// <summary>
        /// Draw the specified string onto the PDF page.
        /// </summary>
        private void draw(string text, XFont font, XPoint point)
        {
            gfx.DrawString(text, font, XBrushes.Black, point);
        }

        /// <summary>
        /// Draw the specified string onto the PDF page.
        /// </summary>
        private void draw(string text, XFont font, XRect rect, string format)
        {
            XStringFormat finalFormat = formatchecker(format);
            gfx.DrawString(text, font, XBrushes.Black, rect, finalFormat);
        }

        /// <summary>
        /// Draw the specified string onto the PDF page.
        /// </summary>
        private void draw(string text, XFont font, XRect rect, string format, XTextFormatter tf)
        {
            XStringFormat finalFormat = formatchecker(format);
            tf.DrawString(text, font, XBrushes.Black, rect, finalFormat);
        }

        /// <summary>
        /// Draw the specified string onto the PDF page.
        /// </summary>
        private void draw(XPen pen, double xLeft, double yLeft, double xRight, double yRight)
        {
            gfx.DrawLine(pen, xLeft, yLeft, xRight, yRight);
        }

        /// <summary>
        /// Textalignment options. 
        /// </summary>
        /// <returns> Stringalignment format. </returns>
        private XStringFormat formatchecker(string format)
        {
            // Konvertiere den String in Kleinbuchstaben
            string lowerFormat = format.Trim().ToLower();
            switch (lowerFormat)
            {
                case "center":
                    return XStringFormats.Center;
                case "topcenter":
                    return XStringFormats.TopCenter;
                case "bottomcenter":
                    return XStringFormats.BottomCenter;
                case "topleft":
                    return XStringFormats.TopLeft;
                case "topright":
                    return XStringFormats.TopRight;
                case "bottomleft":
                    return XStringFormats.BottomLeft;
                case "bottomright":
                    return XStringFormats.BottomRight;
                case "baselineleft":
                    return XStringFormats.BaseLineLeft;
                case "baselineright":
                    return XStringFormats.BaseLineRight;
                case "baselinecenter":
                    return XStringFormats.BaseLineCenter;
                default:
                    return XStringFormats.TopLeft;
            }
        }



        /*---------------------------------------------------------*/
        //                  Data-Switch-Area
        /*---------------------------------------------------------*/

        /// <summary>
        /// Drag the selected item.
        /// </summary>
        private void DataGrid_PreviewMouseLeftButtonDown(object sender, MouseButtonEventArgs e)
        {
            var row = FindVisualParent<DataGridRow>(e.OriginalSource as DependencyObject);

            // Check if the mouse is over a scrollbar or button, and avoid starting drag
            if (IsMouseOverScrollbarOrButton(e.OriginalSource as DependencyObject))
            {
                draggedItem = null;
                return; // Stop if mouse is over scrollbar or button
            }

            if (row != null)
            {
                draggedItem = row.Item as UniversityExamCreator.Models.Task;
            }
        }

        /// <summary>
        /// Shows the drag-effect.
        /// </summary>
        private void DataGrid_PreviewMouseMove(object sender, MouseEventArgs e)
        {
            if (draggedItem != null && e.LeftButton == MouseButtonState.Pressed)
            {
                DragDrop.DoDragDrop(dataGrid, draggedItem, DragDropEffects.Move);
                draggedItem = null; // Reset after dragging
            }
        }

        /// <summary>
        /// Changes the Index of the Items to get them in the right position. 
        /// </summary>
        private void DataGrid_Drop(object sender, DragEventArgs e)
        {
            if (e.Data.GetDataPresent(typeof(UniversityExamCreator.Models.Task)))
            {
                var droppedData = e.Data.GetData(typeof(UniversityExamCreator.Models.Task)) as UniversityExamCreator.Models.Task;
                var target = FindVisualParent<DataGridRow>(e.OriginalSource as DependencyObject)?.Item as UniversityExamCreator.Models.Task;

                if (droppedData != null && target != null && droppedData != target)
                {
                    int removedIdx = SelectedTasks.IndexOf(droppedData);
                    int targetIdx = SelectedTasks.IndexOf(target);

                    SelectedTasks.Move(removedIdx, targetIdx);

                    // UpdateDatabaseAfterReorder();
                }
            }
            Tasks = taskCreator();
        }

        private static T FindVisualParent<T>(DependencyObject child) where T : DependencyObject
        {
            DependencyObject parentObject = VisualTreeHelper.GetParent(child);
            if (parentObject == null) return null;

            T parent = parentObject as T;
            return parent ?? FindVisualParent<T>(parentObject);
        }

        /// <summary>
        /// Check if the mouse is over the scrollbar.
        /// </summary>
        private bool IsMouseOverScrollbarOrButton(DependencyObject originalSource)
        {
            while (originalSource != null)
            {
                if (originalSource is System.Windows.Forms.ScrollBar || originalSource is System.Windows.Forms.Button)
                    return true;

                originalSource = VisualTreeHelper.GetParent(originalSource);
            }
            return false;
        }

        // Optional: Aktualisiere die Reihenfolge in der Datenbank
        private void UpdateDatabaseAfterReorder() { }

        /// <summary>
        /// Updates the EmptyLineCount for the specific task. 
        /// </summary>
        private void EmptyLineCountComboBox_SelectionChanged(object sender, System.Windows.Controls.SelectionChangedEventArgs e)
        {
            // Get the ComboBox and the selected item
            var comboBox = sender as System.Windows.Controls.ComboBox;
            var selectedItem = comboBox.SelectedItem as System.Windows.Controls.ComboBoxItem;

            if (selectedItem != null && comboBox.DataContext is Task task)
            {
                // Set the EmptyLineCount based on the selected ComboBox item content
                task.EmptyLineCount = double.Parse(selectedItem.Content.ToString());
            }
        }

        private void ListViewInformation_SelectionChanged(object sender, SelectionChangedEventArgs e)
        {

        }
    }
}