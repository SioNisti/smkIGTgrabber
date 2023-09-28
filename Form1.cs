using IronOcr;
using System.Text.RegularExpressions;
using System;
using System.Windows;
using System.Windows.Forms;
using System.Drawing;
using OpenCvSharp;
using OpenCvSharp.Extensions;
using System.Linq;

namespace smkIGTgrabber2
{
    public partial class Form1 : Form
    {
        public Form1()
        {
            InitializeComponent();
        }

        int camera = 3;

        public static int q1 = 485;
        public static int q2 = 12;
        public static int w = 130;
        public static int h = 33;

        System.Drawing.Rectangle rectangle4 = new System.Drawing.Rectangle(q1, q2, w, h);

        Bitmap takePic()
        {
            VideoCapture capture = new VideoCapture(camera);
            Bitmap image;
            capture.Grab();
            var frame = capture.RetrieveMat();
            image = BitmapConverter.ToBitmap(frame);
            image.Save(@"immmimimimi.jpg");

            
            return image;
        }

        string getText()
        {
            Bitmap image = takePic();
            Bitmap image2 = image.Clone(rectangle4, image.PixelFormat);
            var ocr = new IronTesseract();

            using (var ocrInput = new OcrInput())
            {
                ocrInput.AddImage(image2);
                ocrInput.DeNoise();

                ocrInput.SaveAsImages();
                var ocrResult = ocr.Read(ocrInput);

                Console.WriteLine($"a {ocrResult.Text} c {ocrResult.Confidence}");
                return ocrResult.Text;
            }
        }

        public void toIgt(string text)
        {
            string igt = "";

            string numericPhone = new String(text.Where(Char.IsDigit).ToArray());
            Console.WriteLine($"b {numericPhone} {numericPhone.Length}");

            if (numericPhone.Length == 6)
            {
                for (int x = 0; x < 6; x++)
                {
                    if (x == 2)
                    {
                        igt += "\'";
                    }
                    else if (x == 4)
                    {
                        igt += "\"";
                    }
                    igt += numericPhone[x];
                    Console.WriteLine($"igt = {igt}");
                }
            }

            label1.Text = igt;
        }

        private void button1_Click(object sender, EventArgs e)
        {
            toIgt(getText());
        }

        private void button2_Click(object sender, EventArgs e)
        {
            var ocr = new IronTesseract();

            using (var ocrInput = new OcrInput())
            {
                ocrInput.AddImage(@"C:\Users\qmena\source\repos\smkIGTgrabber2\bin\Debug\Untitled3.png");
                ocrInput.DeNoise();
                ocrInput.Binarize();

                var ocrResult = ocr.Read(ocrInput);
                Console.WriteLine($"a {ocrResult.Text}");
                toIgt(ocrResult.Text);
            }
            
        }
    }
}
