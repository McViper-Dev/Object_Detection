using Alturos.Yolo;
using Alturos.Yolo.Model;
using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Drawing;
using System.Drawing.Imaging;
using System.IO;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Forms;

namespace Object_Detection
{
    public partial class Form1 : Form
    {
        public Form1()
        {
            InitializeComponent();
        }

        private void button1_Click(object sender, EventArgs e)
        {
            var file = new OpenFileDialog();
            file.Filter = "Image Files | *.jpg;*.png";
            if(file.ShowDialog() == DialogResult.OK)
            {
                pictureBox1.Image = Image.FromFile(file.FileName);
            }
        }

        private void button2_Click(object sender, EventArgs e)
        {
            var con = new ConfigurationDetector();
            var config = con.Detect();
            var dectect = new YoloWrapper(config); //if you want custom detection/trainm it your self  do var yoloCustomWrapper = new YoloWrapper("<<path to .cfg file>>", "<<path to .weights file>>", "<<path to .names file>>");
            var mS = new MemoryStream();
            pictureBox1.Image.Save(mS, ImageFormat.Png);
            var item = dectect.Detect(mS.ToArray()).ToList();
            AddRect(pictureBox1, item);
        }
        void AddRect(PictureBox pictureR, List<YoloItem> items)
        {
            var imgM = pictureR.Image; //for custom training ya know do this here <<< var items = yoloCustomWrapper.Detect(@"image.jpg");
            var graph = Graphics.FromImage(imgM);
            var font = new Font("Arial", 50, FontStyle.Regular);
            var Fbrush = new SolidBrush(Color.Yellow);
            foreach(var item in items)
            {
                var xAxis = item.X;
                var yAxis = item.Y;
                var width = item.Width;
                var height = item.Height;
                var rectangle = new Rectangle(xAxis, yAxis, width, height);
                var pen = new Pen(Color.Red, 12);
                var point = new Point(xAxis, yAxis);
                graph.DrawRectangle(pen, rectangle);
                graph.DrawString(item.Type, font, Fbrush, point);
            }
            pictureR.Image = imgM;
        }
    }
}
