using SharpDX;
using SharpDX.Direct3D11;
using SharpDX.DXGI;
using StripController.Infrastructure.StripWrapper;
using System;
using System.Collections.Generic;
using System.Drawing;
using System.Drawing.Imaging;
using System.Threading.Tasks;

namespace StripController.Services.Modes
{
    class VideoCaptureMode : IVideoCaptureMode
    {
        private bool _active;
        
        public event VideoDataUpdatedEventHandler VideoUpdated;

        public IStripper Stripper { get; }

        public VideoCaptureMode(IStripper stripper)
        {
            Stripper = stripper;
        }
                
        public void Start()
        {
            _active = true;

            var factory = new Factory1();
            var adapter = factory.GetAdapter1(0);
            var device = new SharpDX.Direct3D11.Device(adapter);
            var output = adapter.GetOutput(0);
            var output1 = output.QueryInterface<Output1>();

            var width = output.Description.DesktopBounds.Right;
            var height = output.Description.DesktopBounds.Bottom;

            var textureDesc = new Texture2DDescription
            {
                CpuAccessFlags = CpuAccessFlags.Read,
                BindFlags = BindFlags.None,
                Format = Format.B8G8R8A8_UNorm,
                Width = width,
                Height = height,
                OptionFlags = ResourceOptionFlags.None,
                MipLevels = 1,
                ArraySize = 1,
                SampleDescription = { Count = 1, Quality = 0 },
                Usage = ResourceUsage.Staging
            };
            var screenTexture = new Texture2D(device, textureDesc);

            Task.Factory.StartNew(() =>
            {                
                using (var duplicatedOutput = output1.DuplicateOutput(device))
                {
                    while (_active)
                    {
                        try
                        {
                            SharpDX.DXGI.Resource screenResource;
                            OutputDuplicateFrameInformation duplicateFrameInformation;
                                                        
                            duplicatedOutput.AcquireNextFrame(5, out duplicateFrameInformation, out screenResource);
                                                        
                            using (var screenTexture2D = screenResource.QueryInterface<Texture2D>())
                                device.ImmediateContext.CopyResource(screenTexture2D, screenTexture);
                                                        
                            var mapSource = device.ImmediateContext.MapSubresource(screenTexture, 0, MapMode.Read, SharpDX.Direct3D11.MapFlags.None);
                                                        
                            var bitmap = new Bitmap(width, height, System.Drawing.Imaging.PixelFormat.Format32bppArgb);
                            {
                                var boundsRect = new Rectangle(0, 0, width, height);
                                                                
                                var mapDest = bitmap.LockBits(boundsRect, ImageLockMode.WriteOnly, bitmap.PixelFormat);
                                var sourcePtr = mapSource.DataPointer;
                                var destPtr = mapDest.Scan0;

                                for (int y = 0; y < height; y++)
                                {                                    
                                    Utilities.CopyMemory(destPtr, sourcePtr, width * 4);

                                    sourcePtr = IntPtr.Add(sourcePtr, mapSource.RowPitch);
                                    destPtr = IntPtr.Add(destPtr, mapDest.Stride);
                                }
                                                                
                                bitmap.UnlockBits(mapDest);
                                device.ImmediateContext.UnmapSubresource(screenTexture, 0);

                                CalculateColors(bitmap);

                                bitmap.Dispose();
                                bitmap = null;                                
                            }
                            screenResource.Dispose();
                            duplicatedOutput.ReleaseFrame();
                        }
                        catch (SharpDXException e)
                        {                        
                        }
                    }
                }
            });
        }

        public void Stop()
        {
            _active = false;
        }

        private void RaiseVideoUpdatedEvent(IEnumerable<System.Windows.Media.Color> colors)
        {
            VideoUpdated?.Invoke(this, new VideoUpdatedEventArgs(colors));
        }

        private void CalculateColors(Bitmap data)
        {
            var srcData = data.LockBits(
                new Rectangle(0, 0, data.Width, data.Height),
                ImageLockMode.ReadOnly,
                PixelFormat.Format32bppArgb);

            var stride = srcData.Stride;
            var scan0 = srcData.Scan0;

            var accumulator = new long[Stripper.PixelCount, 3];
            var actualPixels = new int[Stripper.PixelCount];

            var width = srcData.Width;
            var height = srcData.Height;

            var pixelWidth = (int)Math.Ceiling(1.0 * width / Stripper.PixelCount);

            unsafe
            {
                byte* rawData = (byte*)(void*)scan0;

                for (int y = 0; y < height; y++)
                {                    
                    for (int x = 0; x < width; x += pixelWidth)
                    {
                        var pixelColor = new long[3];

                        for (int p = 0; p < pixelWidth && x + p < width; p++)
                        {
                            int idx = (y * stride) + (x + p) * 4;

                            pixelColor[0] += rawData[idx + 0];
                            pixelColor[1] += rawData[idx + 1];
                            pixelColor[2] += rawData[idx + 2];
                        }

                        var pixelIndex = x / pixelWidth;
                        accumulator[pixelIndex, 0] += pixelColor[0] / pixelWidth;
                        accumulator[pixelIndex, 1] += pixelColor[1] / pixelWidth;
                        accumulator[pixelIndex, 2] += pixelColor[2] / pixelWidth;

                        if (!(pixelColor[0] / pixelWidth < 30 && pixelColor[1] / pixelWidth < 30 && pixelColor[2] / pixelWidth < 30))
                            actualPixels[pixelIndex]++;
                    }
                }
            }
            data.UnlockBits(srcData);
            
            var displayColors = new System.Windows.Media.Color[Stripper.PixelCount];
            var stripperColor = new Color[Stripper.PixelCount];

            for (int i = 0; i < Stripper.PixelCount; i++)
            {
                if (actualPixels[i] == 0)
                {
                    displayColors[i] = System.Windows.Media.Color.FromRgb(0, 0, 0);
                    stripperColor[i] = Color.FromArgb(0, 0, 0);
                }
                else
                {
                    displayColors[i] = System.Windows.Media.Color.FromRgb(
                        (byte)(accumulator[i, 2] / actualPixels[i]),
                        (byte)(accumulator[i, 1] / actualPixels[i]),
                        (byte)(accumulator[i, 0] / actualPixels[i]));

                    stripperColor[i] = Color.FromArgb(displayColors[i].R, displayColors[i].G, displayColors[i].B);
                }
            }

            Stripper.SetPixelsColor(255, stripperColor);
            RaiseVideoUpdatedEvent(displayColors);            
        }
    }
}
