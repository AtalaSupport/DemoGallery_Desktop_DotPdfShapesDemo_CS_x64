using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Drawing;
using System.Linq;
using System.Text;
using System.Windows.Forms;
using Atalasoft.PdfDoc.Generating;
using Atalasoft.PdfDoc.Generating.Shapes;
using Atalasoft.PdfDoc.Geometry;
using System.IO;

namespace PdfShapesDemo
{
    public partial class Form1 : Form
    {
        public Form1()
        {
            InitializeComponent();
        }

        private void button1_Click(object sender, EventArgs e)
        {
            MakeDrawingShapePdfDocument();
        }

        string _arial;
        private void MakeDrawingShapePdfDocument()
        {
            MessageBox.Show("The demo is going to create a Pdf document from a series of shapes.  When it is finished processing, the generated Pdf will be launched in your default Pdf reader.");

            PdfGeneratedDocument doc = new PdfGeneratedDocument();
            _arial = doc.Resources.Fonts.AddFromFontName("Arial");
            doc.Pages.Add(MakePdfArcPage());
            doc.Pages.Add(MakePdfCirclePage());
            doc.Pages.Add(MakePdfPathPage());
            doc.Pages.Add(MakePdfRectanglePage());
            doc.Save(GetShapesFilePath());

            System.Diagnostics.Process.Start(GetShapesFilePath());
        }

        private Atalasoft.PdfDoc.BasePage MakePdfArcPage()
        {
            PdfGeneratedPage page = PdfDefaultPages.Letter;
            AddTitle(page, "PdfArc");
            PdfArc arc = new PdfArc(new PdfPoint(page.MediaBox.Width / 2, page.MediaBox.Height / 2), 100, 90, 240, PdfColorFactory.FromColor(Color.Green));
            page.DrawingList.Add(arc);
            return page;
        }


        private Atalasoft.PdfDoc.BasePage MakePdfCirclePage()
        {
            PdfGeneratedPage page = PdfDefaultPages.Letter;
            AddTitle(page, "PdfCircle");
            PdfCircle circle = new PdfCircle(new PdfPoint(page.MediaBox.Width / 2, page.MediaBox.Height / 2),100,PdfColorFactory.FromColor(Color.Firebrick));
            page.DrawingList.Add(circle);
            return page;
        }

        private Atalasoft.PdfDoc.BasePage MakePdfPathPage()
        {
            PdfGeneratedPage page = PdfDefaultPages.Letter;
            AddTitle(page, "PdfPath");
            PdfPath path = new PdfPath(PdfColorFactory.FromColor(Color.Orange), 2.0);
            path.MoveTo(200, 200);
            path.LineTo(400, 400);
            path.LineTo(300, 600);
            path.LineTo(200, 400);
            path.LineTo(200, 200);
            path.LineTo(400, 200);
            path.LineTo(400, 400);
            path.LineTo(200, 400);
            path.LineTo(400, 200);
            page.DrawingList.Add(path);
            return page;
        }

        private Atalasoft.PdfDoc.BasePage MakePdfRectanglePage()
        {
            PdfGeneratedPage page = PdfDefaultPages.Letter;
            AddTitle(page, "PdfRectangle");
            PdfRectangle rect = new PdfRectangle(new PdfBounds(200, 500, 200, 100), PdfColorFactory.FromColor(Color.CadetBlue));
            PdfRoundedRectangle rRect = new PdfRoundedRectangle(new PdfBounds(200,300,200,100), 20, PdfColorFactory.FromColor(Color.Chocolate)); 
            page.DrawingList.Add(rect);
            page.DrawingList.Add(rRect);
            return page;
        }


        private void button2_Click(object sender, EventArgs e)
        {
            MakeTextShapes();
        }

        string _times;
        private void MakeTextShapes()
        {
            MessageBox.Show("The demo is going to create a Pdf document from a series of text operations.  When it is finished processing, the generated Pdf will be launched in your default Pdf reader.");

            PdfGeneratedDocument doc = new PdfGeneratedDocument();
            _arial = doc.Resources.Fonts.AddFromFontName("Arial");
            _times = doc.Resources.Fonts.AddFromFontName("Times New Roman");
            doc.Pages.Add(MakeDynamicTextBox(doc.Resources));
            doc.Pages.Add(MakePdfClippedText(doc.Resources));
            doc.Pages.Add(MakePdfStyledText(doc.Resources));
            doc.Pages.Add(MakePdfTextPath(doc.Resources));
            doc.Save(GetTextFilePath());

            System.Diagnostics.Process.Start(GetTextFilePath());
        }

