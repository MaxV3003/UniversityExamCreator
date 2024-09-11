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
using System.IO;

namespace UniversityExamCreator.Views
{
    public partial class ExamPreview : System.Windows.Controls.Page //System.Windows.Controls. sonst entfernen
    {
        // Configs
        private string tempFilename;
        Examconfig Examconfig { get; set; }
        public List<Task> Tasks { get; set; }

        // PDF-Document & Drawing 
        PdfSharp.Pdf.PdfDocument document;
        PdfSharp.Drawing.XGraphics gfx;

        //Test-Area
        List <String> Fonts { get; set; }

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

        // Page settings
        const double pageHeight = 842;
        const double pageWidth = 600;
        const double margin = 60;
        const double innerWidth = pageWidth - 2 * margin;
        const double taskSpacing = 20; // Decreased spacing between tasks
        const double mcSpacing = 10; // Increased spacing between MC answers
        const double checkboxSize = 10;
        double yPoint = 100;

        internal ExamPreview(Examconfig examconfig, List<Task> tasks)
        {
            Examconfig = examconfig;
            Tasks = tasks;
            InitializeComponent();

            // Initialisiere die Fonts-Liste
            Fonts = new List<string>
            {
                "Verdana",
                "Times New Roman",
                "Arial",
            };

            // Fill FontCombobox.
            FontComboBox.ItemsSource = Fonts;

            // Initialisiere die Schriftgrößen in der Combobox
            for (int i = 9; i <= 72; i += 1)
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

        }

        /// <summary>
        /// Selcetionchanger if there is an other Item selected.
        /// </summary>
        private void FontComboBox_SelectionChanged(object sender, SelectionChangedEventArgs e)
        {
            string selectedFont = FontComboBox.SelectedItem as string;

            if (selectedFont != null)
            {
                //in Extraklasse machen, die auf alle Fontarten und deren Schriftgröße gleichzeitig zugreift. Wenn eine der beiden Attribute geändert wird soll auch nur ein Methodenaufruf dafür notwendig sein.
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
        }

        /// <summary>
        /// Set the selected FontSize-Value.
        /// </summary>
        private void setFontSize(int size, string font)
        {
            switch (font)
            {
                case "examTitelFont":
                    examTitelFont = new XFont(defaultFont, size);
                    break;
                case "titleFont":
                    titleFont = new XFont(defaultFont, size);
                    break;
                case "taskFont":
                    taskFont = new XFont(defaultFont, size);
                    mcFont = new XFont(defaultFont, size);
                    break;
            }
        }

        /*---------------------------------------------------------------------------------------------------*/
        //                                  PDF-Erzeugungs-Abschnitt
        /*---------------------------------------------------------------------------------------------------*/
        
        private void Button_Click(object sender, RoutedEventArgs e)
        {
            NavigationService.Navigate(new ExamCreate(Examconfig));
        }

        private void GeneratePDFButton_Click(object sender, RoutedEventArgs e)
        {
            PdfSharp.Pdf.PdfDocument newDocument = new PdfDocument()
            {
                Info = { Title = "Klausur Vorschau" }
            };
            document = newDocument;
            yPoint = 100;

            // Initialize page and graphics
            PdfPage page = document.AddPage();
            gfx = XGraphics.FromPdfPage(page);

            // Logo laden und einfügen
            //XImage logo = XImage.FromFile("C:/Users/Max/source/repos/UniversityExamCreator/UniversityExamCreator/Models/OVGU-FIN_farbig.jpg");
            string basePath = AppDomain.CurrentDomain.BaseDirectory;
            string projectDirectory = Path.GetFullPath(Path.Combine(basePath, @"..\..\"));
            string relativePath = @"Models\OVGU-FIN_farbig.jpg";
            string fullPath = Path.Combine(projectDirectory, relativePath);

            // Lade das Bild
            XImage logo = XImage.FromFile(fullPath);

            gfx.DrawImage(logo, 150, 0, 300, 100);

            // Draw the Exam Header
            var examHeader = new XTextFormatter(gfx);
            string leftSideText = "Dr. ...";
            DateTime specificDateTime = new DateTime(2024, 12, 12);
            string rightSideText = "Magdeburg, " + specificDateTime.ToString("dd.MM.yyyy");
            XSize rightTextSize = gfx.MeasureString(rightSideText, taskFont);
            double xRightPosition = margin + innerWidth - rightTextSize.Width;
            gfx.DrawString(leftSideText, taskFont, XBrushes.Black, new XPoint(margin, yPoint));
            gfx.DrawString(rightSideText, taskFont, XBrushes.Black, new XPoint(xRightPosition, yPoint));
            yPoint += taskSpacing;


            // Draw the exam name with wrapping
            var examNameRect = new XRect(margin, yPoint, innerWidth, pageHeight - yPoint - margin);
            DrawWrappedText(Examconfig.ExamName, examTitelFont, examNameRect, out double textHeight);
            yPoint += textHeight; // Problem: The Textheight isn't right after Measurement. If the Text is to long it will overlap the MetaTable.

            //Draw the rest of the Sheet
            drawCoverSheet(page);
            yPoint = pageHeight;
            drawTasks(page);


            // Save the document to a temporary file
            tempFilename = System.IO.Path.Combine(System.IO.Path.GetTempPath(), "KlausurVorschau.pdf");
            document.Save(tempFilename);
            System.Diagnostics.Process.Start(new System.Diagnostics.ProcessStartInfo(tempFilename) { UseShellExecute = true });
        }

        private void DrawWrappedText(string text, XFont font, XRect rect, out double textHeight)
        {
            var tf = new XTextFormatter(gfx);
            tf.Alignment = XParagraphAlignment.Center;

            // Measure text height to fit the rect
            var size = gfx.MeasureString(text, font, XStringFormats.TopLeft);
            textHeight = size.Height;

            // Draw text with wrapping
            tf.DrawString(text, font, XBrushes.Black, rect, XStringFormats.TopLeft);

        }

        private void drawCoverSheet(PdfPage page)
        {
            drawMetaTable();
            drawTasksTable(page);
            drawAdditionalInformation(page);
        }

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
                    gfx.DrawString(data[row, col], taskFont, XBrushes.Black, new XRect(x + col * cellWidth, y + (row + 1) * cellHeight, cellWidth, cellHeight), XStringFormats.TopLeft);

                }
                yPoint += cellHeight;
            }
            yPoint += 2 * cellHeight;
        }

