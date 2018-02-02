using StripController.ViewInterfaces;

namespace StripController.Views
{
    class OpenFileDialog : IOpenFileDialog
    {
        public string FileName { get; private set; }
        public string Filter { get; set; }

        public bool ShowDialog()
        {
            var dialog = new Microsoft.Win32.OpenFileDialog {Filter = Filter};
            var result = dialog.ShowDialog();

            if (result.HasValue && result.Value)
            {
                FileName = dialog.FileName;
                return true;
            }

            return false;
        }
    }
}