using System;
using System.Collections.Generic;
using System.Diagnostics;
using System.Drawing;
using System.Drawing.Imaging;
using System.IO;
using System.Threading;
using System.Threading.Tasks;
using System.Windows;
using System.Windows.Controls;
using System.Windows.Forms;
using System.Windows.Media;
using System.Windows.Media.Imaging;
using StripController.PresentationEntities;
using StripController.ViewInterfaces;

namespace StripController.Views
{
    public partial class VideoCaptureModeView : IVideoCaptureModeView
    {
        public VideoCaptureModeView()
        {
            InitializeComponent();
        }

        public event EventHandler Activated;
        public event EventHandler Deactivated;
        public event EventHandler LoadStateRequested;
        public event EventHandler SaveStateRequested;

        public void Activate()
        {
            color.Children.Clear();
            for(int i = 0; i < 50; i++)
            {
                color.ColumnDefinitions.Add(new ColumnDefinition());
                var border = new Border();
                Grid.SetColumn(border, i);
                color.Children.Add(border);
            }

            new Task(() =>
            {
                var bmpScreenshot = new Bitmap(
                    Screen.PrimaryScreen.Bounds.Width,
                    Screen.PrimaryScreen.Bounds.Height,
                    System.Drawing.Imaging.PixelFormat.Format32bppArgb);

                var gfxScreenshot = Graphics.FromImage(bmpScreenshot);

                while (true)
                {
                    Stopwatch sw = new Stopwatch();
                    sw.Start();

                    gfxScreenshot.CopyFromScreen(
                        Screen.PrimaryScreen.Bounds.X,
                        Screen.PrimaryScreen.Bounds.Y,
                        0,
                        0,
                        Screen.PrimaryScreen.Bounds.Size,
                        CopyPixelOperation.SourceCopy);

                    var srcData = bmpScreenshot.LockBits(
                        new System.Drawing.Rectangle(0, 0, bmpScreenshot.Width, bmpScreenshot.Height),
                        ImageLockMode.ReadOnly,
                        System.Drawing.Imaging.PixelFormat.Format32bppArgb);

                    int stride = srcData.Stride;
                    IntPtr Scan0 = srcData.Scan0;

                    long[,] totals = new long[50, 3];

                    int width = srcData.Width;
                    int height = srcData.Height;

                    unsafe
                    {
                        byte* p = (byte*)(void*)Scan0;

                        for (int y = 0; y < height; y++)
                        {
                            for (int x = 0; x < width; x++)
                            {
                                for (int color = 0; color < 3; color++)
                                {
                                    int idx = (y * stride) + x * 4 + color;

                                    totals[(int)(x * 50.0 / width), color] += p[idx];
                                }
                            }
                        }
                    }
                    bmpScreenshot.UnlockBits(srcData);

                    var colors = new System.Windows.Media.Color[50];
                    for (int i = 0; i < 50; i++)
                    {
                        colors[i] = System.Windows.Media.Color.FromRgb(
                            (byte)(totals[i, 2] / (width / 50 * height)),
                            (byte)(totals[i, 1] / (width / 50 * height)),
                            (byte)(totals[i, 0] / (width / 50 * height)));
                    }
                    sw.Stop();

                    Dispatcher.BeginInvoke((Action)(() =>
                    {
                        label.Text = sw.Elapsed.ToString();

                        for (int i = 0; i < colors.Length; i++)
                            (color.Children[i] as Border).Background = new SolidColorBrush(colors[i]);
                    }));

                    
                }
            }).Start();
        }

        public void Deactivate()
        {
            
        }

        public void LoadState()
        {
            
        }

        public void SaveState()
        {
            
        }
    }
}
