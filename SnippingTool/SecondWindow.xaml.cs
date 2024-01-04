using System;
using System.Collections.Generic;
using System.Drawing;
using System.IO;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows;
using System.Windows.Controls;
using System.Windows.Data;
using System.Windows.Documents;
using System.Windows.Input;
using System.Windows.Media;
using System.Windows.Media.Imaging;
using System.Windows.Shapes;
using Brushes = System.Windows.Media.Brushes;
using Point = System.Windows.Point;
using Rectangle = System.Windows.Shapes.Rectangle;

namespace SnippingTool
{
    /// <summary>
    /// Логика взаимодействия для Window1.xaml
    /// </summary>
    public partial class SecondWindow : Window
    {
        bool mclicked;
        Point mstartpoint = new Point();
        Rectangle rectangle;
        Thickness myThickness;
        int w, h;
        double factor;

        public SecondWindow()
        {
            InitializeComponent();


        }

        private void secondwindow_MouseDown(object sender, MouseButtonEventArgs e)
        {
            mclicked = true;

            mstartpoint = e.GetPosition(grid);
            grid.Children.Clear();
            rectangle = new Rectangle();
            rectangle.Stroke = Brushes.Red;
            myThickness = new Thickness();

            factor = System.Windows.PresentationSource.FromVisual(this).CompositionTarget.TransformToDevice.M11;

            if (e.RightButton == MouseButtonState.Pressed)
            {
                Environment.Exit(0);
            }
        }

        private void secondwindow_MouseUp(object sender, MouseButtonEventArgs e)
        {
            mclicked = false;

            if (w <= 0 || h <= 0) return;

            secondwindow.Close();
        }

        private void secondwindow_MouseMove(object sender, MouseEventArgs e)
        {
            if (mclicked)
            {
                grid.Children.Clear();

                var mcurrentpoint = e.GetPosition(grid);
                w = (int)(mcurrentpoint.X - mstartpoint.X);
                h = (int)(mcurrentpoint.Y - mstartpoint.Y);

                double x = 0, y = 0;

                if (w > 0 && h > 0)
                {
                    x = mstartpoint.X;
                    y = mstartpoint.Y;
                }
                else if (w < 0 && h < 0)
                {
                    x = mcurrentpoint.X;
                    y = mcurrentpoint.Y;
                }
                else if (w < 0 && h > 0)
                {
                    x = mcurrentpoint.X;
                    y = mstartpoint.Y;
                }
                else if (w > 0 && h < 0)
                {
                    x = mstartpoint.X;
                    y = mcurrentpoint.Y;
                }

                w = Math.Abs(w);
                h = Math.Abs(h);

                myThickness.Left = x;
                myThickness.Top = y;

                rectangle.Margin = myThickness;
                rectangle.Width = w;
                rectangle.Height = h;

                grid.Children.Add(rectangle);


            }
        }

        private void secondwindow_Closed(object sender, EventArgs e)
        {
            int width = (int)(w * factor);
            int height = (int)(h * factor);



            // Снимок части экрана
            Bitmap bmp = new Bitmap(width, height);
            using (Graphics g = Graphics.FromImage(bmp))
            {
                g.CopyFromScreen((int)(myThickness.Left * factor), (int)(myThickness.Top * factor), 0, 0, new System.Drawing.Size(width, height));
            }
            Clipboard.SetImage(BitmapToImageSource(bmp));


            MainWindow mainwindow = new MainWindow();
            mainwindow.Visibility = Visibility.Visible;
            mainwindow.img.Source = BitmapToImageSource(bmp);
        }

        private BitmapImage BitmapToImageSource(System.Drawing.Bitmap bitmap)
        {
            using (MemoryStream memory = new MemoryStream())
            {
                bitmap.Save(memory, System.Drawing.Imaging.ImageFormat.Bmp);
                memory.Position = 0;
                BitmapImage bitmapimage = new BitmapImage();
                bitmapimage.BeginInit();
                bitmapimage.StreamSource = memory;
                bitmapimage.CacheOption = BitmapCacheOption.OnLoad;
                bitmapimage.EndInit();
                return bitmapimage;
            }
        }
    }
}
