using System;
using System.Collections.Generic;
using System.Linq;
using System.Windows;
using System.Windows.Controls;
using System.Windows.Input;
using System.Windows.Media;
using StripController.PresentationEntities;
using Xceed.Wpf.Toolkit;

namespace StripController.Views
{
    public delegate void GradientChangedDelegate(object sender, GradientChangedEventArgs args);

    public partial class GradientPicker
    {
        private const double MinMovementDelta = 15;
        private const double MinValueDelta = 0.1;

        private readonly GradientSelectorPe _displayObject;
        private readonly Dictionary<GradientPointPe, Button> _pointButtons;
        private readonly Style _buttonStyle;

        private double _clickPosition;
        private bool _isMoved;
        private bool _isDeleted;
        private bool _initialized;

        public event GradientChangedDelegate GradientChanged;

        public IEnumerable<GradientPointPe> Gradient
        {
            get { return NormolizeGradient(); }
            set
            {
                _displayObject.Points.Clear();
                _pointButtons.Clear();
                ThumbContainer.Children.Clear();

                foreach (var itm in value)
                {
                    var btn = CreateThumbButton(itm.Value, 0);
                    var pe = (GradientPointPe)btn.Tag;
                    pe.Color = itm.Color;
                }

                UpdateButtons();
                UpdateBrush();
            }
        }

        public GradientPicker()
        {
            InitializeComponent();

            _displayObject = new GradientSelectorPe();
            _pointButtons = new Dictionary<GradientPointPe, Button>();
            _buttonStyle = FindResource("ThumbButton") as Style;
            
            DataContext = _displayObject;
        }

        protected override void OnRenderSizeChanged(SizeChangedInfo sizeInfo)
        {
            base.OnRenderSizeChanged(sizeInfo);
            if (!_initialized)
            {
                _initialized = true;
                Initialize();
            }

            UpdateButtons();
        }

        private void BtnOnMouseUp(object sender, MouseButtonEventArgs mouseButtonEventArgs)
        {
            var btn = sender as Button;
            if (btn == null)
                return;

            btn.ReleaseMouseCapture();

            var pe = btn.Tag as GradientPointPe;
            if (pe == null)
                return;

            if (!_isMoved)
            {
                ColorPicker.SelectedColor = pe.Color;
                ColorPicker.Tag = pe;

                ColorPickerPopup.PlacementTarget = btn;
                ColorPickerPopup.IsOpen = true;
            }

            _isMoved = false;
        }

        private void BtnOnMouseMove(object sender, MouseEventArgs mouseEventArgs)
        {
            var btn = sender as Button;
            if (btn == null)
                return;

            if (btn.IsMouseCaptureWithin)
            {
                var position = mouseEventArgs.GetPosition(this);
                if (position.X < 0 || position.X >= ActualWidth)
                    return;

                if (!_isMoved && Math.Abs(_clickPosition - position.X) > MinMovementDelta)
                    _isMoved = true;

                if (!_isMoved)
                    return;

                var pe = btn.Tag as GradientPointPe;
                if (pe == null)
                    return;

                pe.Value = position.X / ActualWidth;
                Canvas.SetLeft(btn, position.X - btn.Width / 2);
                UpdateBrush();
            }
        }

        private void BtnOnMouseDown(object sender, MouseButtonEventArgs mouseButtonEventArgs)
        {
            var btn = sender as Button;
            if (btn == null)
                return;

            _isDeleted = false;

            if (Keyboard.IsKeyDown(Key.LeftCtrl) || Keyboard.IsKeyDown(Key.RightCtrl))
            {
                if (_displayObject.Points.Count <= 2)
                    return;

                var pe = btn.Tag as GradientPointPe;
                if (pe == null)
                    return;

                ThumbContainer.Children.Remove(btn);
                _pointButtons.Remove(pe);
                _displayObject.Points.Remove(pe);
                UpdateBrush();

                _isDeleted = true;

                return;
            }
                
            _clickPosition = mouseButtonEventArgs.GetPosition(this).X;

            btn.CaptureMouse();
        }

        private void OnSelectedColorChanged(object sender, RoutedPropertyChangedEventArgs<Color?> e)
        {
            var picker = sender as ColorCanvas;
            var pe = picker?.Tag as GradientPointPe;
            if (pe == null)
                return;

            if (!e.NewValue.HasValue)
                return;

            if (!ColorPickerPopup.IsOpen)
                return;

            pe.Color = e.NewValue.Value;
            UpdateBrush();
        }

        private void ThumbContainer_OnMouseUp(object sender, MouseButtonEventArgs e)
        {
            if (_isDeleted)
            {
                _isDeleted = false;
                return;
            }

            var position = e.GetPosition(this);
            var value = position.X / Preview.ActualWidth;

            if (_displayObject.Points.Any(x => Math.Abs(x.Value - value) < MinValueDelta))
                return;

            CreateThumbButton(value, position.X);
            UpdateBrush();
        }

        private void UpdateBrush()
        {
            var brush = new LinearGradientBrush();
            foreach (var point in _displayObject.Points)
            {
                brush.GradientStops.Add(new GradientStop(point.Color, point.Value));
            }

            _displayObject.Brush = brush;
            RaiseGradientChangedEvent();
        }

        private void Initialize()
        {
            if (_displayObject.Points.Count < 2)
            {
                CreateThumbButton(0, 0);
                CreateThumbButton(1, ActualWidth - 1);
            }

            UpdateBrush();
        }

        private void UpdateButtons()
        {
            foreach (var point in _displayObject.Points)
            {
                var button = _pointButtons[point];
                Canvas.SetLeft(button, point.Value * Preview.ActualWidth - button.Width / 2);
            }
        }

        private Button CreateThumbButton(double value, double position)
        {
            var btn = new Button {Style = _buttonStyle};
            ThumbContainer.Children.Add(btn);

            Canvas.SetLeft(btn, position - btn.Width / 2);

            btn.PreviewMouseDown += BtnOnMouseDown;
            btn.PreviewMouseMove += BtnOnMouseMove;
            btn.PreviewMouseUp += BtnOnMouseUp;
            
            var pe = new GradientPointPe { Value = value };

            var nearestPoint = _displayObject.Points
                .Select((p, i) => new {Point = p, Index = i})
                .OrderBy(x => Math.Abs(x.Point.Value - pe.Value)).FirstOrDefault();

            pe.Color = nearestPoint?.Point.Color ?? Colors.Green;

            btn.Tag = pe;

            if (nearestPoint != null)
            {
                if (nearestPoint.Point.Value < value)
                    _displayObject.Points.Insert(nearestPoint.Index + 1, pe);
                else
                    _displayObject.Points.Insert(nearestPoint.Index, pe);
            }
            else
                _displayObject.Points.Add(pe);

            _pointButtons.Add(pe, btn);
            return btn;
        }

        private IEnumerable<GradientPointPe> NormolizeGradient()
        {
            var items = _displayObject.Points.ToList();
            var firstItem = items.First();
            var lastItem = items.Last();

            if (firstItem.Value > 0)
                items.Insert(0, new GradientPointPe {Color = firstItem.Color, Value = 0});

            if (lastItem.Value < 1)
                items.Add(new GradientPointPe {Color = lastItem.Color, Value = 1});

            return items;
        }

        private void RaiseGradientChangedEvent()
        {
            GradientChanged?.Invoke(this, new GradientChangedEventArgs(NormolizeGradient()));
        }
    }
}