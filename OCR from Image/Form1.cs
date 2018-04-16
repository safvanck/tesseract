using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Drawing;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Forms;
using Tesseract;

namespace OCR_from_Image
{
    public partial class Form1 : Form
    {
        string path;

        public Form1()
        {
            InitializeComponent();
        }

        private void Form1_Load(object sender, EventArgs e)
        {
            path = new System.IO.DirectoryInfo(Environment.CurrentDirectory).Parent.Parent.FullName;
        }

        private string browseImg()
        {
            System.IO.Stream myStream = null;
            OpenFileDialog openFileDialog1 = new OpenFileDialog();

            openFileDialog1.InitialDirectory = "C:\\";
            openFileDialog1.Filter = "Image files (*.jpg, *.jpeg, *.jpe, *.jfif, *.png) | *.jpg; *.jpeg; *.jpe; *.jfif; *.png";
            openFileDialog1.FilterIndex = 2;
            openFileDialog1.RestoreDirectory = true;

            if (openFileDialog1.ShowDialog() == DialogResult.OK)
            {
                try
                {
                    if ((myStream = openFileDialog1.OpenFile()) != null)
                    {
                        using (myStream)
                        {
                            return openFileDialog1.FileName;
                        }
                    }
                }
                catch (Exception ex)
                {
                    MessageBox.Show("Could not read file! Error: " + ex.Message);
                }
            }
            return ("" + path + @"\sample.png");
        }

        private void button1_Click(object sender, EventArgs e)
        {
            var testImagePath = browseImg();// @"C:\Users\project\Downloads\IMGsampleForOCR.png";
            
            var dataPath =""+ path+ @"\tessdata\";

            try
            {
                using (var tEngine = new TesseractEngine(dataPath, "eng", EngineMode.Default)) //creating the tesseract OCR engine with English as the language
                {
                    using (var img = Pix.LoadFromFile(testImagePath)) // Load of the image file from the Pix object which is a wrapper for Leptonica PIX structure
                    {
                        pictureBox1.Image = Image.FromFile(testImagePath);//show image

                        using (var page = tEngine.Process(img)) //process the specified image
                        {
                            var text = page.GetText(); //Gets the image's content as plain text.
                            label1.Text = text.ToString();
                            // Console.WriteLine(page.GetMeanConfidence()); //Get's the mean confidence that as a percentage of the recognized text.

                            
                        }
                    }
                }
            }
            catch (Exception ex)
            {
                label1.Text = "error!"+ ex.Message.ToString();
               // Console.WriteLine("Unexpected Error: " + e.Message);
            }

            
        }

        private void groupBox1_Enter(object sender, EventArgs e)
        {

        }
    }
}