        private void drawTasksTable(PdfPage page)
        {
            double cellHeight = 20;
            double defaultCellWidth = 60;

            // Erstelle die Tabelle-Daten
            string[,] data = new string[Tasks.Count + 1, 4]; // +1 für die Kopfzeile

            // Kopfzeile
            string[] headers = { " # ", " Aufgabe ", " max. Punkte ", " erreicht " };
            for (int i = 0; i < headers.Length; i++)
            {
                data[0, i] = headers[i];
            }

            // Datenzeilen
            int a = 1; // Beginne mit der zweiten Zeile, da die erste Zeile die Kopfzeile ist
            foreach (var task in Tasks)
            {
                data[a, 0] = (a).ToString();  // Index anpassen, damit es korrekt angezeigt wird
                data[a, 1] = task.TaskName;
                data[a, 2] = task.Points.ToString();
                data[a, 3] = "";
                a++;
            }

            // Berechne die maximale Breite der zweiten Spalte
            double secondColumnWidth = CalculateMaxCellWidthInSecondColumn(data, taskFont, defaultCellWidth);

            // Gesamte Breite der Tabelle berechnen
            double totalTableWidth = defaultCellWidth * 3 + secondColumnWidth;

            // Zeichnen des Hinweises
            string noticeText = "Diese Tabelle bitte nicht ausfüllen!";
            gfx.DrawString(noticeText, taskFont, XBrushes.Black,
                           new XRect(margin, yPoint, innerWidth, taskFont.Height),
                           XStringFormats.Center);
            yPoint += taskFont.Height + 2;

            // Zentrieren der Tabelle auf der Seite
            double x = (page.Width - totalTableWidth) / 2;
            double y = yPoint;

            // Zeichnen der Tabelle
            for (int row = 0; row < data.GetLength(0); row++)
            {
                double currentX = x; // Startpunkt der X-Koordinate für jede Zeile

                for (int col = 0; col < data.GetLength(1); col++)
                {
                    double currentCellWidth = (col == 1) ? secondColumnWidth : defaultCellWidth;

                    // Bestimme Formatierung für Kopfzeile (erste Zeile)
                    if (row == 0)
                    {
                        // Zeichne die Kopfzeile mit spezieller Formatierung
                        gfx.DrawRectangle(XPens.Black, XBrushes.LightGray, currentX, y, currentCellWidth, cellHeight);
                        gfx.DrawString(data[row, col], taskFont, XBrushes.Black, new XRect(currentX, y, currentCellWidth, cellHeight), XStringFormats.Center);
                    }
                    else
                    {
                        // Zeichne die Datenzeilen mit Standardformatierung
                        gfx.DrawRectangle(XPens.Black, currentX, y, currentCellWidth, cellHeight);
                        gfx.DrawString(data[row, col], taskFont, XBrushes.Black,
                                       new XRect(currentX, y, currentCellWidth, cellHeight),
                                       XStringFormats.Center);
                    }

                    // Aktualisiere die X-Koordinate für die nächste Spalte
                    currentX += currentCellWidth;
                }

                // Erhöhe yPoint nach dem Zeichnen der Zeile, damit die nächste Zeile korrekt positioniert wird
                y += cellHeight;
            }

            // Zusätzlicher Abstand nach der Tabelle
            yPoint = y + 2 * cellHeight;
        }

