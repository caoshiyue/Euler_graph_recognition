using System;
using System.Collections.Generic;
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
using System.Windows.Navigation;
using System.Windows.Shapes;
using System.Windows.Threading;
using System.Runtime.InteropServices;
using System.Windows.Interop;
using generate_graph;
using MathWorks.MATLAB.NET.Arrays;
using System.Diagnostics;

namespace discrete_mathematics3
{
    /// <summary>
    /// MainWindow.xaml 的交互逻辑
    /// </summary>
    public partial class MainWindow : Window
    {
        gg ggraph = new gg();
        string infilesPath;
        bool findwindow = false;
        Microsoft.Win32.OpenFileDialog ofd = new Microsoft.Win32.OpenFileDialog();
        PluginContainer plugin;
        public MainWindow()
        {
            InitializeComponent();
            _start.IsEnabled = false;
        }
        private void Start(object sender, RoutedEventArgs e)
        {
            MWCharArray cellArray = infilesPath;
            MWArray majm = ggraph.recognition(cellArray);
            int[,] ajm = ConvertDouble((double[,])majm.ToArray());
            if (!findwindow)
            {
                IntPtr hostWinHandle = ((HwndSource)PresentationSource.FromVisual(mainBorder)).Handle;
                plugin = new PluginContainer("recognition", hostWinHandle);
                mainBorder.Child = plugin;
                Point point = grid2.TranslatePoint(new Point(), grid0);
                findwindow = plugin.EmbedProcess((int)grid2.Width, (int)grid2.Height, point);
            }
            try
            {
                imshow.Visibility = Visibility.Collapsed;
                Win32API.ShowWindow(plugin.tarwinhandle, 1);
            }
            catch {; }
            Eulardll.initadj(ajm.GetLength(0), ref ajm[0, 0]);
            try
            {
                IntPtr p;
                string eularpath;
                p = Marshal.AllocHGlobal(20);
                Action act = new Action(() =>
                {
                    Eulardll.getpath(ref p);
                });
                act.Invoke();
                eularpath = Marshal.PtrToStringAnsi(p);
                if (eularpath == "")
                    eularpath = "无欧拉通路或回路";
                pathout.Text = eularpath;
            }
            catch {; }
        }
        private void Input(object sender, RoutedEventArgs e)
        {
            ofd.DefaultExt = System.Environment.CurrentDirectory;
            ofd.Filter = "image file|*.jpg;*.png";
            if (ofd.ShowDialog() == true)
            {
                    Uri uri = new Uri(ofd.FileName);
                    BitmapImage bitmap = new BitmapImage(uri);
                try
                {
                    imshow.Visibility = Visibility.Visible;
                    Win32API.ShowWindow(plugin.tarwinhandle, 0);
                }
                catch {; }
                imshow.Source = bitmap;
                infilesPath = ofd.FileName;
                _start.IsEnabled = true;
                //DoEvents();
            }
        }
        /// <summary>
        ///  Double[,] to int[,]
        /// </summary>
        /// <param name="a"></param>
        /// <returns></returns>
        private int[,] ConvertDouble(double[,] a)
        {
            var x = a.GetLength(0);
            var y = a.GetLength(1);
            int[,] result = new int[x, y];
            for (var i = 0; i < x; i++)
                for (var j = 0; j < y; j++)
                    result[i, j] = (int)a[i, j];
            return result;
        }
    }
}
