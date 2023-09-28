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

        int camera = 3; //obs virtual camera

        public static int q1 = 485; //X of igt area
        public static int q2 = 12; //y of igt area
        public static int w = 130; //igt area width
        public static int h = 33; //igt area height

        System.Drawing.Rectangle rectangle4 = new System.Drawing.Rectangle(q1, q2, w, h);

        Bitmap takePic()
        {
            VideoCapture capture = new VideoCapture(camera);
            Bitmap image;
            capture.Grab();
            var frame = capture.RetrieveMat();
            image = BitmapConverter.ToBitmap(frame);
            image.Save(@"immmimimimi.jpg"); //save pic so you can see what it saw

            
            return image;
        }

        string getText()
        {
            Bitmap image = takePic();
            Bitmap image2 = image.Clone(rectangle4, image.PixelFormat);  //crop image with the earlier defined x,y,w,h
            var ocr = new IronTesseract();

            using (var ocrInput = new OcrInput())
            {
                ocrInput.AddImage(image2);
                ocrInput.DeNoise();

                ocrInput.SaveAsImages(); //save image so you can see what ocr saw
                var ocrResult = ocr.Read(ocrInput);

                Console.WriteLine($"a {ocrResult.Text} c {ocrResult.Confidence}");
                return ocrResult.Text;
            }
        }

        public void toIgt(string text)
        {
            string igt = "";

            string numericPhone = new String(text.Where(Char.IsDigit).ToArray()); //only take the numbers from the text it found
            Console.WriteLine($"b {numericPhone} {numericPhone.Length}");

            if (numericPhone.Length == 6) //go through the found numbers and format it to xx'xx"xx
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
    }
}