        private Atalasoft.PdfDoc.BasePage MakeDynamicTextBox(Atalasoft.PdfDoc.Generating.ResourceHandling.GlobalResources globalResources)
        {
            PdfGeneratedPage page = PdfDefaultPages.Letter;
            AddTitle(page, "DynamicPdfTextBox");
            DynamicPdfTextBox text = new DynamicPdfTextBox(new PdfPoint(0, 0), _arial, 12, page.MediaBox.Width, page.MediaBox.Height - 300, "This is a DynamicPdfText box. It takes a width and a maximum height and some text and it provides the minimum size required to fit the text on to the page. As you can see this helps greatly while center aligning text and drawing a rectangle around the text", globalResources.Fonts);
            text.Alignment = PdfTextAlignment.Center;
            text.Location = new PdfPoint((page.MediaBox.Width - text.MinimumBounds.Width) / 2.0,page.MediaBox.Height-300);
            page.DrawingList.Add(text);
            PdfRectangle rect = new PdfRectangle(new PdfBounds(text.Location.X, text.Location.Y - text.MinimumBounds.Height, text.MinimumBounds.Width, text.MinimumBounds.Height), PdfColorFactory.FromColor(Color.Brown), 1.0);
            page.DrawingList.Add(rect);
            return page;
        }

        private Atalasoft.PdfDoc.BasePage MakePdfClippedText(Atalasoft.PdfDoc.Generating.ResourceHandling.GlobalResources globalResources)
        {
            PdfGeneratedPage page = PdfDefaultPages.Letter;
            AddTitle(page, "PdfClippedTextLine");
            PdfClippedTextLine text = new PdfClippedTextLine("This text does not fit", _arial,new PdfBounds(100,page.MediaBox.Top - 300, 200,40),globalResources.Fonts);
            page.DrawingList.Add(text);
            return page;
        }

        private Atalasoft.PdfDoc.BasePage MakePdfStyledText(Atalasoft.PdfDoc.Generating.ResourceHandling.GlobalResources globalResources)
        {
            PdfGeneratedPage page = PdfDefaultPages.Letter;
            AddTitle(page, "PdfStyledTextBox");

            PdfStyledTextBox text = new PdfStyledTextBox(_arial, new PdfBounds(0, 0, page.MediaBox.Width, page.MediaBox.Height - 300));
            StyleTextInput input = new StyleTextInput(_arial);
            input.AddText("This is a PdfStyledTextBox. Its primary purpose is to be able to ");
            input.ChangeFont(_times);
            input.AddText("change the font");
            input.ChangeFontSize(30);
            input.AddText(", its size");
            input.ChangeFontColor(PdfColorFactory.FromColor(Color.LawnGreen));
            input.AddText(" and other settings of the font.");
            input.ChangeFont(_arial);
            input.ChangeFontSize(12);
            input.ChangeFontColor(PdfColorFactory.FromColor(Color.Black));
            input.AddText(" It automatically deals with any formatting to the box and fills the text automatically. Use a StyledTextInput to call the Fill method to add styled text to this textbox object");
            text.Fill(input, globalResources.Fonts);
            page.DrawingList.Add(text);
            return page;
        }

        private Atalasoft.PdfDoc.BasePage MakePdfTextPath(Atalasoft.PdfDoc.Generating.ResourceHandling.GlobalResources globalResources)
        {
            PdfGeneratedPage page = PdfDefaultPages.Letter;
            AddTitle(page, "PdfTextPath");
            PdfTextPath text = new PdfTextPath(_arial, 20);
            text.MoveTo(new PdfPoint(200, 200));
            text.LineTo(new PdfPoint(200, 400));
            text.CurveTo(new PdfPoint(300, 300), new PdfPoint(300, 300), new PdfPoint(400, 400));
            text.LineTo(new PdfPoint(400, 200));
            text.Text = "The PdfTextPath writes text along a path. Use MoveTo CurveTo LineTo";
            page.DrawingList.Add(text);
            return page;
        }



        private void AddTitle(PdfGeneratedPage page, string p)
        {
            page.DrawingList.Add(new PdfTextLine(_arial, 32, p, new PdfPoint(100,page.MediaBox.Top-100)));
        }

        private static string GetShapesFilePath()
        {
            return Path.Combine(Path.GetTempPath(), "dotpdf-shapes.pdf");
        }

        private static string GetTextFilePath()
        {
            return Path.Combine(Path.GetTempPath(), "dotpdf-text.pdf");
        }

        private void exitToolStripMenuItem_Click(object sender, EventArgs e)
        {
            Application.Exit();
        }

        private void helpAboutToolStripMenuItem_Click(object sender, EventArgs e)
        {
            AtalaDemos.AboutBox.About aboutBox = new AtalaDemos.AboutBox.About("About Atalasoft DotPdf Simple Shape Demo", "DotPdf Simple Shape Demo");
            aboutBox.Description = "This is a minimal application that provides two buttons - one of which generates a markup-based PDF using some of our built in drawing shapes, and one of which generates a vector-based PDF using various Text shapes.  The source code should provide a good example to get you started with laying out basic text and shapes in your own PDFs. \r\n\r\n" +
                                   "Drawing shapes demonstrated: PdfArc, PdfCircle, PdfPath (a wireframe type drawing object), and two PdfRectangle examples (one with square corners, one with rounded corners. \r\n\r\n" +
                                   "Text shapes demonstrated: DynamicPdfTextBox, PdfClippedTextLine, PdfStyledTextBox (a text box that can handle StyledTextInput), and PdfTextPath (a way to have a line of text follow an arbitrary path)."; 
            aboutBox.ShowDialog();
        }
    }
}