        private void drawAdditionalInformation(PdfPage page)
        {
            double titelHigth = MeasureTextHeight("Hinweise", titleFont, innerWidth);

            gfx.DrawString("Zusätzliche Hinweise", titleFont, XBrushes.Black, new XRect(margin, yPoint, innerWidth, page.Height), XStringFormats.TopCenter);
            yPoint += titelHigth + taskSpacing / 2;

            List<String> additionalInformation = additionalInformationCreator();

            foreach (var info in additionalInformation)
            {
                string bulletPoint = "\u2022 "; // Unicode für den Punkt (•)
                string bulletText = bulletPoint + info;

                double infoHigth = MeasureTextHeight(bulletText, taskFont, innerWidth / 2);

                if (yPoint + infoHigth > pageHeight - margin)
                {
                    // Neue Seite hinzufügen und yPoint zurücksetzen
                    page = document.AddPage();
                    gfx = XGraphics.FromPdfPage(page);
                    yPoint = margin; // Zurücksetzen auf den oberen Rand der neuen Seite
                }

                var tf = new XTextFormatter(gfx)
                {
                    Alignment = XParagraphAlignment.Justify // Setzt den Text im Blocksatz
                };

                // Berechnung der X-Position, um den Text in der Mitte der Seite anzuzeigen
                double xPosition = (page.Width - innerWidth / 2) / 2;
                var rect = new XRect(xPosition, yPoint, innerWidth / 2, page.Height - yPoint - margin);

                tf.DrawString(bulletText, taskFont, XBrushes.Black, rect, XStringFormats.TopLeft);
                yPoint += infoHigth + 2; // Etwas Platz zwischen den Stichpunkten
            }
        }

        private double getCellWidth(string text, XFont font, double cellWidth)
        {
            // Erzeuge einen Grafik-Kontext zum Messen des Textes
            using (var gfx = XGraphics.CreateMeasureContext(
                new XSize(1000, 1000),               // Größe des Messbereichs
                XGraphicsUnit.Point,                 // Maßeinheit
                XPageDirection.Downwards))           // Seitenausrichtung
            {
                XSize size = gfx.MeasureString(text, font);
                if (size.Width < cellWidth)
                {
                    return cellWidth;
                }
                else
                {
                    return size.Width; // Gibt die tatsächliche Breite des Textes zurück
                }
            }
        }

        private double CalculateMaxCellWidthInSecondColumn(string[,] data, XFont font, double cellWidth)
        {
            double maxWidth = 0;

            for (int row = 1; row < data.GetLength(0); row++) // Beginne bei 1, um die Kopfzeile zu überspringen
            {
                double currentWidth = getCellWidth(data[row, 1], font, cellWidth);
                if (currentWidth > maxWidth)
                {
                    maxWidth = currentWidth;
                }
            }

            return maxWidth + 10; // Padding hinzufügen
        }

        private void drawTasks(PdfPage page)
        {
            List<Task> tasks = taskCreator();

            //hier werden die Tasks aus der ExamCreate überführt 
            foreach (var task in Tasks)
            {
                tasks.Add(task);
            }

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
                    yPoint = margin;

                    // Creating a Header.
                    double headerhight = MeasureTextHeight(Examconfig.ExamName, taskFont, innerWidth);
                    gfx.DrawString(Examconfig.ModuleID + " - " + DateTime.Now.Year.ToString(), taskFont, XBrushes.Black, margin, yPoint);
                    gfx.DrawString("Name: ", taskFont, XBrushes.Black, innerWidth - margin, yPoint);
                    XPen pen = new XPen(XColors.Black, 1);
                    gfx.DrawLine(pen, margin, yPoint + headerhight, pageWidth - margin, yPoint + headerhight);
                    yPoint += headerhight + 10; // Reset to top margin of the new page
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
        }

        private List<Task> taskCreator()
        {
            ///Task-Stuff
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

            return tasks;
        }

        private List<String> additionalInformationCreator()
        {
            var information = new List<String>();

            information.Add("Überprüfe die Klausur auf Vollständigkeit.");
            information.Add("Fülle das Deckblatt aus!");
            information.Add("Beschriften Sie alle Blätter mit Ihrem Namen.");
            information.Add("Tragen Sie die Anzahl beschriebener Blätter auf dem Deckblatt ein.");
            information.Add("Legen Sie bitte alle für die Klausur benötigten Dinge, Stifte, Verpflegung und insbesondere Lichtbildausweis auf Ihren Tisch.");
            information.Add("Schalten Sie Ihr Mobiltelefon aus!");
            information.Add("Erlaubte Hilfsmittel: Taschenrechner, Wörterbuch");
            information.Add("Täuschungsversuche führen zum Nichtbestehen der Klausur!");
            information.Add("Verwenden Sie den Ihnen zur Verfügung stehenden Platz und nutzen Sie die Zusatzblätter.");
            information.Add("Schreiben Sie deutlich und benutzen keine Bleistifte oder Farbstifte!");

            return information;
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
    }
}
