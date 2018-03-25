using System;
using System.Collections.Generic;
using System.Linq;
using System.Windows;
using System.Windows.Controls;
using System.Windows.Media;
using StripController.ViewInterfaces;

namespace StripController.Views
{
    public partial class VideoCaptureModeView : IVideoCaptureModeView
    {
        private Border[] _pixels;

        public event EventHandler Activated;
        public event EventHandler Deactivated;
        public event EventHandler LoadStateRequested;
        public event EventHandler SaveStateRequested;

        public VideoCaptureModeView()
        {
            InitializeComponent();
        }

        public void UpdateVisual(IReadOnlyCollection<Color> colors)
        {            
            var sourceColros = colors.ToArray();
            if(_pixels == null || _pixels.Length != sourceColros.Length)
            {
                color.ColumnDefinitions.Clear();
                color.Children.Clear();
                _pixels = new Border[sourceColros.Length];

                for(int i = 0; i < sourceColros.Length; i++)
                {
                    color.ColumnDefinitions.Add(new ColumnDefinition());
                    var pixel = new Border
                    {
                        CornerRadius = new CornerRadius(4),
                        BorderThickness = new Thickness(1),
                        BorderBrush = new SolidColorBrush(Color.FromRgb(0, 0, 0)),
                        Margin = new Thickness(2)
                    };
                    Grid.SetColumn(pixel, i);

                    color.Children.Add(pixel);
                    _pixels[i] = pixel;
                }
            }

            for (int i = 0; i < sourceColros.Length; i++)
            {
                var size = color.ActualWidth / sourceColros.Length - (_pixels[i].Margin.Left + _pixels[i].Margin.Right);
                _pixels[i].Background = new SolidColorBrush(sourceColros[i]);
                _pixels[i].Width = size;
                _pixels[i].Height = size;
            }
        }        

        public void Activate()
        {
            RaiseActivatedEvent();
        }

        public void Deactivate()
        {
            RaiseDeactivatedEvent();
        }

        public void LoadState()
        {
            
        }

        public void SaveState()
        {
            
        }

        private void RaiseActivatedEvent()
        {
            Activated?.Invoke(this, EventArgs.Empty);
        }

        private void RaiseDeactivatedEvent()
        {
            Deactivated?.Invoke(this, EventArgs.Empty);
        }      
    }
}
