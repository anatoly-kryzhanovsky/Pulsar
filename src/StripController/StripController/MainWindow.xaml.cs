using System;
using System.ComponentModel;
using System.Linq;
using System.Windows;
using System.Windows.Controls;
using System.Windows.Forms;
using StripController.Services;
using StripController.ViewInterfaces;

namespace StripController
{
    public partial class MainWindow: IMainWindow
    {
        private ICaptureModeView _captureModeView;
        private ICustomColorModeView _customColorModeView;
        private IProgramModeView _programModeView;

        public MainWindow()
        {
            InitializeComponent();

            var notifyIcon = new NotifyIcon
            {
                Icon = new System.Drawing.Icon("icon.ico"),
                Visible = true
            };
            notifyIcon.Click += NotifyIconOnDoubleClick;
        }

        public void SetViewFactory(IViewFactory viewFactory)
        {
            _captureModeView = viewFactory.CreateCaptureColorModeView();
            _captureModeView.LoadState();

            _customColorModeView = viewFactory.CreateCustomCoroModeView();
            _customColorModeView.LoadState();

            _programModeView = viewFactory.CreateProgramModeView();
            _programModeView.LoadState();
            
            Modes.Items.Add(new TabItem
            {
                Content = _captureModeView,
                Header = "Capture color"
            });

            Modes.Items.Add(new TabItem
            {
                Content = _customColorModeView,
                Header = "Custom color"
            });

            Modes.Items.Add(new TabItem
            {
                Content = _programModeView,
                Header = "Program"
            });
        }

        protected override void OnClosing(CancelEventArgs e)
        {
            var tab = Modes.SelectedItem as TabItem;
            if (tab != null)
            {
                var view = tab.Content as IView;
                view?.Deactivate();
            }

            _captureModeView.SaveState();
            _customColorModeView.SaveState();
            _programModeView.SaveState();

            base.OnClosing(e);
        }

        protected override void OnStateChanged(EventArgs e)
        {
            if (WindowState == WindowState.Minimized)
                Hide();

            base.OnStateChanged(e);
        }

        private void NotifyIconOnDoubleClick(object o, EventArgs eventArgs)
        {
            Show();
            WindowState = WindowState.Normal;
        }

        private void Modes_OnSelectionChanged(object sender, SelectionChangedEventArgs e)
        {
            var selected = e.AddedItems
                .OfType<TabItem>()
                .Select(x => x.Content)
                .OfType<IView>()
                .ToArray();

            var deselected = e.RemovedItems
                .OfType<TabItem>()
                .Select(x => x.Content)
                .OfType<IView>()
                .ToArray();

            foreach(var item in deselected)
                item.Deactivate();

            foreach (var item in selected)
                item.Activate();
        }
    }
}
