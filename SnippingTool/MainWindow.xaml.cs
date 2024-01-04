using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows;
using System.Windows.Controls;
using System.Windows.Data;
using System.Windows.Documents;
using System.Windows.Forms;
using System.Windows.Input;
using System.Windows.Media;
using System.Windows.Media.Imaging;
using System.Windows.Navigation;
using System.Windows.Shapes;
using Window = System.Windows.Window;

namespace SnippingTool
{
    public partial class MainWindow : Window
    {
        MainWindow mainwindow;
        SecondWindow secondwindow;

        System.Drawing.Point mousepos;
        public string mousepos_str { get; set; }
        double factor;

        public MainWindow()
        {
            InitializeComponent();

        }

        private void btnStart_Click(object sender, RoutedEventArgs e)
        {
            this.Visibility = Visibility.Hidden;

            // Create second transparent window
            int width = System.Windows.Forms.SystemInformation.PrimaryMonitorSize.Width; // (int)System.Windows.SystemParameters.VirtualScreenWidth;
            int height = System.Windows.Forms.SystemInformation.PrimaryMonitorSize.Height; ; // (int)System.Windows.SystemParameters.VirtualScreenHeight;

            secondwindow = new SecondWindow()
            {
                Width = width,
                Height = height,
                Left = 0,
                Top = 0,
                AllowsTransparency = true,
                Opacity = 0.5,
                WindowStyle = WindowStyle.None,
            };

            secondwindow.Show();
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

        private System.Drawing.Point GetMousePositionWindowsForms() => Form.MousePosition;

        private void Window_Closed(object sender, EventArgs e)
        {
            Environment.Exit(0);
        }
    }
}
